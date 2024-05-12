using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class ForgotPasswordRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
