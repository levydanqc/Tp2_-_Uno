using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tp2___A21
{
    public static class Utilitaires
    {
        public static Random aleatoire = new Random();

        //public static byte[] SaltMotDePasse()
        //{
        //    var buffer = new byte[16];
        //    var rng = new RNGCryptoServiceProvider();
        //    rng.GetBytes(buffer);
        //    return buffer;
        //}

        //public static byte[] HashMotDePasse(string password, byte[] salt)
        //{
        //    var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        //    {
        //        Salt = salt,
        //        DegreeOfParallelism = 8,
        //        Iterations = 4,
        //        MemorySize = 1024 * 1024
        //    };

        //    return argon2.GetBytes(16);
        //}

        //public static bool VerifierMdp(string password, byte[] salt, byte[] hash)
        //{
        //    return hash.SequenceEqual(HashMotDePasse(password, salt));
        //}
    }
}
