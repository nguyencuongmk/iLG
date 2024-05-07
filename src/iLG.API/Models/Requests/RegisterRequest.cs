using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class RegisterRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}
