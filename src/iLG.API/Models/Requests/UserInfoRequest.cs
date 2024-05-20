using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class UserInfoRequest
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("nickName")]
        public string? Nickname { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("relationshipStatus")]
        public string? RelationshipStatus { get; set; }

        [JsonProperty("zodiac")]
        public string? Zodiac { get; set; }

        [JsonProperty("biography")]
        public string? Biography { get; set; }

        [JsonProperty("images")]
        public List<ImageRequest> Images { get; set; } = [];

        [JsonProperty("hobbies")]
        public List<HobbyRequest> Hobbies { get; set; }
    }
}