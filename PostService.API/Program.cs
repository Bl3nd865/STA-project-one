using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using PostService.API.Models;
using PostService.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register AppDbContext with PostgreSQL
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// Register PostService
builder.Services.AddScoped<IPostServices, PostServices>();

// Register Mapster
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
