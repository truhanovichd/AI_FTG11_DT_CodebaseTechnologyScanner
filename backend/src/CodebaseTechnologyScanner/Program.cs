using CodebaseTechnologyScanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Codebase Technology Scanner API",
        Version = "v1",
        Description = "API for scanning codebases to detect technologies like .csproj, package.json, and Dockerfile",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Tech Scanner Team",
        },
    });
});
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IFileScanner, FileScanner>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Codebase Technology Scanner API v1");
        options.RoutePrefix = string.Empty; // Swagger UI at root path
    });

    // Redirect root to swagger UI
    app.MapGet("/", () => Results.Redirect("/index.html")).ExcludeFromDescription();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
