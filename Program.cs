using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Humanizer;
using INTEXII.Data;
using INTEXII.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// User account db connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(connectionString));
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<IntexW24datasetContext>(options => {
    //options.UseSqlite(builder.Configuration["ConnectionStrings:ShoppingConnection"]);
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ShoppingAzureConnection"]);
});
builder.Services.AddScoped<IIntexW24datasetRepository, EFIntexW24datasetRepository>();

// Adding identities/roles to user accounts
builder.Services.AddDefaultIdentity<IdentityUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
// Require users to be authenticated
builder.Services.AddControllers(config => {
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddControllersWithViews();

// 3PA Services. See here: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-8.0&tabs=visual-studio
// Google: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0
builder.Services.AddAuthentication()
    .AddGoogle(options => {
        IConfigurationSection googleAuthNSection =
        builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];
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

// Redirect HTTP to HTTPS. We will have to wait for deployment to try this out
app.Use(async (context, next) => {
    // If the request is HTTP, redirect to HTTPS
    if (!context.Request.IsHttps) {
        var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        context.Response.Redirect(httpsUrl);
        return;
    }

    // Otherwise, continue processing the request
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
}
else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // https://www.namecheap.com/support/knowledgebase/article.aspx/9711/38/how-to-check-if-hsts-is-enabled/
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(); // Also added for cookie notification

app.UseSession(); // Use the session that we set up above

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// CSP Header
// See here: https://www.stackhawk.com/blog/net-content-security-policy-guide-what-it-is-and-how-to-enable-it/
app.Use(async (context, next) => {
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self';" + // http://localhost:53172 wss://localhost:44346 ws://localhost:53172/;" +
		"connect-src 'self';" + // http://localhost:53172 wss://localhost:44346 ws://localhost:53172/ wss://localhost:44311/ http://localhost:58225 ws://localhost:58225;" +
		"script-src 'self' 'unsafe-inline';" + // TRY NOT TO USE unsafe-inline IF YOU CAN HELP IT!
		"script-src-elem 'self' 'unsafe-inline' https://cdnjs.cloudflare.com;" +
		"style-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com https://fonts.googleapis.com; " + // TRY NOT TO USE unsafe-inline IF YOU CAN HELP IT!
		"font-src 'self' https://cdnjs.cloudflare.com https://fonts.gstatic.com; " +
        "img-src 'self' http://www.w3.org https://m.media-amazon.com/ https://www.lego.com/ https://images.brickset.com/ https://www.brickeconomy.com data:; " +
        //"frame-src 'self';"
        "");

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

// Some default account services/scopes (This seems to only work in development)
if (app.Environment.IsDevelopment()) {
    using (var scope = app.Services.CreateScope()) {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed our roles. Let's just do Admin, everyone else is just a logged in user without a role
        var roles = new[] { "Admin" };
        foreach (var role in roles) {
            // If the role doesn't exist in the system, we can create it
            if (!await roleManager.RoleExistsAsync(role)) {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    /*
    // Add admins here admin accounts
    using (var scope = app.Services.CreateScope()) {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure "Admin" role exists
        if (await roleManager.FindByNameAsync("Admin") == null) {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Our default admin
        string email = PUT EMAIL HERE
        string password = PUT PASSWORD HERE (make sure it aligns with password requirements above)

        if (await userManager.FindByEmailAsync(email) == null) {
            var user = new IdentityUser();
            user.UserName = email;
            user.Email = email;
            user.EmailConfirmed = true;

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded) {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
    */
}


app.Run();
