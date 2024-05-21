using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class HobbyCategoryResponse
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("hobbies")]
        public List<HobbyResponse> Hobbies { get; set; }
    }
}