using Microsoft.EntityFrameworkCore;
using InvestigacionClinica.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Forzar puerto para Railway (como en Ventas)
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Configurar DbContext (igual que tenías)
builder.Services.AddDbContext<InvestigacionClinicaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InvestigacionClinicaContext")
        ?? throw new InvalidOperationException("Connection string 'InvestigacionClinicaContext' not found.")));

// Agregar servicios
builder.Services.AddHttpClient();
builder.Services.AddControllers();

// Swagger (siempre habilitado para producción)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Investigacion Clinica API",
        Version = "v1"
    });
});

// (Opcional) Configurar CORS similar a Ventas
builder.Services.AddCors(options =>
{
    options.AddPolicy("myApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 🔧 Migraciones automáticas (opcional, como en Ventas)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InvestigacionClinicaContext>();
    db.Database.Migrate();
}

// Swagger UI - siempre activo (no solo desarrollo)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investigacion Clinica API v1");
    c.RoutePrefix = string.Empty; // Esto hace que Swagger esté en la raíz (opcional)
    // Si prefieres Swagger en /swagger, comenta la línea anterior y descomenta la siguiente:
    // c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection(); // (puede causar warning pero no afecta)
app.UseCors("myApp");      // Si agregaste CORS
app.UseAuthorization();
app.MapControllers();

app.Run();