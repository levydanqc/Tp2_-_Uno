using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Tp2___A21
{
    public static class Utilitaires
    {
        public static Random Aleatoire = new Random();

        public static byte[] SaltMotDePasse()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        public static byte[] HashMotDePasse(string pPassword, byte[] pSalt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(pPassword))
            {
                Salt = pSalt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 1024 * 1024
            };

            return argon2.GetBytes(16);
        }

        public static bool VerifierMdp(string pPassword, byte[] pSalt, byte[] pHash)
        {
            return pHash.SequenceEqual(HashMotDePasse(pPassword, pSalt));
        }
    }
}
