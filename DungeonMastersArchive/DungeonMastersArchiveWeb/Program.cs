using DungeonMasterArchiveData.Data;
using DungeonMastersArchiveWeb.Auth;
using DungeonMastersArchiveWeb.Components;
using DungeonMastersArchiveWeb.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<DMArchiveContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DMArchive"), opt => opt.EnableRetryOnFailure()));


builder.Services.AddScoped<DMArchiveAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp
    => sp.GetRequiredService<DMArchiveAuthenticationStateProvider>());
builder.Services.AddScoped<DMArchiveAuthenticationService>();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IValueStoreService, ValueStoreService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArticleService, ArticleService>();

var salt = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("PasswordSalt"));
builder.Services.AddScoped<IPasswordService>(s => new PasswordService(salt));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
