namespace CoStudy.API.WebAPI.Middlewares
{
    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }

        public ApiOkResponse(object result) : base(true, 200)
        {
            Result = result;
        }

        public ApiOkResponse(object result,string message) :base(true, 200,message)
        {
            Result = result;
        }
    }
}
