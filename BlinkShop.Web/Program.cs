using BlinkShop.Web.AsyncDataService.Implements;
using BlinkShop.Web.AsyncDataService.Interfaces;
using BlinkShop.Web.Service;
using BlinkShop.Web.Service.IService;
using BlinkShop.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAthuService, AthuService>();
SD.CouponBaseUrlApi = builder.Configuration["ServiceUrl:CouponApi"];
SD.AthuUrlApi = builder.Configuration["ServiceUrl:AthuApi"];
SD.Product = builder.Configuration["ServiceUrl:productApi"];
///


//inject the  dependency
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAthuService, AthuService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
{
opt.ExpireTimeSpan=TimeSpan.FromHours(10);
opt.LoginPath = "/Athu/Login";
opt.AccessDeniedPath = "/Athu/AccessDenied";
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();