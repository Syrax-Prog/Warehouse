using Microsoft.EntityFrameworkCore;
using Warehouse.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => Results.Redirect("/Login"));

app.UseSession();

app.MapRazorPages();

app.Run();