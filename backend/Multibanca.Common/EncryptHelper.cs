using System.Security.Cryptography;
using System.Text;

namespace Multibanca.Common
{
    public class EncryptHelper
    {
        private static readonly int SALTSIZE = 48;

        private static string ComputeSalt()
        {
            // Generate a cryptographic random number using the cryptographic
            // service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] buff = new byte[SALTSIZE];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff).Substring(0, SALTSIZE);
        }


        /// <summary>
        /// Metodo que encripta el password del usuario.
        /// </summary>
        /// <param name="pwd">password de usuario.</param>
        /// <param name="salt">Salto de bytes.</param>
        /// <returns>String con contraseña encriptada.</returns>
        private static string GenerateHash(string pwd, string salt)
        {
            var hashTool = new SHA512Managed();
            var saltAndPwd = string.Concat(pwd, salt);

            Byte[] passwordAsByte = Encoding.UTF8.GetBytes(saltAndPwd);
            Byte[] encryptedBytes = hashTool.ComputeHash(passwordAsByte);
            Byte[] desncryptedBytes = hashTool.Hash;
            var hashedPwd1 = Convert.ToBase64String(desncryptedBytes);

            hashTool.Clear();

            var hashedPwd = String.Concat(Convert.ToBase64String(encryptedBytes), salt);
            return hashedPwd;
        }


        public static string GenerateHash(string pwd)
        {
            return GenerateHash(pwd, ComputeSalt());
        }

        public static bool ValidateHash(string encryptedPwd, string enteredPwd)
        {
            string salt = encryptedPwd.Substring(encryptedPwd.Length - SALTSIZE);

            string hashToValidate = GenerateHash(enteredPwd, salt);

            return encryptedPwd.Equals(hashToValidate);
        }
    }
}
