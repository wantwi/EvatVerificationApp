using EvatVerificationApp.services;


var builder = WebApplication.CreateBuilder(args);




//var dataSecurityOptions = builder.Services.AddOptions<DataSecurityOptions>()
//            .Bind(builder.Configuration.GetSection("DataSecurityOptions"));
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IVerification, Verification>();
//builder.Services.AddSingleton(dataSecurityOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
