using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class HobbyResponse
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("hobbyCategoryId")]
        public int HobbyCategoryId { get; set; }
    }
}