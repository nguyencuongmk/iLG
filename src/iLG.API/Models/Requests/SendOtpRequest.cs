using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class SendOtpRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
