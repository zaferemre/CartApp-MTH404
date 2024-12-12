using CartAPP_MTH404.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<MongoDbService>(); // MongoDB service
builder.Services.AddHttpClient(); // Register HttpClient for dependency injection
builder.Services.AddControllers(); // Register controllers for API routing

// Add CORS policy to allow requests from your frontend (Next.js or React app)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Use HSTS in production for secure connections
}

// Middleware configuration
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowReactApp"); // Apply the CORS policy

app.UseAuthorization();

// Map routes
app.MapRazorPages();
app.MapControllers(); // Map API controllers

app.Run();
