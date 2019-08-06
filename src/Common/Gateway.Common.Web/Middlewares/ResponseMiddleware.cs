using Gateway.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Gateway.Common.Web.Middlewares
{
    public class ResponseMiddleware
    {
        private const string GenericCustomErrorMessage = "An error has occurred";

        private readonly RequestDelegate _next;

        private static readonly IReadOnlyDictionary<Type, HttpStatusCode> StatusCodeMappings =
            new Dictionary<Type, HttpStatusCode>
            {
                { typeof(ObjectNotFoundException), HttpStatusCode.NotFound },
                { typeof(BadRequestException), HttpStatusCode.BadRequest },
                { typeof(GatewayException), HttpStatusCode.InternalServerError },
                { typeof(UnauthorizedException), HttpStatusCode.Unauthorized },
                { typeof(ObjectAlreadyExistsException), HttpStatusCode.Conflict }
            };

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var isGenericException = !StatusCodeMappings.TryGetValue(ex.GetType(), out var httpStatusCode);
            if (isGenericException) httpStatusCode = HttpStatusCode.InternalServerError;

            if (context?.Response?.HasStarted == true) throw ex;

            await PopulateResponse(context, httpStatusCode, ex);
        }

        private async Task PopulateResponse(HttpContext context, HttpStatusCode httpStatusCode, Exception ex)
        {
            var response = new Models.WebResponse
            {
                Error = (ex as ExceptionBase)?.CustomMessage ?? GenericCustomErrorMessage,
            };

            // Need to check for environment before adding additional information.
            response.ErrorAdditionalInfo = ex.ToString();

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}