using DatabaseConnection;
using IntegrationApi.Interfaces;
using IntegrationApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationApi.Middlewares
{
    public class WriteLogs
    {
        private IDataBaseService _dataBaseService { get; }
        private readonly RequestDelegate _next;
        public WriteLogs(RequestDelegate next, IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Headers.ContainsKey("X-Request-ID"))
            {
                _dataBaseService.WriteLogs(context.Request.Headers["X-Request-ID"]);
            }
            await _next.Invoke(context);
        }
    }
}
