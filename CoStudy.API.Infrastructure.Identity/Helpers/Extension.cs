using System;
using System.Security.Cryptography;

namespace CoStudy.API.Infrastructure.Identity.Helpers
{
    public static class Extension
    {
        public static string randomTokenString()
        {
            using RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public static string randomOTPString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[3];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

    }
}
