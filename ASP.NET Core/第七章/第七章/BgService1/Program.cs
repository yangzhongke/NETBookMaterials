using BgService1;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BgService1", Version = "v1" });
});
IServiceCollection services = builder.Services;
services.AddMemoryCache();
services.AddDbContext<TestDbContext>(opt => {
    string connStr = "Server=.;Database=YouzackVNextDB;Trusted_Connection=True;";
    opt.UseSqlServer(connStr);
});
//services.AddHostedService<LoadUsersCacheBgService>();
services.AddHostedService<ExplortStatisticBgService>();
services.AddHostedService<DemoBgService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BgService1 v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
