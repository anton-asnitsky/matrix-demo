using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TaskAPI.Common
{
    public static class Utils
    {
        public static string PasswordToHash(string password)
        {
            /** STEP 1 Create the salt value with a cryptographic PRNG: */
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            /* STEP 2 Create the Rfc2898DeriveBytes and get the hash value */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);

            /* STEP 3 Combine the salt and password bytes for later use: */
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string savedPasswordHash, string passwordToVerify)
        {
            /* STEP 1 Extract the bytes */
            var hashBytes = Convert.FromBase64String(savedPasswordHash);

            /* STEP 2 Get the salt */
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            /* STEP 3 Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(passwordToVerify, salt, 10000);
            var hash = pbkdf2.GetBytes(20);

            /* STEP 4 Compare the results */
            for (var i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }

        public static string GuidEncode(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .Substring(0, 22);
        }
    }
}
