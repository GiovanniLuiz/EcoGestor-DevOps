using System.Text;
using EcoGestorApi.Data;
using EcoGestorApi.Middlewares;
using EcoGestorApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

if (!builder.Environment.IsEnvironment("Testing"))
    builder.Services.AddDbContext<EcoGestorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PontoColetaService>();
builder.Services.AddScoped<AlertaColetaService>();
builder.Services.AddScoped<ColetaResiduoService>();
builder.Services.AddScoped<DestinacaoResiduoService>();
builder.Services.AddScoped<RelatorioService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
var jwtKey = builder.Configuration["Jwt:Key"] ?? "";
if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new InvalidOperationException("Configure Jwt:Key com pelo menos 32 bytes.");
var key = Encoding.UTF8.GetBytes(jwtKey);


    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EcoGestor API",
        Version = "v1",
        Description = "Webservice ESG para gestão de resíduos e reciclagem desenvolvido em ASP.NET Core 8."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTP local nos containers; HTTPS deve ser terminado no proxy em hospedagem pública.
if (app.Configuration.GetValue<bool>("HttpsRedirection:Enabled")) app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
{
    using var scope = app.Services.CreateScope();
    await DatabaseInitializer.InitializeAsync(scope.ServiceProvider.GetRequiredService<EcoGestorDbContext>(), app.Configuration);
}
app.MapGet("/health/live", () => Results.Ok(new { status = "alive" }));
app.MapGet("/health/ready", async (EcoGestorDbContext db) =>
{
    try
    {
        _ = await db.Unidades.AsNoTracking().Select(u => u.IdUnidade).FirstOrDefaultAsync();
        return Results.Ok(new { status = "ready", environment = app.Environment.EnvironmentName, version = app.Configuration["APP_VERSION"] ?? "local" });
    }
    catch { return Results.StatusCode(503); }
});
app.MapControllers();
app.Run();

public partial class Program { }
