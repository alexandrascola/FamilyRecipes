using FamilyRecipes.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Choose which repo to use by swapping the type
builder.Services.AddSingleton<IRecipeRepository, JsonRecipeRepository>(); //stores in a JSON file, will persist when app is restarted
//builder.Services.AddSingleton<IRecipeRepository, InMemoryRecipeRepository>(); //stores in RAM memory, will be lost when app is restarted (UNCOMMENT TO USE THIS REPO)

var app = builder.Build();

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

app.Run();
