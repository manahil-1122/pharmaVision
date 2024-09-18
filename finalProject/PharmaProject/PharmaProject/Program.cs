using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PharmaProject.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpContextAccessor();


//Configure Enitity framework and sql server with retry on failure 
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure()));

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add authentication services
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Main/Login";
        options.AccessDeniedPath = "/Main/AccessDenied";
    });

//google
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "19313724760-4pu4bpnfoq1boqe18g74fe1p6en097oa.apps.googleusercontent.com";
    options.ClientSecret = "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";
})
.AddFacebook(options =>
{
    options.ClientId = "870691951777109";
    options.ClientSecret = "eb4e1860721b76cb7b055d8a704e5c98";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//session and authentication middleware
app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Login}/{id?}");

//seed the admin
SeedAdminUser(app);

app.Run();

//method of seed admin
void SeedAdminUser(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Check if admin user exists
        if (!context.Clients.Any(u => u.Username == "admin"))
        {
            // Create admin user
            var adminUser = new Users
            {
                Username = "admin",
                Password = "admin123", // You should hash this in a real application
                Role = "Admin",
                CompanyName = "Admin Company" // Provide a value for CompanyName
            };

            context.Clients.Add(adminUser);
            context.SaveChanges();
        }
    }
}
