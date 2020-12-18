using EVendas.Aplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVendas.Aplication.Filters
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var errorResponse = ErrorResponse.From(context.Exception);
            context.Result = new ObjectResult(errorResponse) { StatusCode = 500 };
        }
    }
}
