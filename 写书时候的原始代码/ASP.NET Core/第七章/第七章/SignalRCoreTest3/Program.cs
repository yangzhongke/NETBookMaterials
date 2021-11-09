using SignalRCoreTest2;
using SignalRCoreTest3;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddStackExchangeRedis("127.0.0.1", options => {
        options.Configuration.ChannelPrefix = "SignalRCoreTest1_";
    });
string[] urls = new[] { "http://localhost:3000" };
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder => builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowCredentials())
);

builder.Services.AddTransient<ImportExecutor>();
builder.Services.Configure<ConnStrOptions>(
    builder.Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<ImportDictHub>("/Hubs/ImportDictHub");
app.MapControllers();

app.Run();
