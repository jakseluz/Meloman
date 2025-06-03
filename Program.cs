using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Meloman.Data;
var builder = WebApplication.CreateBuilder(args);

// Add DbContext from SQLite
builder.Services.AddDbContext<MelomanContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MelomanContext") ?? throw new InvalidOperationException("Connection string 'MelomanContext' not found.")));

// Add services to the container. Add controllers with views.
builder.Services.AddControllersWithViews();

// Add session handling
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // add session before authorization

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
