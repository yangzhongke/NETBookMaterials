using System.Reflection;
using ¼òµ¥µÄmvc¿ò¼Ü;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
ActionLocator locator = new ActionLocator(services,Assembly.GetEntryAssembly()!);
services.AddSingleton(locator);
services.AddMemoryCache();
var app = builder.Build();
app.UseMiddleware<MyStaticFilesMiddleware>();
app.UseMiddleware<MyWebAPIMiddleware>();
app.UseMiddleware<NotFoundMiddleware>();
app.Run();