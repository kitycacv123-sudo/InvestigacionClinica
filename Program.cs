using Microsoft.EntityFrameworkCore;
using InvestigacionClinica.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Forzar puerto para Railway
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Agregar servicios base
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------------------------------------------------
// 🗄️ CONFIGURACIÓN DE BASE DE DATOS (con soporte para Railway)
// --------------------------------------------------------------
string? connectionString = null;

// 1. En Railway, la base de datos se conecta mediante DATABASE_URL
if (string.IsNullOrEmpty(connectionString))
    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

// 2. También podría estar en otra variable manual (si la definiste)
if (string.IsNullOrEmpty(connectionString))
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__InvestigacionClinicaContext");

// 3. Si no hay variables de entorno (modo desarrollo local), usa appsettings.json
if (string.IsNullOrEmpty(connectionString))
    connectionString = builder.Configuration.GetConnectionString("InvestigacionClinicaContext");

// Si aún así no hay cadena, lanzamos error claro
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("No se encontró cadena de conexión para la base de datos.");

// Registrar DbContext con PostgreSQL
builder.Services.AddDbContext<InvestigacionClinicaContext>(options =>
    options.UseNpgsql(connectionString));

// --------------------------------------------------------------
var app = builder.Build();

// 🧩 APLICAR MIGRACIONES AUTOMÁTICAMENTE (crea la BD en Railway)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InvestigacionClinicaContext>();
    dbContext.Database.Migrate();  // Crea o actualiza la base de datos
}

// Configurar Swagger (activo siempre para que puedas probar)
app.UseSwagger();
app.UseSwaggerUI();

// Nota: No uses app.UseHttpsRedirection() porque Railway ya maneja HTTPS.
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();