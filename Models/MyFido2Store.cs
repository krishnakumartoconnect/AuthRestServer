using Fido2NetLib;
using Fido2NetLib.Objects;

public class MyFido2Store : IFido2Store
{
    public Task<FidoStoredCredential> GetCredentialByIdAsync(byte[] id)
    {
        throw new NotImplementedException();
    }

    public Task<List<FidoStoredCredential>> GetCredentialsByUserHandleAsync(byte[] userHandle)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FidoStoredCredential>> GetCredentialsByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsCredentialIdUniqueToUserAsync(byte[] credentialId, Fido2User user)
    {
        throw new NotImplementedException();
    }

    public Task StoreCredentialAsync(FidoStoredCredential credential)
    {
        throw new NotImplementedException();
    }

    public Task<FidoStoredCredential> StoreCredentialAsync(AuthenticatorAttestationRawResponse? result)
    {
        throw new NotImplementedException();
    }

    public Task<FidoStoredCredential> UpdateCounterAsync(FidoStoredCredential creds)
    {
        throw new NotImplementedException();
    }
}
