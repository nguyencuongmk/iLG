namespace iLG.API.Constants
{
    public static class Message
    {
        public static class Success
        {
            public static class User
            {
                public const string SIGNED_IN = "User has been signed in";
                public const string SIGNED_OUT = "User has been signed out";
                public const string SIGNED_UP = "User has been signed up";
                public const string PW_CHANGED = "Password has been changed";
                public const string NEW_PASSWORD = "New password has been sent via email";
                public const string OTP = "OTP code has been sent via email";
            }
        }

        public static class Error
        {
            public static class Common
            {
                public const string SERVER_ERROR = "Server error";
                public const string EMAIL_ERROR = "Send email error";
            }

            public static class Paging
            {
                public const string INVALID_PAGING = "Invalid pageIndex or pageSize";
            }

            public static class User
            {
                public const string LOGIN_FAILED = "Invalid email or password";
                public const string INVALID_EMAIL = "Invalid email";
                public const string INVALID_PASSWORD = "Invalid password";
                public const string INVALID_CURRENT_PASSWORD = "Invalid current password";
                public const string EXISTS_EMAIL = "Email already exists";
                public const string NOT_EXISTS_USER = "User is not exists";
                public const string USER_LOCKED = "User is locked";
                public const string EMAIL_NOT_YET_CONFIRMED = "User has not been confirmed by email";
                public const string INVALID_RF_TOKEN = "Refresh token is invalid or expired";
                public const string INVALID_AC_TOKEN = "Access token is invalid or expired";
                public const string EMPTY_TOKEN = "Token is null or empty";
                public const string NOT_ENOUGH_INFO = "Not enough information. Please fill all the required fields";
                public const string NOT_AUTH = "User has not been authenticated";
                public const string INVALID_OTP = "Invalid OTP";
                public const string EXPIRED_OTP = "OTP has expired. Please send a new request";
                public const string CACHE_OTP = "You have just sent a request to receive an OTP code. Please wait 2 minutes to send the next request";
            }

            public static class UserInfo
            {
                public const string NOT_EXISTS = "User Information is not exists";
                public const string NOT_YET_UPDATED = "User information has not been updated";
            }
        }
    }
}