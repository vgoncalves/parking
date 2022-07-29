global using App.API.Features;
global using App.API.Models;
global using MediatR;
global using FluentValidation;
global using App.API.Data;
global using App.API.Extensions;

using Microsoft.EntityFrameworkCore;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("App.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase(nameof(AppDbContext)));
builder.Services.AddValidatorsFromAssemblyContaining<CreateParking>();
builder.Services.AddMediatR(typeof(CreateParking.Handler).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
