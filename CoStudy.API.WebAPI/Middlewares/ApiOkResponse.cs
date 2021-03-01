namespace CoStudy.API.WebAPI.Middlewares
{
    /// <summary>
    /// Api ok response
    /// </summary>
    /// <seealso cref="CoStudy.API.WebAPI.Middlewares.ApiResponse" />
    public class ApiOkResponse : ApiResponse
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public object Result { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiOkResponse"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public ApiOkResponse(object result)
           : base(true, 200)
        {
            Result = result;
        }
    }
}
