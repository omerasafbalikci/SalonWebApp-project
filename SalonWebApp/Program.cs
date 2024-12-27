﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SalonWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// DB Context
var connectionString = builder.Configuration.GetConnectionString("dbconn")
                       ?? throw new InvalidOperationException("Connection string 'dbconn' not found.");

builder.Services.AddDbContext<SalonContext>(options =>
    options.UseSqlServer(connectionString));

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Users/Login";   // Giriş sayfası
        opt.AccessDeniedPath = "/Home/AccessDenied"; // Erişim kısıtlaması
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Oturum süresi
        opt.SlidingExpiration = true; // Oturum uzatılabilirlik
    });

// Session (Opsiyonel - Session Kullanıyorsanız Açık Bırakın)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true; // Güvenlik için HttpOnly
    options.Cookie.IsEssential = true; // GDPR için gerekli
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sırayla Authentication -> Session -> Authorization
app.UseAuthentication();
app.UseSession(); // Eğer Session kullanıyorsanız aktif bırakın
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
