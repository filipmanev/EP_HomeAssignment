using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.DataContext;
using WebApplication1.DataAccess.Interfaces;
using WebApplication1.DataAccess.Repositories;
using WebApplication1.Presentation.Filters;
using WebApplication1.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PollDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<PollDbContext>();

string repositoryType = builder.Configuration.GetValue<string>("RepositoryType")?.ToUpper();


if (repositoryType == "JSON")
{
    builder.Services.AddScoped<IPollRepository, PollFileRepository>();
}
else { 
    builder.Services.AddScoped<IPollRepository, PollRepository>();
}

builder.Services.AddScoped<SingleVoteFilterAttribute>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Poll}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

