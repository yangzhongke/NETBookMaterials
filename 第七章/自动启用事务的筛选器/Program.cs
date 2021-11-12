using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 自动启用事务的筛选器;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>(opt => {
    string connStr = builder.Configuration.GetConnectionString("Default");
    opt.UseSqlServer(connStr);
});
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<TransactionScopeFilter>();
});
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
