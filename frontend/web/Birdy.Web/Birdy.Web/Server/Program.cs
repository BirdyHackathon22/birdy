using Birdy.Web.Server.Services;
using Birdy.Web.Server.Services.Interfaces;
using Birdy.Web.Shared;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var baseUrl = builder.Configuration.GetValue<string>("ApiConfiguration:BaseUrl");

builder.Services.AddSingleton<ApiConfiguration>(sp => new ApiConfiguration(baseUrl));
builder.Services.AddTransient(sp => new HttpClient());
builder.Services.AddScoped<IBirdyApiService, BirdyApiService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
