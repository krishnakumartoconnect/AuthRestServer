using System.Text;
using System.Threading;
using Fido2NetLib;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;

using Microsoft.AspNetCore.Mvc;

public class FidoController : Controller
{
    private readonly IFido2 _fido2;
    private readonly Dictionary<string, CredentialCreateOptions> _regOptions = new Dictionary<string, CredentialCreateOptions>();
    public static readonly DevelopmentInMemoryStore DemoStorage = new();

    public FidoController(IFido2 fido2)
    {
        _fido2 = fido2;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthenticatorAttestationRawResponse attestationResponse, CancellationToken cancellationToken)
    {
        var username = "testuser"; // get from session or request
        if (!_regOptions.TryGetValue(username, out var options))
            return BadRequest("No registration options found for user");

        // 2. Create callback so that lib can verify credential id is unique to this user
        IsCredentialIdUniqueToUserAsyncDelegate callback = static async (args, cancellationToken) =>
        {
            var users = await DemoStorage.GetUsersByCredentialIdAsync(args.CredentialId, cancellationToken);
            if (users.Count > 0)
                return false;

            return true;
        };
        
        // 2. Verify and make the credentials
        var credential = await _fido2.MakeNewCredentialAsync(new MakeNewCredentialParams
        {
            AttestationResponse = attestationResponse,
            OriginalOptions = options,
            IsCredentialIdUniqueToUserCallback = callback
        }, cancellationToken: cancellationToken);

        // 3. Store the credentials in db
        DemoStorage.AddCredentialToUser(options.User, new StoredCredential
        {
            Id = credential.Id,
            PublicKey = credential.PublicKey,
            UserHandle = credential.User.Id,
            SignCount = credential.SignCount,
            AttestationFormat = credential.AttestationFormat,
            RegDate = DateTimeOffset.UtcNow,
            AaGuid = credential.AaGuid,
            Transports = credential.Transports,
            IsBackupEligible = credential.IsBackupEligible,
            IsBackedUp = credential.IsBackedUp,
            AttestationObject = credential.AttestationObject,
            AttestationClientDataJson = credential.AttestationClientDataJson
        });

        // 4. return "ok" to the client
        return Json(credential);
    }

    [HttpPost("request")]
    public IActionResult RequestNewCredential([FromForm] string username,
                                            [FromForm] string displayName,
                                            [FromForm] string attType,
                                            [FromForm] string authType,
                                            [FromForm] string residentKey,
                                            [FromForm] string userVerification)
    {
        if (string.IsNullOrEmpty(username))
        {
            username = $"{displayName} (Usernameless user created at {DateTime.UtcNow})";
        }

        // 1. Get user from DB by username (in our example, auto create missing users)
        var user = DemoStorage.GetOrAddUser(username, () => new Fido2User
        {
            DisplayName = username,
            Name = username,
            Id = Encoding.UTF8.GetBytes(username) // byte representation of userID is required
        });

        // 2. Get user existing keys by username
        var existingKeys = DemoStorage.GetCredentialsByUser(user).Select(c => c.Descriptor).ToList();

        // 3. Create options
        var authenticatorSelection = new AuthenticatorSelection
        {
            ResidentKey = residentKey.ToEnum<ResidentKeyRequirement>(),
            UserVerification = userVerification.ToEnum<UserVerificationRequirement>()
        };

        if (!string.IsNullOrEmpty(authType))
            authenticatorSelection.AuthenticatorAttachment = authType.ToEnum<AuthenticatorAttachment>();

        var exts = new AuthenticationExtensionsClientInputs()
        {
            Extensions = true,
            UserVerificationMethod = true,
            CredProps = true
        };


        var options = _fido2.RequestNewCredential(new RequestNewCredentialParams { User = user, 
        ExcludeCredentials = existingKeys, AuthenticatorSelection = authenticatorSelection, 
        AttestationPreference = attType.ToEnum<AttestationConveyancePreference>(), 
        Extensions = exts });
        
        // Store the options for later use during the registration flow
        _regOptions[username] = options;

        // Return the options to the client to be used for registration
        return Json(options);
    }
}

