using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Common.Exceptions;
using TaskAPI.Common.Logging;
using TaskAPI.Common.Models.Outbound;

namespace TaskAPI.Common.Middlewares
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestMiddleware> _logger;

        public RequestMiddleware(
            RequestDelegate next, 
            ILogger<RequestMiddleware> logger
        ) {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            catch (InvalidValueException ex)
            {
                _logger.LogInformation(new RequestLogEntry(
                    $"InvalidValueException: code {StatusCodes.Status422UnprocessableEntity} message {string.Join(",", ex.Errors)}",
                    StatusCodes.Status422UnprocessableEntity, context).ToString());

                await Respond(context, StatusCodes.Status422UnprocessableEntity, ex.Errors);
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation(new RequestLogEntry(
                    $"NotFoundException: code {StatusCodes.Status404NotFound} message {string.Join(",", ex.Errors)}",
                    StatusCodes.Status404NotFound, context).ToString());

                await Respond(context, StatusCodes.Status404NotFound, ex.Errors);
            }
            catch (ConflictException ex)
            {
                _logger.LogInformation(new RequestLogEntry(
                    $"ConflictException: code {StatusCodes.Status409Conflict} message {ex.Message}",
                    StatusCodes.Status409Conflict, context).ToString());

                await Respond(context, StatusCodes.Status409Conflict, new[] { ex.Message });
            }
            catch (NotModifiedException ex)
            {
                _logger.LogInformation(new RequestLogEntry(
                    $"NotModifiedException: code {StatusCodes.Status304NotModified} message {ex.Message}",
                    StatusCodes.Status304NotModified, context).ToString());

                await Respond(context, StatusCodes.Status304NotModified, null);
            }
            catch (HttpStatusCodeException ex)
            {
                _logger.LogInformation(new RequestLogEntry(
                    $"HttpStatusCodeException: code {(int)ex.StatusCode} message {ex.Message}", (int)ex.StatusCode,
                    context).ToString());

                await Respond(context, (int)ex.StatusCode, new[] { ex.Message });
            }
            catch (AccessException ex)
            {
                _logger.LogWarning(ex,
                    new RequestLogEntry(
                        $"AccessException: code {StatusCodes.Status400BadRequest} message {ex.Message}",
                        StatusCodes.Status400BadRequest, context).ToString());

                await Respond(context, StatusCodes.Status400BadRequest, new[] { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, new RequestLogEntry($"Exception: code {StatusCodes.Status500InternalServerError} message {ex.Message}", StatusCodes.Status500InternalServerError, context).ToString());
                await Respond(context, StatusCodes.Status500InternalServerError,
                    new[] { "Service unavailable, event was logged and will be addressed." });
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation(new PerformanceLogEntry(stopWatch.ElapsedMilliseconds, context).ToString());
            }
        }

        private async Task Respond(HttpContext context, int status, string[] messages)
        {
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";
            if (messages != null)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
                {
                    Errors = messages
                }), Encoding.UTF8);
            }
        }
    }
}
