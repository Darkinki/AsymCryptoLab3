using System;
using System.Numerics;
using System.Security.Cryptography;

namespace AsymCryptoLab3
{
    class RabinCryptosystem
    {
        public class KeyPair
        {
            public BigInteger PublicKey { get; set; }
            public (BigInteger P, BigInteger Q) PrivateKey { get; set; }
        }

        public static KeyPair KeyGen(int bitLength)
        {
            BigInteger p, q;
            do { p = MathUtils.GenerateRandomPrime(bitLength / 2); } while (p % 4 != 3);
            do { q = MathUtils.GenerateRandomPrime(bitLength / 2); } while (q % 4 != 3);

            BigInteger n = p * q;

            return new KeyPair
            {
                PublicKey = n,
                PrivateKey = (p, q)
            };
        }

        public static byte[] Encrypt(byte[] message, BigInteger publicKey)
        {
            BigInteger messageInt = new BigInteger(message);
            BigInteger ciphertext = BigInteger.ModPow(messageInt, 2, publicKey);
            return ciphertext.ToByteArray();
        }

        public static byte[] Decrypt(byte[] ciphertext, (BigInteger P, BigInteger Q) privateKey)
        {
            BigInteger ciphertextInt = new BigInteger(ciphertext);
            BigInteger[] roots = Decrypt(ciphertextInt, privateKey);


            return roots[0].ToByteArray();
        }

        private static BigInteger[] Decrypt(BigInteger ciphertext, (BigInteger P, BigInteger Q) privateKey)
        {
            BigInteger p = privateKey.P;
            BigInteger q = privateKey.Q;

            BigInteger mp = BigInteger.ModPow(ciphertext, (p + 1) / 4, p);
            BigInteger mq = BigInteger.ModPow(ciphertext, (q + 1) / 4, q);

            return MathUtils.KTO(mp, mq, p, q);
        }
        /// םו ןנאצ‏÷
        public static BigInteger Sign(BigInteger message, (BigInteger P, BigInteger Q) privateKey)
        {
            BigInteger[] roots = Decrypt(message, privateKey);
            return roots[0];
        }

        public static bool Verify(BigInteger message, BigInteger signature, BigInteger publicKey)
        {
            BigInteger verifiedMessage = BigInteger.ModPow(signature, 2, publicKey);
            return message == verifiedMessage;
        }
    }
}
