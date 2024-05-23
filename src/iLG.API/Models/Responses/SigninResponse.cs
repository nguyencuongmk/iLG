using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class SigninResponse
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("isUpdatedInfo")]
        public bool IsUpdatedInfo { get; set; }
    }
}