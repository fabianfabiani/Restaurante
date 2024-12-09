using Microsoft.AspNetCore.Mvc.Filters;
using Restaurante.Entities;
using Restaurante.Enumerables;
using System.Linq;

namespace Restaurante.Filters
{
    public class AccessFilter : ActionFilterAttribute
    {
        private readonly ERol[] roles;
        private readonly ILogger<AccessFilter> logger;
        public AccessFilter(params ERol[] roles)
        {
            this.roles = roles;
            ILoggerFactory factory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            this.logger = factory.CreateLogger<AccessFilter>();
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Usuario? usuario = context.HttpContext.Items["usuario"] as Usuario;

            if (usuario == null || !this.roles.Select(r => r.ToString()).Contains(usuario.Rol.Descripcion))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    codigo = StatusCodes.Status401Unauthorized,
                    mensaje = "Usted no está autorizado para acceder a este endpoint"
                });
            }
            else
            {
                await next();
            }
        }


    }
}
