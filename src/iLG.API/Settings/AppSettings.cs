namespace iLG.API.Settings
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }

        public MailSettings MailSettings { get; set; }

    }

    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }
    }

    public class MailSettings
    {
        public bool EnableSsl { get; set; }

        public string MailServerUsername { get; set; }

        public string MailServerPassword { get; set; }

        public string MailServer { get; set; }

        public int MailServerPort { get; set; }
    }
}
