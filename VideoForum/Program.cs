using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoForum.Core.Entities;
using VideoForum.DataAccess.Data;
using VideoForum.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.AddApplicationServices();
builder.AddAuthenticationServices();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Effect database migration automatically
await InitializeContextAsync();

app.Run();


// This method calls the static InitializeAsync method
async Task InitializeContextAsync()
{
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    try
    {
        //Injects a scoped instance of the dbContext
        AppDbContext? dbContext = scope.ServiceProvider.GetService<AppDbContext>();

        //Injects a scoped instance of the generic usermanager
        UserManager<AppUser>? userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();

        //Injects a scoped instance of the generic rolemanager
        RoleManager<AppRole>? roleManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();

        await ContextInitializer.InitializeAsync(dbContext, userManager, roleManager);
    }
    catch(Exception ex)
    {
        var logger = services.GetService<ILogger<Program>>();
        logger?.LogError(ex,"An error occured while migrating the database");
    }
}