using TravelSite.Services;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        // Remove snake case contract resolver - rely on attributes for output
        // options.SerializerSettings.ContractResolver = new DefaultContractResolver
        // {
        //     NamingStrategy = new SnakeCaseNamingStrategy()
        // };
        // Configure other Newtonsoft settings if needed (e.g., null handling)
        // options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

builder.Services.AddHttpClient();
builder.Services.AddScoped<IFlightSearchService, SerpApiFlightService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
