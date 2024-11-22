using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Rejestracja kontekstu bazy danych
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodanie kontrolerów i widoków (opcjonalne, jeśli używasz API)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Włącz HSTS dla środowisk produkcyjnych
}

// Middleware dla obsługi HTTPS
app.UseHttpsRedirection();

// Middleware dla obsługi plików statycznych
app.UseStaticFiles(); // Obsługuje pliki z katalogu wwwroot

// Middleware dla routingu
app.UseRouting();

app.UseAuthorization(); // Obsługa autoryzacji (jeśli potrzebna)

// Domyślna trasa dla kontrolerów i API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Domyślna strona (np. index.html dla frontendu)
app.MapFallbackToFile("index.html");

app.Run();