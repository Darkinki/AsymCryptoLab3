using AsymCryptoLab3;
using System;
using System.Numerics;
using System.Text;
using static AsymCryptoLab3.RabinCryptosystem;

namespace AsymCryptolab3;


internal class Program
{
    public static void Main(string[] args)
    {

        var keyPair = RabinCryptosystem.KeyGen(512);
        Console.WriteLine($"Public Key (n): {keyPair.PublicKey}");
        Console.WriteLine($"Private Key (p, q): ({keyPair.PrivateKey.P}, {keyPair.PrivateKey.Q})");

        string messageString = "Ad impossibilia nemo tenetur.";
        byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);

        byte[] ciphertext = RabinCryptosystem.Encrypt(messageBytes, keyPair.PublicKey);
        Console.WriteLine($"Ciphertext: {Convert.ToBase64String(ciphertext)}");

        byte[] decryptedMessage = RabinCryptosystem.Decrypt(ciphertext, keyPair.PrivateKey);
        Console.WriteLine($"Decrypted Message: {Encoding.UTF8.GetString(decryptedMessage)}");



    }

}
