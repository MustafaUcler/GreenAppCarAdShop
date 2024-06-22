using GreenApp.Helpers;
using GreenApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add database services
builder.Services.AddDbContext<GreenAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowVueApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(app.Environment.ContentRootPath, "Images")),
    RequestPath = "/images"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
