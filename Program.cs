using Microsoft.EntityFrameworkCore;
using InvestigacionClinica.Data;

var builder = WebApplication.CreateBuilder(args);

// Puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// --- 1. CONFIGURACIÓN DE CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRailwayFront", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader()
              .AllowAnyMethod();

       
    });
});

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- LÓGICA DE CONEXIÓN ROBUSTA (Tu código original) ---
string? connectionString = null;
var customConn = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? builder.Configuration["CONNECTION_STRING"];
var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrEmpty(customConn))
{
    connectionString = customConn;
    Console.WriteLine("✅ CONNECTION_STRING detectada.");
}
else if (!string.IsNullOrEmpty(dbUrl))
{
    connectionString = dbUrl;
    Console.WriteLine("✅ DATABASE_URL detectada.");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("InvestigacionClinicaContext");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Falta configuración de DB en Railway.");
}

builder.Services.AddDbContext<InvestigacionClinicaContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// --- 2. ACTIVAR CORS (Debe ir antes de MapControllers) ---
app.UseCors("AllowRailwayFront");

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<InvestigacionClinicaContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("✅ Base de datos conectada y migraciones aplicadas.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al conectar o migrar: {ex.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();