using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.WebAPI.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; }

        [JsonPropertyName("code")]
        [JsonProperty(PropertyName = "code")]
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
        public string Message { get; }

        public ApiResponse(bool success, int statusCode, string message = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode); ;
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    return "Success";
                case 404:
                    return "Not Found";
                case 500:
                    return "Internal Server Error";
                default:
                    return null;
            }
        }
    }
}
