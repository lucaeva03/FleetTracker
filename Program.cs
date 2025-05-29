using FleetTracker.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using FleetTracker.Data;
using FleetTracker.Services;
using System.Net.Http.Headers;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services.AddMudServices();

// Video Login Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.Name = "auth_token";
		options.LoginPath = "/login";
		// options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
		options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
		options.AccessDeniedPath = "/access-denied";
	});
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Program.cs: imposta Accept di default per il client
builder.Services.AddHttpClient<OrsRoutingService>(client =>
{
	client.BaseAddress = new Uri("https://api.openrouteservice.org/");
	client.Timeout = TimeSpan.FromSeconds(10);
	client.DefaultRequestHeaders
		  .Accept
		  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
	client.DefaultRequestHeaders
		  .Accept
		  .Add(new MediaTypeWithQualityHeaderValue("application/geo+json"));
});


// Connect database to the project.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FleetTrackerDbContext>(options =>
options.UseSqlServer(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<FleetTrackerDbContext>();
	VehicleStateUpdater.UpdateAllStates(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Login Authentication
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
