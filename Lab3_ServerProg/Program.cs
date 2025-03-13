using Serilog;

var builder = WebApplication.CreateBuilder(args);
// ��������� Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()  // �������� ����������� ������� ������������
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); //Serilog ��� ������������

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

// Middleware ��� ����������� ��������
app.Use(async (context, next) =>
{
    Log.Information("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next.Invoke(); // ������� � ���������� middleware
    Log.Information("Response: {StatusCode} ��� {Method} {Path}", context.Response.StatusCode, context.Request.Method, context.Request.Path);
});

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();


// ���������� ��� 404 ������
app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
    {
        context.HttpContext.Response.Redirect("/404");
    }
    return Task.CompletedTask;
});

app.Run();
