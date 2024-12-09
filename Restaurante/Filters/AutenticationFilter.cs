using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Entities;
using Restaurante.Enumerables;
using Restaurante.Service;

namespace Restaurante.Filters
{
    public class AutenticationFilter : ActionFilterAttribute
    {
        private readonly AuthService authService;
        private readonly DataBaseContext _context;
        public AutenticationFilter(DataBaseContext context, AuthService authService)
        {
            this.authService = authService;
            this._context = context;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string bearerToken = context.HttpContext.Request.Headers.Authorization;

            if (!string.IsNullOrWhiteSpace(bearerToken) && bearerToken.Contains("Bearer"))
            {
                string accessToken = bearerToken.Split(" ")[1];
                IDictionary<string, object>? claims = await this.authService.ValidarToken(accessToken);

                // Buscamos el Rol por descripción o ID desde la base de datos
                string rolDescripcion = claims["Rol"].ToString()!;
                Rol rolEncontrado = _context.Roles.FirstOrDefault(r => r.Descripcion == rolDescripcion);
                if (rolEncontrado == null)
                {
                    throw new Exception("Rol no encontrado");
                }

                Usuario usuario = new Usuario()
                {
                    Rol = rolEncontrado,  // Asignamos la entidad Rol obtenida de la base de datos
                    Sector = new Sector
                    {
                        descripcion = claims["Sector"].ToString() // Asigna la descripción del sector
                    },
                    Nombre = claims["UserName"].ToString()!
                };

                context.HttpContext.Items.Add("usuario", usuario);
                await next();
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.HttpContext.Response.WriteAsJsonAsync(new { codigo = StatusCodes.Status401Unauthorized, mensaje = "Token invalido" });
            }
        }

    }
}
