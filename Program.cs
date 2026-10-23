using FamilyRecipes.Services;
using FamilyRecipes.Options;
using FamilyRecipes.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(); //razor pages
builder.Services.AddControllersWithViews(); //mvc


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

app.Run();
