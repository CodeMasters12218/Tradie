using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddSingleton(
	new Tradie.Models.Paypal.PaypalClient(
		builder.Configuration["PaypalOptions:ClientId"],
		builder.Configuration["PaypalOptions:ClientSecret"],
		builder.Configuration["PaypalOptions:Mode"]
		)
	);

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
	name: "userCards",
	pattern: "UserCardProfile/{action=Index}/{userId?}",
	defaults: new { controller = "UserCardProfile" });

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	//dbContext.Database.EnsureCreated();


	var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
	var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

	string[] roles = { "Admin", "Seller", "Customer" };
	foreach (var role in roles)
		if (!await roleMgr.RoleExistsAsync(role))
			await roleMgr.CreateAsync(new IdentityRole<int>(role));

	var adminEmail = "admin@example.com";
	var existing = await userMgr.FindByEmailAsync(adminEmail);

	if (existing == null)
	{
		var admin = new Admin
		{
			UserName = adminEmail,
			Email = adminEmail,
			Name = "Administrador",
			LastNames = "Sistema",
			EmailConfirmed = true
		};
		var createRes = await userMgr.CreateAsync(admin, "Admin123!");
		if (createRes.Succeeded)
			await userMgr.AddToRoleAsync(admin, "Admin");
		else
			foreach (var e in createRes.Errors)
				Console.WriteLine($"ERROR creando admin: {e.Code} / {e.Description}");
	}
	else
	{
		var entry = dbContext.Entry(existing);
		entry.Property("UserType").CurrentValue = nameof(Admin);
		await dbContext.SaveChangesAsync();

		if (!await userMgr.IsInRoleAsync(existing, "Admin"))
			await userMgr.AddToRoleAsync(existing, "Admin");

		var token = await userMgr.GeneratePasswordResetTokenAsync(existing);
		var resetRes = await userMgr.ResetPasswordAsync(existing, token, "Admin123!");
		if (!resetRes.Succeeded)
			foreach (var e in resetRes.Errors)
				Console.WriteLine($"ERROR reseteando contraseña admin: {e.Code} / {e.Description}");
	}

	var sellerEmail = "seller@example.com";
	if (await userMgr.FindByEmailAsync(sellerEmail) == null)
	{
		var seller = new Seller
		{
			UserName = sellerEmail,
			Email = sellerEmail,
			Name = "Vendedor",
			LastNames = "Prueba",
			EmailConfirmed = true
		};
		var resSeller = await userMgr.CreateAsync(seller, "Seller123!");
		if (resSeller.Succeeded)
			await userMgr.AddToRoleAsync(seller, "Seller");
	}

}

app.Run();
