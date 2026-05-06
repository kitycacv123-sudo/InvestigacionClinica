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
// OBTENER CADENA DE CONEXIÓN (con diagnóstico)
// --------------------------------------------------------------
string? connectionString = null;

// Variables de entorno que Railway usa
var envConnection = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(envConnection))
{
    connectionString = envConnection;
    Console.WriteLine("✅ DATABASE_URL encontrada en entorno.");
}
else
{
    // Fallback: puede estar con otro nombre
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__InvestigacionClinicaContext");
    if (!string.IsNullOrEmpty(connectionString))
        Console.WriteLine("✅ ConnectionStrings__... encontrada.");
}

// Si no hay ninguna variable, intentamos con appsettings.json (solo local)
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("InvestigacionClinicaContext");
    if (!string.IsNullOrEmpty(connectionString))
        Console.WriteLine("✅ Usando cadena desde appsettings.json (entorno local).");
}

// Si después de todo sigue vacía, lanzamos error claro con instrucciones
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("❌ CRÍTICO: No se encontró cadena de conexión.");
    Console.WriteLine("   Asegúrate de que en Railway la variable DATABASE_URL esté definida en este servicio.");
    throw new InvalidOperationException("No se encontró cadena de conexión para la base de datos. Verifica la variable DATABASE_URL en Railway.");
}

// Registrar DbContext con PostgreSQL
builder.Services.AddDbContext<InvestigacionClinicaContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InvestigacionClinicaContext>();
    dbContext.Database.Migrate();
    Console.WriteLine("✅ Migraciones aplicadas correctamente.");
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();