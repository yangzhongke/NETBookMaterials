using SignalR基本使用;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSignalR();
builder.Services.AddSignalR().AddStackExchangeRedis("127.0.0.1", options => {
    options.Configuration.ChannelPrefix = "Test1_";
});

string[] urls = new[] { "http://localhost:3000" };
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials())
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<ChatRoomHub>("/Hubs/ChatRoomHub");
app.MapControllers();

app.Run();
