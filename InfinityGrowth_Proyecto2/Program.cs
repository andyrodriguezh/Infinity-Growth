using AppLogic;
using AppLogic.Services;
using DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//configuration for JWT
var jwtSection = configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("TipoUsuario", "1"));
    options.AddPolicy("Asesor", policy => policy.RequireClaim("TipoUsuario", "2"));
    options.AddPolicy("Cliente", policy => policy.RequireClaim("TipoUsuario", "3"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IRegistrarseManager, RegistrarseManager>();
builder.Services.AddScoped<ILoginManager, LoginManager>();
builder.Services.AddScoped<IRecuperarPasswordManager, RecuperarPasswordManager>();
builder.Services.AddScoped<IEmailService>(provider => new Email_Service(configuration));
builder.Services.AddSingleton<Jwt_Service>();
builder.Services.AddScoped<StockManager>();
builder.Services.AddScoped<TwelveData_Service>();
builder.Services.AddScoped<IInversionesActivasManager, InversionesActivasManager>();
builder.Services.AddScoped<IPriceManager, PriceManager>();
builder.Services.AddScoped<IPortafoliosManager, PortafoliosManager>();
builder.Services.AddScoped<IReporteManager, ReporteManager>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAjusteComisionesManager, AjusteComisionesManager>();


// --- PayPal Integration ---
builder.Services.Configure<PayPalOptions>(configuration.GetSection("PayPal"));

builder.Services.AddScoped<PayPalService>();
// --- Fin PayPal Integration ---


//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();

