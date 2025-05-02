using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tradie.Authentication;
using Tradie.Data;
using Tradie.Models.Users;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); 

builder.Services
    .AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string[] roles = { "Admin", "Seller", "Customer" };
    foreach (var role in roles)
        if (!await roleMgr.RoleExistsAsync(role))
            await roleMgr.CreateAsync(new IdentityRole<int>(role));

    var adminEmail = "admin@example.com";
    if (await userMgr.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new User { UserName = adminEmail, Email = adminEmail, Name = "Administrador", EmailConfirmed = true };
        var result = await userMgr.CreateAsync(admin, "Admin123!");
        if (result.Succeeded)
        {
            await userMgr.AddToRoleAsync(admin, "Admin");
        }
        else
        {
            foreach (var e in result.Errors)
                Console.WriteLine($"ERROR creando admin: {e.Code} / {e.Description}");
        }
    }

  
    var sellerEmail = "seller@example.com";
    if (await userMgr.FindByEmailAsync(sellerEmail) == null)
    {
        var seller = new Seller
        {
            UserName = sellerEmail,
            Email = sellerEmail,
            Name = "Vendedor",
            EmailConfirmed = true
        };
        var resSeller = await userMgr.CreateAsync(seller, "Seller123!");
        if (resSeller.Succeeded)
            await userMgr.AddToRoleAsync(seller, "Seller");
    }

}

app.Run();
