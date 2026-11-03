using FamilyRecipes.Models;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using FamilyRecipes.Services;
using FamilyRecipes.Options;
using FamilyRecipes.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(); //razor pages
builder.Services.AddControllersWithViews(); //mvc
//Add cookie authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
    }); //cookie auth for login


//Choose which repo to use by swapping the type
builder.Services.AddSingleton<IRecipeRepository, JsonRecipeRepository>(); //stores in a JSON file, will persist when app is restarted
//builder.Services.AddSingleton<IRecipeRepository, InMemoryRecipeRepository>(); //stores in RAM memory, will be lost when app is restarted (UNCOMMENT TO USE THIS REPO)

//Bind SiteOptions from configuration
builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection("SiteOptions"));
var app = builder.Build();
//Log incoming requests to a local file
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}")
    .WithStaticAssets();

//Health endpoint returns the app version/status
app.MapGet("/health", () =>
{
    return Results.Json(new
    {
        version = "1.0.0",
        status = "Healthy"
    });
});

//API endpoint return all recipes as JSON
app.MapGet("/api/recipes", (IRecipeRepository repo) =>
{
    return Results.Json(repo.GetAll());
});




//the first user
var dataDir = Path.Combine(app.Environment.ContentRootPath, "data");
var usersFile = Path.Combine(dataDir, "users.json");
Directory.CreateDirectory(dataDir);

//Create the first user only if the file does NOT exist
if (!File.Exists(usersFile))
{
    var hash = Convert.ToHexString(
        SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("Password123")));

    var user = new[]
    {
        new User { Username = "admin", PasswordHash = hash }
    };

    File.WriteAllText(usersFile, JsonSerializer.Serialize(
        user, new JsonSerializerOptions { WriteIndented = true }));
}



app.Run();
