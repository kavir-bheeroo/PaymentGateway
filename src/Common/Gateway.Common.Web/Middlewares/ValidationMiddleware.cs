using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Common.Web.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var responseDictionary = BuildResponseDictionary(ex.Errors);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseDictionary));
        }

        private static IDictionary<string, List<string>> BuildResponseDictionary(IEnumerable<ValidationFailure> errors)
        {
            var dictionary = new Dictionary<string, List<string>>();

            foreach (var error in errors)
            {
                if (dictionary.ContainsKey(error.PropertyName))
                {
                    dictionary[error.PropertyName].Add(error.ErrorMessage);
                }
                else
                {
                    dictionary.Add(error.PropertyName, new List<string> { error.ErrorMessage });
                }
            }

            return dictionary;
        }
    }
}