using Microsoft.EntityFrameworkCore;
using DSW_3P_PR03.Data;
using DSW_3P_PR03.Repositories;
using DSW_3P_PR03.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias
builder.Services.AddScoped<IPersonajeRepository, PersonajeSQLRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

//Configuracion DE SQL Server

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
            builder.Configuration.GetConnectionString("SqlServerConnection")
    )
);



var app = builder.Build();

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapear los controladores
app.MapControllers();

app.Run();

//ORM
//dotnet add package Microsft.EntityFrameworkCore.SqlServer
//dotnet add package Microsft.EntityFrameworkCore.Design
//dotnet tool install --global dotnet-ef --version 10.0.*
//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version "10.0*"
//dotnet add package Swashbuckle.AspNetCore
