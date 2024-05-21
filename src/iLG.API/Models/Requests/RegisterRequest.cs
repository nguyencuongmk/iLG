using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class RegisterRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("otp")]
        public string Otp { get; set; }
    }
}
