using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class SigninRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
