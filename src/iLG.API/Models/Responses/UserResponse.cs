using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class UserInfoResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

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
        public List<ImageResponse> Images { get; set; } = [];

        [JsonProperty("hobbies")]
        public List<HobbyDetailResponse> HobbyDetails { get; set; }
    }
}