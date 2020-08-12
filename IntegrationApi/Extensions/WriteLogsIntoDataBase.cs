using IntegrationApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationApi.Extensions
{
    public static class WriteLogsIntoDataBase
    {
         public static IApplicationBuilder WriteLogs(this IApplicationBuilder builder)
         {
                return builder.UseMiddleware<WriteLogs>();
         }
        
    }
}
