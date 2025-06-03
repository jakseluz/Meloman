using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Meloman.Data;
using Meloman.Filters;

var cultureInfo = new System.Globalization.CultureInfo("en-US");
Thread.CurrentThread.CurrentCulture = cultureInfo;
Thread.CurrentThread.CurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext from SQLite
builder.Services.AddDbContext<MelomanContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MelomanContext") ?? throw new InvalidOperationException("Connection string 'MelomanContext' not found.")));

// Add POST and GET methods attributes
builder.Services.AddScoped<AdminAllowedAttribute>();
builder.Services.AddScoped<VerifiedUserAllowed>();

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

var supportedCultures = new[] { cultureInfo };

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pl-PL"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // add session before authorization

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Data seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MelomanContext>();
    context.Database.Migrate(); // context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}


app.Run();
