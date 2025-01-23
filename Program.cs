using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using HOPITAL2.Data;
using HOPITAL2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HopitaldbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HopitaldbContextConnection")));

builder.Services.AddDbContext<HOPITAL2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HopitaldbContextConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<HOPITAL2Context>();
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization();

// Conditional redirection middleware
app.Use(async (context, next) =>
{
    if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
    {
        // Redirect logged-in users to Home/Index
        if (context.Request.Path == "/" || context.Request.Path == "/Home/Index2")
        {
            context.Response.Redirect("/Home/Index");
            return;
        }
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index2}/{id?}"); // Default route for non-authenticated users

app.Run();
