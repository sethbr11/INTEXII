using Humanizer;
using INTEXII.Data;
using INTEXII.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// User account db connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(connectionString));
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<IntexW24datasetContext>(options => {
    options.UseSqlite(builder.Configuration["ConnectionStrings:ShoppingConnection"]);
});

builder.Services.AddScoped<IIntexW24datasetRepository, EFIntexW24datasetRepository>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// MFA Services. See here: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-8.0&tabs=visual-studio
// Google: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

// Cookie consent notification. See here: https://learn.microsoft.com/en-us/aspnet/core/security/gdpr?view=aspnetcore-8.0
builder.Services.Configure<CookiePolicyOptions>(options => {
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;

    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookieValue = "true";
});

// Stronger password requirements. See here: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0
builder.Services.Configure<IdentityOptions>(options => {
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 13;
    options.Password.RequiredUniqueChars = 1;
});

// Add support for razor pages
builder.Services.AddRazorPages();

// Session memory, a.k.a. cookies. Using sessions for the cart
builder.Services.AddDistributedMemoryCache(); // stores cached data in memory across the application instances
builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // required to access the current session in the SessionCart class

// HSTS Header
// Check out this site: https://www.stackhawk.com/blog/net-http-strict-transport-security-guide-what-it-is-and-how-to-enable-it/
// Maybe look into this one too? https://www.hanselman.com/blog/how-to-enable-http-strict-transport-security-hsts-in-iis7
builder.Services.AddHsts(options => {
    options.Preload = true; // Preloading means adding your website to the HSTS preload list, which is maintained by web browsers. Being on this list ensures that browsers will always use HTTPS for your website, even for the first visit.
    options.IncludeSubDomains = true; // This instructs browsers to apply the HSTS policy not only to your main domain but also to all of its subdomains.
    options.MaxAge = TimeSpan.FromDays(60); // It specifies how long (in seconds) the HSTS policy should be cached by the browser. In this case, it's set to 60 days.
    // options.ExcludedHosts.Add("example.com"); // If you have subdomains that you don't want to include in the HSTS policy, you can add them to the ExcludedHosts list.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
}
else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(); // Also added for cookie notification

app.UseSession(); // Use the session that we set up above

app.UseRouting();

app.UseAuthorization();

// CSP Header -- NEEDS TO BE CONFIGURED CORRECTLY
// See here: https://www.stackhawk.com/blog/net-content-security-policy-guide-what-it-is-and-how-to-enable-it/
app.Use(async (context, next) => {
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self' http://localhost:53172 wss://localhost:44346;" +
        "script-src 'self'; " +
        "style-src 'self'; " +
        "font-src 'self'; " +
        "img-src 'self' http://www.w3.org https://m.media-amazon.com/ https://www.lego.com/ https://images.brickset.com/; " +
        "frame-src 'self';");

    await next();
});

// Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{action=Index}/{id?}",
    defaults: new { Controller = "Home" });
app.MapDefaultControllerRoute();
app.MapRazorPages();

// Route Razor Pages
app.MapRazorPages();

app.Run();
