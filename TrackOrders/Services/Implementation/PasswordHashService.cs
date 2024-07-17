using System.Security.Cryptography;
using TrackOrders.Services.Interfaces;

namespace TrackOrders.Services.Implementation
{
    public class PasswordHashService : IPasswordHashService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int ITERATIONS = 10000;

        public (bool verified, bool needsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.');

            if (parts.Length != 3)
            {
                throw new Exception();
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != ITERATIONS;

            using (var algorithm = new Rfc2898DeriveBytes(
              password,
              salt,
              iterations))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);

                var verified = keyToCheck.SequenceEqual(key);

                return (verified, needsUpgrade);
            }

        }

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, ITERATIONS))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{ITERATIONS}.{salt}.{key}";
            }
        }
    }
}
