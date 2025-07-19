using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projeto_Nutri.Application.Service;
using Projeto_Nutri.Infrastructure.Context;
using Projeto_Nutri.Infrastructure.IRepository;
using Projeto_Nutri.Infrastructure.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexão com o banco
builder.Services.AddDbContext<NutriContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT - configuração
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // sem tolerância de expiração
    };
});

// Autorização com roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("NutritionistOnly", policy => policy.RequireRole("NUTRITIONIST"));
});

// Repositórios
builder.Services.AddScoped<IFoodsRepository, FoodsRepository>();
builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
builder.Services.AddScoped<IMealPlansRepository, MealPlansRepository>();

// Serviços
builder.Services.AddScoped<FoodsService>();
builder.Services.AddScoped<PatientsService>();
builder.Services.AddScoped<MealPlansService>();

var app = builder.Build();

// Configurações do pipeline
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
