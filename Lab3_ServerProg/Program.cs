using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()  // Поставил минимальный уровень логгирования
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); //Serilog для логгирования

builder.Services.AddRazorPages();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// Middleware для логирования запросов
app.Use(async (context, next) =>
{
    Log.Information("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next.Invoke(); // Переход к следующему middleware
    Log.Information("Response: {StatusCode} для {Method} {Path}", context.Response.StatusCode, context.Request.Method, context.Request.Path);
});

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();


// Обработчик для 404 ошибок
app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
    {
        context.HttpContext.Response.Redirect("/404");
    }
    return Task.CompletedTask;
});

app.Run();
