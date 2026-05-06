using Microsoft.EntityFrameworkCore;
using InvestigacionClinica.Data;

var builder = WebApplication.CreateBuilder(args);

// Puerto dinámico para Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------------------------------------------------
// LÓGICA DE CONEXIÓN ROBUSTA
// --------------------------------------------------------------
string? connectionString = null;

// 1. Intentar con la variable manual que creamos (Prioridad)
var customConn = Environment.GetEnvironmentVariable("CONNECTION_STRING");
// 2. Intentar con la variable estándar de Railway
var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
// 3. Intentar con el formato de ConnectionStrings de .NET
var dotnetConn = Environment.GetEnvironmentVariable("ConnectionStrings__InvestigacionClinicaContext");

if (!string.IsNullOrEmpty(customConn))
{
    connectionString = customConn;
    Console.WriteLine("✅ Usando: CONNECTION_STRING encontrada.");
}
else if (!string.IsNullOrEmpty(dbUrl))
{
    connectionString = dbUrl;
    Console.WriteLine("✅ Usando: DATABASE_URL encontrada.");
}
else if (!string.IsNullOrEmpty(dotnetConn))
{
    connectionString = dotnetConn;
    Console.WriteLine("✅ Usando: ConnectionStrings__ encontrada.");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("InvestigacionClinicaContext");
    if (!string.IsNullOrEmpty(connectionString))
        Console.WriteLine("✅ Usando: appsettings.json (Local).");
}

// Validación final antes de configurar el DBContext
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("❌ ERROR CRÍTICO: No se detectó ninguna variable de configuración en Railway.");
    Console.WriteLine("Variables actuales detectadas: ");
    Console.WriteLine($"- CONNECTION_STRING: {(!string.IsNullOrEmpty(customConn) ? "Presente" : "Nula")}");
    Console.WriteLine($"- DATABASE_URL: {(!string.IsNullOrEmpty(dbUrl) ? "Presente" : "Nula")}");

    throw new InvalidOperationException("Falta configuración de DB en Railway.");
}

builder.Services.AddDbContext<InvestigacionClinicaContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Aplicar migraciones automáticamente
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