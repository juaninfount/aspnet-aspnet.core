using Microsoft.Extensions.Options;
using SocialMedia.Infraestructure.Interfaces;
using SocialMedia.Infraestructure.Options;
using System.Security.Cryptography;
using System;
using System.Linq;

namespace SocialMedia.Infraestructure.Services
{
    public class PasswordHasherService : IPasswordService
    {
        private readonly PasswordOptions _options;

        public PasswordHasherService(IOptions<PasswordOptions> options)
        {
            _options = options.Value;
        }

        public bool Check(string hash, string password)
        {

            if (string.IsNullOrEmpty(hash) || string.IsNullOrWhiteSpace(hash)) 
            {
                throw new FormatException("Unexpected hash format");
            }
            
            var parts = hash.Split('.');
            if(parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt       = Convert.FromBase64String(parts[1]);
            var key        = Convert.FromBase64String(parts[2]);

            // Hash PBKDF2 
            using(var algorithm = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                var keyToCheck = algorithm.GetBytes(_options.KeySize);
                return keyToCheck.SequenceEqual(key); // comparar elemento por elemento
            }            
        }

        public string Hash(string password)
        {
            // Hash PBKDF2 
            using(var algorithm = new Rfc2898DeriveBytes(password, _options.SaltSize, _options.Iterations))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(_options.KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt); 

                return $"{_options.Iterations}.{salt}.{key}";
            }
        }
    }
}