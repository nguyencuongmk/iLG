using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class ApiResponse
    {
        public ApiResponse()
        {
        }

        public ApiResponse(dynamic data)
        {
            Data = data;
        }

        [JsonProperty("timeStamp")]
        public string TimeStamp { get; set; } = DateTime.UtcNow.TimeOfDay.ToString();

        [JsonProperty("result")]
        public Result Result { get; set; } = new();

        [JsonProperty("data")]
        public dynamic Data { get; set; } = default!;

        [JsonProperty("errors")]
        public List<Error> Errors { get; set; } = [];
    }

    public class Result
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = default!;

        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty("totalErrors")]
        public int TotalErrors { get; set; }
    }

    public class Error
    {
        [JsonProperty("errorDetail")]
        public dynamic ErrorDetail { get; set; } = default!;

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; } = default!;
    }
}