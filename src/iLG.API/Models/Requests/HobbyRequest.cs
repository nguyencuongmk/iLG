using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class HobbyRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
