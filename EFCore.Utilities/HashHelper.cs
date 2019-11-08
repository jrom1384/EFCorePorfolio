using System.Security.Cryptography;
using System.Text;

namespace EFCore.Utilities
{
    public static class HashHelper
    {
        public static HashResult Create(string value, out byte[] hash, out byte[] salt)
        {
            hash = null;
            salt = null;

            if (value == null)
            {
                return HashResult.Error_NullHash;
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                return HashResult.Error_EmptyOrWhiteSpaceHash;
            }

            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }

            return HashResult.Success;
        }

        public static HashResult Verify(string hash, byte[] storedHash, byte[] storedSalt)
        {
            if (hash == null)
            {
                return HashResult.Error_NullHash;
            }
            else if (string.IsNullOrWhiteSpace(hash))
            {
                return HashResult.Error_EmptyOrWhiteSpaceHash;
            }
            else if (storedHash.Length != 64)
            {
                return HashResult.Error_HashLengthNot64;
            }
            else if (storedSalt.Length != 128)
            {
                return HashResult.Error_SaltLengthNot128;
            }

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                for (int index = 0; index < computedHash.Length; index++)
                {
                    if (computedHash[index] != storedHash[index])
                    {
                        return HashResult.Failed;
                    }
                }
            }

            return HashResult.Success;
        }
    }
}
