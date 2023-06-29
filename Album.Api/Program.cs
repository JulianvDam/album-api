using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Album.Api.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GreetingService>();
builder.Services.AddScoped<Album.Api.Services.IAlbumService, Album.Api.Services.AlbumService>();
builder.Services.AddCors();

var Configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


//Adding the database
builder.Services.AddDbContext<Album.Api.Database.albumContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Album API", Version = "v1" });
});


var app = builder.Build();

app.UseCors(policy => policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Album API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Album.Api.Database.albumContext>();
    DBInitializer.Initialize(dbContext);
}

app.Run();

public partial class Program { }
