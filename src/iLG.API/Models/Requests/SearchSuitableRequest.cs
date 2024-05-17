using iLG.Domain.Enums;
using Newtonsoft.Json;

namespace iLG.API.Models.Requests
{
    public class SearchSuitableRequest
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("minAge")]
        public int MinAge { get; set; }

        [JsonProperty("maxAge")]
        public int MaxAge { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("interests")]
        public List<string> Interests { get; set; } = [];

        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }
}
