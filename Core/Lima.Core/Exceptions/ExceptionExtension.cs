using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Core.Exceptions
{
    public static class ExceptionExtension
    {
        public static IApplicationBuilder UseLimaExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LimaExceptionMiddleware>();
        }
    }
}
