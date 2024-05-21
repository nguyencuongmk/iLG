using System.Text.RegularExpressions;

namespace iLG.API.Helpers
{
    public static class OtpHelper
    {
        public static string GenerateOTP()
        {
            int otpLength = 6;
            string validChars = "0123456789";

            Random random = new();
            char[] otpChars = new char[otpLength];

            for (int i = 0; i < otpLength; i++)
            {
                otpChars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(otpChars);
        }
    }
}