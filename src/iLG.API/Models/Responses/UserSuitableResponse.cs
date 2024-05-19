using iLG.Domain.Enums;
using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class UserSuitableResponse
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
        public Zodiac? Zodiac { get; set; }

        [JsonProperty("biography")]
        public string? Biography { get; set; }

        [JsonProperty("images")]
        public List<ImageResponse> Images { get; set; } = [];

        [JsonProperty("sameHobbies")]
        public List<string?> SameHobbies { get; set; } = [];
    }

    public class ImageResponse
    {
        public string? Path { get; set; }

        public string? Type { get; set; }
    }
}
