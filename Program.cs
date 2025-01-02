using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using dotNET.Interfaces; 

var builder = WebApplication.CreateBuilder(args);

// Dodanie EF Core z PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodanie usług dla kontrolerów, Swaggera i punktów końcowych
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rejestracja IUserService
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Używanie Swaggera w środowisku deweloperskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Konfiguracja plików statycznych i domyślnego pliku
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" } // Ustaw index.html jako domyślny
});

// Przekierowanie HTTP do HTTPS
app.UseHttpsRedirection();

// Włączenie obsługi plików statycznych (np. CSS, JS, obrazy)
app.UseStaticFiles();

// Ręczne mapowanie ścieżki do index.html
app.MapGet("/", async (context) =>
{
    context.Response.Redirect("/index.html");
});

app.UseAuthorization();

// Mapowanie kontrolerów (API)
app.MapControllers();

// Uruchomienie aplikacji
app.Run();