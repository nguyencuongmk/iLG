using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class UserSuitableResponse : UserInfoResponse
    {
        [JsonProperty("sameHobbies")]
        public List<string> SameHobbies { get; set; } = [];
    }
}