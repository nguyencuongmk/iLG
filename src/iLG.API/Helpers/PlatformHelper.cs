namespace iLG.API.Helpers
{
    public static class PlatformHelper
    {
        public static string GetPlatformName(PlatformID platformId)
        {
            return platformId switch
            {
                PlatformID.Win32NT => "Windows",
                PlatformID.Unix => "Unix",
                PlatformID.MacOSX => "Mac OS",
                _ => "Unknown platform"
            };
        }
    }
}
