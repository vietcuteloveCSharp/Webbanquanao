using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.CheckEnpoint
{
    public class AuthorizeCheckOperationFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Kiểm tra nếu phương thức hoặc controller có attribute [Authorize]
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
            // Kiểm tra nếu phương thức có attribute [AllowAnonymous]
            var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
            // Chỉ áp dụng bảo mật nếu có [Authorize] và không có [AllowAnonymous]
            if (hasAuthorize && !hasAllowAnonymous)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme,        Id = "Bearer" }
                            },
                            new string[] {}
                        }
                    }
                };

            }
        }
    }
}
