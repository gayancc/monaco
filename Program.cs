using Microsoft.AspNetCore.SpaServices.AngularCli;
using monacoeditor;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<SafeWorkspace>();
builder.Services.AddSingleton<ScriptCompiler>();


// Serve Angular static files in production
builder.Services.AddSpaStaticFiles(cfg => { cfg.RootPath = "wwwroot"; });


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseSpaStaticFiles();
app.MapControllers();


// SPA fallback: proxy in dev, serve static in prod
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
    if (app.Environment.IsDevelopment())
    {
        spa.UseAngularCliServer(npmScript: "start");
        // Or: spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});


app.Run();