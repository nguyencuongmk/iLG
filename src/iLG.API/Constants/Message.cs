namespace iLG.API.Constants
{
    public static class Message
    {
        public static class Success
        {
            public static class User
            {
                public const string LOGGED_IN = "Account has been logged in";
                public const string LOGGED_OUT = "Account has been logged out";
                public const string ACC_ACTIVATED = "Account has been activated";
                public const string PW_CHANGED = "Password has been changed";
                public const string NEW_PASSWORD = "New password has been sent via email";
                public const string NEW_OTP = "New OTP code has been sent via email";
            }
        }

        public static class Error
        {
            public static class Common
            {
                public const string SERVER_ERROR = "Server error";
                public const string EMAIL_ERROR = "Send email error";
            }

            public static class User
            {
                public const string LOGIN_FAILED = "Email or Password is invalid";
                public const string INVALID_EMAIL = "Email is invalid";
                public const string EXISTS_EMAIL = "Email already exists";
                public const string NOT_EXISTS_USER = "User is not exists";
                public const string ACCOUNT_LOCKED = "Account is locked";
                public const string EMAIL_NOT_YET_CONFIRMED = "Account has not been confirmed by email";
                public const string INVALID_TOKEN = "Invalid token";
                public const string INVALID_OTP = "Invalid OTP";
                public const string NOT_ENOUGH_INFO = "Not enough information. Please fill all the required fields";
                public const string NOT_AUTH = "Account has not been authenticated";
            }
        }
    }
}