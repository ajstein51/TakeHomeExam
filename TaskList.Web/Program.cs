using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TaskList.Infastructure;
using TaskList.Infastructure.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

#region ThingsIAdded
// db string
builder.Services.AddDbContextFactory<DBContext>(o 
    =>o.UseSqlServer(builder.Configuration.GetConnectionString("TaskLists")));
// Add scopes
builder.Services.AddScoped<TaskListService>();
builder.Services.AddScoped<TaskItemService>();


builder.Services.AddHttpClient();
builder.Services.AddMudServices();

// Logging to console
// To Do: Look into changing to Serilog
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // app console
builder.Logging.AddDebug(); // debug console
builder.Logging.AddEventLog(e =>
{
    e.SourceName = "CheckLists";
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

