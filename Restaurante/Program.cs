using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//--------------Services------------------------ Se agrego para la coneccion con DB
builder.Services.AddDbContext<DataBaseContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringEF")));  


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Resto producto 
Producto2 producto = new Producto2(10);
producto.restarProducto(2);

Console.WriteLine($"El stock actual es: {producto.Stock}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();







