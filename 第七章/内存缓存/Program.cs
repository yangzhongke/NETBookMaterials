using Microsoft.EntityFrameworkCore;
using Zack.ASPNETCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<MyDbContext>(opt => {
    string connStr = builder.Configuration.GetConnectionString("Default");
    opt.UseSqlServer(connStr);
});
builder.Services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
