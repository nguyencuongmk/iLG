using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class LoginResponse
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}
