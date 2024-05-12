using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class ResendOtpRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
