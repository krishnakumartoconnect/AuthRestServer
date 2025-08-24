using Fido2NetLib;
using Fido2NetLib.Objects;

public interface IFido2Store
{
    Task<FidoStoredCredential> GetCredentialByIdAsync(byte[] id);
    Task<List<FidoStoredCredential>> GetCredentialsByUserHandleAsync(byte[] userHandle);
    Task StoreCredentialAsync(FidoStoredCredential credential);
    Task<IEnumerable<FidoStoredCredential>> GetCredentialsByUsernameAsync(string username);
     Task<bool> IsCredentialIdUniqueToUserAsync(byte[] credentialId, Fido2User user);
    Task<FidoStoredCredential> StoreCredentialAsync(AuthenticatorAttestationRawResponse? result);
    Task<FidoStoredCredential> UpdateCounterAsync(FidoStoredCredential creds);
}
