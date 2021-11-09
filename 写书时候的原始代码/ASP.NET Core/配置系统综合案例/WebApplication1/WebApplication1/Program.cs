using StackExchange.Redis;
using System.Data.SqlClient;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((_, configBuilder) => {
    string connStr = builder.Configuration.GetConnectionString("configServer");
    configBuilder.AddDbConfiguration(() => new SqlConnection(connStr));
});
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => {
    string redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnStr");
    return ConnectionMultiplexer.Connect(redisConnStr);
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
