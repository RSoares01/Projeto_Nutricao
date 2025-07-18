using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;
using Projeto_Nutri.Infrastructure.Repository;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<NutriContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

// Repos + Services
builder.Services.AddScoped<IFoodsRepository, FoodsRepository>();
builder.Services.AddScoped<FoodsService>();



// Injeção de dependência dos serviços


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // <- Aqui!
app.UseAuthorization();

app.MapControllers();
app.Run();
