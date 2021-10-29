using BooksEFCore;
using Microsoft.EntityFrameworkCore;
using 分布式缓存1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "分布式缓存1", Version = "v1" });
});
builder.Services.AddDbContext<MyDbContext>(opt => {
    //如果把连接字符串写入UserScrets，那么默认在Ef Migration中是读不到的
    string connStr = builder.Configuration.GetConnectionString("Default");
    opt.UseSqlServer(connStr);
});
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
    options.InstanceName = "yzk_";
});
builder.Services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "分布式缓存1 v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
