using Microsoft.EntityFrameworkCore;
using VideoForum.DataAccess.Data;
using VideoForum.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.AddApplicationServices();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Effect database migration automatically
InitializeContext();

app.Run();


// This method calls the static Initialize method
void InitializeContext()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        //Injects a scoped instance of the dbContext
        AppDbContext? dbContext = scope.ServiceProvider.GetService<AppDbContext>();


        ContextInitializer.Initialize(dbContext);
    }
    catch(Exception ex)
    {
        var logger = services.GetService<ILogger<Program>>();
        logger.LogError("An error occured while migrating the database");
    }
}