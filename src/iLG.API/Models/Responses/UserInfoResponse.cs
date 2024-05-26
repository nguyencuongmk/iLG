using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class UserInfoResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("relationship")]
        public string Relationship { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

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

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("zodiac")]
        public string? Zodiac { get; set; }

        [JsonProperty("biography")]
        public string? Biography { get; set; }

        [JsonProperty("images")]
        public List<ImageResponse> Images { get; set; } = [];

        [JsonProperty("hobbies")]
        public List<HobbyResponse> Hobbies { get; set; }
    }
}