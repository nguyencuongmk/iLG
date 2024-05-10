using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class ActivationRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("otp")]
        public string Otp { get; set; }
    }
}
