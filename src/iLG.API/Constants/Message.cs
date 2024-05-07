namespace iLG.API.Constants
{
    public static class Message
    {
        public static class Success
        {
        }

        public static class Error
        {
            public const string SERVER_ERROR = "Server Error";

            public static class User
            {
                public const string LOGIN_FAILED = "Email or Password is invalid";
                public const string ACCOUNT_LOCKED = "Account is locked";
                public const string EMAIL_NOT_YET_CONFIRMED = "Account has not been confirmed by email";
            }
        }
    }
}