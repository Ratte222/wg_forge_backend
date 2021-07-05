using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using BLL.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace wg_forge_backend
{
    public class ExeptionMeddleware
    {
        private readonly RequestDelegate _next;

        public ExeptionMeddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException ex)
            {
                //return this.Problem(ex.Message);
                //return this.BadRequest(ex.Message);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (SelectException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                //logger.LogWarning($"{ex.Message}\r\n {ex?.StackTrace}");
                //return StatusCode(500);
                context.Response.StatusCode = 500;
                //await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
