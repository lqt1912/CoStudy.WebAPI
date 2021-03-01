using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CoStudy.API.WebAPI.Middlewares
{
    /// <summary>
    /// Class Api response
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ApiResponse"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        [JsonPropertyName("code")]
        [JsonProperty(PropertyName = "code")]
        public int StatusCode { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        public ApiResponse(bool success, int statusCode, string message = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode); ;
        }

        /// <summary>
        /// Gets the default message for status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns></returns>
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
