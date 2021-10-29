using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Users.Domain;
using Users.Infrastructure;
using Users.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(b => {
    string connStr=builder.Configuration
            .GetConnectionString("Default");
    b.UseSqlServer(connStr);
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
    options.InstanceName = "UsersDemo_";
});
builder.Services.Configure<MvcOptions>(opt => {
    opt.Filters.Add<UnitOfWorkFilter>();
});
builder.Services.AddScoped<UserDomainService>();
builder.Services.AddScoped<ISmsCodeSender,MockSmsCodeSender>();
builder.Services.AddScoped<IUserDomainRepository, UserDomainRepository>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
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
