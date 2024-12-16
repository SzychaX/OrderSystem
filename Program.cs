using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodanie EF Core z PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" } // Ustaw index.html jako domyślny
});

app.UseHttpsRedirection();

// Włączenie obsługi plików statycznych
app.UseStaticFiles();

// Ręczne mapowanie do index.html
app.MapGet("/", async (context) =>
{
    context.Response.Redirect("/index.html");
});

app.UseAuthorization();
app.MapControllers();

app.Run();