using BooksEFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 筛选器1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "筛选器1", Version = "v1" });
});
builder.Services.AddDbContext<MyDbContext>(opt => {
    //如果把连接字符串写入UserScrets，那么默认在Ef Migration中是读不到的
    string connStr = builder.Configuration.GetConnectionString("Default");
    opt.UseSqlServer(connStr);
});
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<MyExceptionFilter>();
    options.Filters.Add<TransactionScopeFilter>();
    options.Filters.Add<MyActionFilter1>();
    options.Filters.Add<MyActionFilter2>();
    options.Filters.Add<RateLimitFilter>();
});
builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "筛选器1 v1"));
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
