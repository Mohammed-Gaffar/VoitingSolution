using httpsecurityheader;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAntiforgery(x =>
{
    x.SuppressXFrameOptionsHeader = true;
});

builder.Services.AddHsts(x =>
{
    x.Preload = true;
    x.IncludeSubDomains = true;
    x.MaxAge = TimeSpan.FromDays(60);
    x.ExcludedHosts.Add("google.com");
    x.ExcludedHosts.Add("www.google.com");
});

builder.Services.AddHttpsRedirection(x =>
{
    x.HttpsPort = 7047;
    x.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
});




var app = builder.Build();


app.UseMiddleware<CustomSecurityHeader>();

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
