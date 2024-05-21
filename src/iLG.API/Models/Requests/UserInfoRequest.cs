using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class UserInfoRequest
    {
        [JsonProperty("relationshipId")]
        public int RelationshipId { get; set; }

        [JsonProperty("companyId")]
        public int CompanyId { get; set; }

        [JsonProperty("jobId")]
        public int JobId { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("nickName")]
        public string? Nickname { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

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