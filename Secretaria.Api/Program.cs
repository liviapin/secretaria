using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Secretaria.Aplicacao.Interfaces;
using Secretaria.Aplicacao.Services;
using Secretaria.Dominio.Interfaces;
using Secretaria.Dominio.Models;
using Secretaria.Infra.Context;
using Secretaria.Infra.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura o DbContext (assegure-se de que o contexto herde de IdentityDbContext ou esteja configurado para o Identity)
builder.Services.AddDbContext<SecretariaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,                      // Número máximo de tentativas
            maxRetryDelay: TimeSpan.FromSeconds(10), // Tempo entre as tentativas
            errorNumbersToAdd: null                // Códigos de erro adicionais (opcional)
        );
    }));

// Configura o Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Opções de senha, lockout, usuário, etc. (ajuste conforme necessário)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<SecretariaDbContext>()
.AddDefaultTokenProviders();

// Configure o JWT Bearer
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException("JWT Key is not configured in appsettings.json");
}
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Adicione os serviços ao container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Registro dos Repositórios
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();

// Registro dos Serviços
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<ITurmaService, TurmaService>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();

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
