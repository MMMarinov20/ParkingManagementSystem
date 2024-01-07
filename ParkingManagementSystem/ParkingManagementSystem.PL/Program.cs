using ParkingManagementSystem.BLL.Interfaces;
using ParkingManagementSystem.BLL.Services;
using ParkingManagementSystem.DAL.Repositories;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BCryptNet = BCrypt.Net.BCrypt;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DatabaseConnector>();
builder.Services.AddScoped<IParkingLotRepository, ParkingLotRepository>();
builder.Services.AddScoped<IParkingLotService, ParkingLotService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
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

app.UseAuthorization();

app.MapControllers(); // Use top-level route registration for controllers.

app.MapRazorPages();

app.Run();

