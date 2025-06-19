using DeviceManagement.Infrastructure.Data;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Infrastructure.Repositories;
using DeviceManagement.Application.UseCases.Clientes;
using DeviceManagement.Application.UseCases.Dispositivos;
using DeviceManagement.Application.UseCases.Eventos;
using DeviceManagement.Application.UseCases.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = false;
    options.SingleLine = true;
    options.TimestampFormat = "[HH:mm:ss] ";
});

// Add services to the container.
builder.Services.AddControllers();

// Entity Framework
builder.Services.AddDbContext<DeviceManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Use Cases
builder.Services.AddScoped<CreateClienteUseCase>();
builder.Services.AddScoped<GetClienteByIdUseCase>();
builder.Services.AddScoped<GetAllClientesUseCase>();
builder.Services.AddScoped<UpdateClienteUseCase>();
builder.Services.AddScoped<DeleteClienteUseCase>();
builder.Services.AddScoped<PatchClienteUseCase>();

builder.Services.AddScoped<CreateDispositivoUseCase>();
builder.Services.AddScoped<UpdateDispositivoUseCase>();
builder.Services.AddScoped<GetDispositivoByIdUseCase>();
builder.Services.AddScoped<GetDispositivosByClienteUseCase>();
builder.Services.AddScoped<GetAllDispositivosUseCase>();
builder.Services.AddScoped<DeleteDispositivoUseCase>();
builder.Services.AddScoped<PatchDispositivoUseCase>();

builder.Services.AddScoped<CreateEventoUseCase>();
builder.Services.AddScoped<GetEventosByPeriodUseCase>();
builder.Services.AddScoped<GetEventosByDispositivoUseCase>();
builder.Services.AddScoped<GetAllEventosUseCase>();
builder.Services.AddScoped<GetEventoByIdUseCase>();
builder.Services.AddScoped<DeleteEventoUseCase>();

builder.Services.AddScoped<GetDashboardDataUseCase>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Device Management API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
