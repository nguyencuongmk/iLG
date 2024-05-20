using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class ImageRequest
    {
        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}
