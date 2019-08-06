namespace Gateway.Contracts.Interfaces
{
    public interface ICryptor
    {
        string Encrypt(string plaintext);
        string Decrypt(string encryptedData);
    }
}