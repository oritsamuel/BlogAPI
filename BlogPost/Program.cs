
using BlogPost.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; // gives access to EF Core features, like DbContext and SQL Server configuration.

var builder = WebApplication.CreateBuilder(args); //Creates a builder object for your application, configuring services, logging, and other dependencies before the app runs.

// Add services
builder.Services.AddControllers();

//swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure EF Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));//Registers your database context (AppDbContext) with the app. I.e connects your C# classes to your SQL Server database, so EF Core can read/write data.


var app = builder.Build(); //This builds the app using the settings you just configured.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Middleware
app.UseHttpsRedirection(); //automatically redirects HTTP requests to HTTPS (secure connection).
app.MapControllers(); //Tells the app to use your controllers to handle requests.

app.Run(); //Starts the application and begins listening for incoming HTTP requests.
