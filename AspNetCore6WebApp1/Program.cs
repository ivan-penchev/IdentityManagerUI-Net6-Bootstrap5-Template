using AspNetCore6WebApp1.Data;
using IdentityManagerUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = IsInDocker() ? builder.Configuration.GetConnectionString("DockerConnection") : builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

})
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.MapRazorPages();

if (builder.Configuration.GetValue<bool>("Identity:SeedInitialUserData"))
{
    SeedData(app);
}


app.Run();

bool IsInDocker() =>  
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

void SeedData(IHost app)
{

    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory?.CreateScope())
    {
        var dbContext = scope?.ServiceProvider.GetService<ApplicationDbContext>();
        dbContext?.Database.Migrate();
        var userManager = scope?.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        var roleManager = scope?.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
        if (userManager ==null || roleManager == null)
        {
            throw new Exception("Could not register admin accounts");
        }


        if (!roleManager.RoleExistsAsync("Admin").Result)
        {
            var result = roleManager.CreateAsync(new ApplicationRole { Name = "Admin" }).Result;
        }

        if (!roleManager.RoleExistsAsync("User").Result)
        {
            var result = roleManager.CreateAsync(new ApplicationRole { Name = "User" }).Result;
        }

        var adminEmail = "admin@test.com";
        var userEmail = "user@test.com";
        var admin = userManager.FindByEmailAsync(adminEmail).Result;
        var user = userManager.FindByEmailAsync(userEmail).Result;

        if (admin == null)
        {
            var adminUser = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
            var result = userManager.CreateAsync(adminUser, "password").Result;

            if (result.Succeeded)
            {
                var resultAdminRole = userManager.AddToRoleAsync(adminUser, "Admin").Result;
            }
        }

        if (user == null)
        {
            var regularUser = new ApplicationUser { Email = userEmail, UserName = userEmail };
            var result = userManager.CreateAsync(regularUser, "password").Result;

            if (result.Succeeded)
            {
                var userRole = userManager.AddToRoleAsync(regularUser, "User").Result;
            }
        }
    }
   

}