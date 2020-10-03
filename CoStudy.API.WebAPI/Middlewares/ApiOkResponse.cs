using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.WebAPI.Middlewares
{
    public class ApiOkResponse : ApiResponse
    {
        public object Result { get;  }
        public ApiOkResponse(object result)
           : base(true, 200)
        {
            Result = result;
        }
    }
}
