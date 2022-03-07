using CommonInitializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Nest;
using SearchService.Domain;
using SearchService.Infrastructure;
using SearchService.WebAPI.Options;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureDbConfiguration();
builder.ConfigureExtraServices(new InitializerOptions
{
    LogFilePath = "e:/temp/SearchService.log",
    EventBusQueueName = "SearchService.WebAPI"
});
// Add services to the container.

builder.Services.Configure<ElasticSearchOptions>(builder.Configuration.GetSection("ElasticSearch"));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SearchService.WebAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SearchService.WebAPI v1"));
}
app.UseZackDefault();
app.MapControllers();

app.Run();
