using System;
using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;

namespace Authentication_Service.Logic
{
    public class SecurityLogic
    {
        private readonly Argon2Config _argon2Config = new Argon2Config
        {
            Type = Argon2Type.DataIndependentAddressing,
            Version = Argon2Version.Nineteen,
            Threads = Environment.ProcessorCount,
        };
        private readonly RandomNumberGenerator _randomNumberGenerator;
        public SecurityLogic()
        {
            _randomNumberGenerator = RandomNumberGenerator.Create();
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(password);
            }

            byte[] salt = new byte[32];
            Argon2Config argon2Config = _argon2Config;
            argon2Config.Password = Encoding.UTF8.GetBytes(password);
            argon2Config.Salt = salt;

            _randomNumberGenerator.GetBytes(salt);
            var argon2 = new Argon2(argon2Config);

            using SecureArray<byte> secureArray = argon2.Hash();
            argon2.Dispose();

            return argon2Config.EncodeString(secureArray.Buffer);
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                throw new ArgumentNullException(password ?? hash);
            }

            return Argon2.Verify(hash, Encoding.UTF8.GetBytes(password));
        }
    }
}
