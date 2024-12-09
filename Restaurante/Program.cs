using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Entities;
using Restaurante.Filters;
using Restaurante.Interface;
using Restaurante.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LaboratorioApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//--------------Services------------------------ Se agrego para la coneccion con DB
builder.Services.AddDbContext<DataBaseContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringEF")));  


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Rol", "Admin"));
    options.AddPolicy("AdminOrSocio", policy => policy.RequireClaim("Rol", "Admin", "Socio"));
    options.AddPolicy("Socio", policy => policy.RequireClaim("Rol", "Socio"));
    options.AddPolicy("Empleado", policy => policy.RequireClaim("Rol", "Empleado"));
    options.AddPolicy("Mozo", policy => policy.RequireClaim("Sector", "Mozo"));

});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Servicios que agrego de la carpeta Service
builder.Services.AddScoped<IEncuestaService, EncuestaService>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IComandaService, ComandaService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IMesaService, MesaService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AutenticationFilter>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    JwtSettings? jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

                    TokenValidationParameters tokenValidationParameters = new TokenValidationParameters();

                    tokenValidationParameters.ValidateIssuerSigningKey = true;
                    tokenValidationParameters.ValidIssuer = jwtSettings.Issuer;
                    tokenValidationParameters.ValidAudience = jwtSettings.Audience;
                    tokenValidationParameters.IssuerSigningKey = key;
                    tokenValidationParameters.ClockSkew = TimeSpan.Zero;

                    options.TokenValidationParameters = tokenValidationParameters;
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();







