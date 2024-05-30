using ApiRestEDC.Datos;
using ApiRestEDC.Repositories;
using ApiRestEDC.Repositories.Impl;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var connectionString = builder.Configuration.GetConnectionString("localConection");
builder.Services.AddDbContext<MyAppContext>(options =>options.UseSqlServer(connectionString));
// Configure Dapper IDbConnection
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("localConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEstadoCuentaRepository, EstadoCuentaRepositoryImpl>();
builder.Services.AddScoped<IDetalleEstadoCuentaRepository, DetalleEstadoCuentaRepositoryImpl>();

var app = builder.Build();

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
