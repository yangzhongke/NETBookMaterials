using AsmResolver.DotNet;
using AsmResolver.PE;
using AsmResolver.PE.File;
using MediatR;
using System.Reflection;
using Zack.Commons;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
builder.Services.RunModuleInitializers(assemblies);
builder.Services.AddMediatR(assemblies);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
