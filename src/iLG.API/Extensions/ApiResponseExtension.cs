using iLG.API.Models.Responses;
using Microsoft.AspNetCore.WebUtilities;

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
                if (response.Data.GetType().GetGenericTypeDefinition().IsGenericTypeDefinition)
                    response.Result.TotalRecords = response.Data.Count;
                else
                {
                    var data = response.Data;
                    response.Data = data is null ? [] : new List<dynamic>() { data };
                    response.Result.TotalRecords = response.Data.Count;
                }

                response.Errors = [];
            }

            response.Result.StatusCode = statusCode;
            response.Result.Message = string.IsNullOrEmpty(message) ? ReasonPhrases.GetReasonPhrase(statusCode) : message;

            return response;
        }
    }
}