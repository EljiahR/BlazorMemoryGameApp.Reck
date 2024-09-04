using BlazorMemoryGameApp.Client.Pages;
using BlazorMemoryGameApp.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlazorMemoryGameApp.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GamesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GamesContext") ?? throw new InvalidOperationException("Connection string 'GamesContext' not found.")));

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(BlazorMemoryGameApp.Client._Imports).Assembly);

app.MapControllers();

app.Run();
