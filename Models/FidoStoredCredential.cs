using Fido2NetLib.Objects;

public class FidoStoredCredential
{
    // Required by Fido2NetLib
    public byte[] CredentialId { get; set; } = Array.Empty<byte>();
    public byte[] PublicKey { get; set; } = Array.Empty<byte>();
    public byte[] UserId { get; set; } = Array.Empty<byte>();
    public byte[] UserHandle { get; set; } = Array.Empty<byte>();
    public uint SignatureCounter { get; set; }
    public string CredType { get; set; } = "public-key";
    public Guid AaGuid { get; set; } = Guid.Empty;
    public DateTime RegDate { get; set; } = DateTime.UtcNow;
    public string Username { get; set; } = "";

    // For login
    public PublicKeyCredentialDescriptor Descriptor => new PublicKeyCredentialDescriptor(CredentialId);
    public string CredentialIdBase64 => Convert.ToBase64String(CredentialId);
}
