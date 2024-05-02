using iLG.API.Models.Responses;

namespace iLG.API.Extensions
{
    public static class ApiResponseExtension
    {
        public static ApiResponse GetResult(this ApiResponse response, int statusCode, string message)
        {
            if (statusCode != StatusCodes.Status200OK)
            {
                response.Data = new List<dynamic>();
                response.Result.TotalErrors = response.Errors.Count;
            }
            else
            {
                if (response.Data is ICollection<dynamic>)
                    response.Result.TotalRecords = response.Data is ICollection<dynamic> data ? data.Count : 0;
                else
                {
                    var data = response.Data;
                    response.Data = new List<dynamic>() { data };
                    response.Result.TotalRecords = response.Data.Count;
                }

                response.Errors = [];
            }

            response.Result.StatusCode = statusCode;
            response.Result.Message = message;

            return response;
        }
    }
}
