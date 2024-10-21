using Microsoft.AspNetCore.Mvc.Filters;
using Restaurante.Enumerables;

namespace Restaurante.Filters
{
    public class AccessFilter : ActionFilterAttribute
    {
        private readonly ERol[] roles;

        public AccessFilter(params ERol[] rol)
        {
            this.roles = rol;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string headerRol = context.HttpContext.Request.Headers["Rol"];
            ERol rol = Enum.Parse<ERol>(headerRol);
            if (this.roles.Contains(rol))
            {
                await next();
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.HttpContext.Response.WriteAsJsonAsync(new { mensaje = "Usted no esta autorizado", code = StatusCodes.Status401Unauthorized });
            }
        }
    }
}
