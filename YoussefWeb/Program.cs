using Microsoft.EntityFrameworkCore;
using Youssef.DataAccess.Data;
using Youssef.DataAccess.Repository;
using Youssef.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Youssef.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<IdentityUser,IdentityRole>
    (/*options => options.SignIn.RequireConfirmedAccount = true This means that u enter the account only if email confirmed (Optinal)*/)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

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

app.UseAuthentication(); /* this means that it will check if the username and password exixsts in database or not (it must come before Authorization)*/

app.UseAuthorization(); /* this means that okay i will give u the access to website after u have authenticated 
                         * but wait i will check ur role and give u access to the pages ur role is allowed to enter (it must come after authentication) */

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=index}/{id?}");

app.Run();
