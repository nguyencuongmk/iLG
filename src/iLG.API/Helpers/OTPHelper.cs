using System.Text.RegularExpressions;

namespace iLG.API.Helpers
{
    public static class OTPHelper
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

        public static bool VerifyOTP(string userInputOTP, string generatedOTP)
        {
            if (string.IsNullOrEmpty(userInputOTP) || string.IsNullOrEmpty(generatedOTP))
                return false;

            if (userInputOTP.Length != 6)
                return false;

            if (!Regex.IsMatch(userInputOTP, "^[0-9]+$"))
                return false;

            return userInputOTP == generatedOTP;
        }
    }
}