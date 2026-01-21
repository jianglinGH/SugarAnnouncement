 
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using SugarAnnouncement.Api.Middleware;
using SugarAnnouncement.Core.Interfaces;
using SugarAnnouncement.Infrastructure.Data;
using SugarAnnouncement.Infrastructure.Respositories;
using System.Text.Json;
using System.Text.Json.Serialization;

// 配置全局日志系统 Debug/Info/Warning/Error
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/api-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();


try {
    Log.Information("-------启动 Sugar API 服务-------");
    //构建ASP.NET Core Web应用构建器
    var builder = WebApplication.CreateBuilder(args);


    var folder = Path.Combine(AppContext.BaseDirectory, "Data");
    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
    var dbPath = Path.Combine(folder, "announcement_sugar.db");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
     
    builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

    builder.Services.AddControllers()
        .AddJsonOptions(options => {
            // 枚举能传字符串
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
        });
     

    //使用日志系统
    builder.Host.UseSerilog();

    ////配置 web 服务器监听端口和协议
    //builder.WebHost.ConfigureKestrel(options =>
    //{
    //    options.ListenAnyIP(5153); // HTTP
    //    options.ListenAnyIP(7292, listenOptions => listenOptions.UseHttps());
    //});


    //注册依赖和服务，数据库上下文 仓储服务 应用服务 Swagger Controllers等
    ConfigureServices(builder.Services);

    //创建可运行的 WebApplication 实例 
    var app = builder.Build();

    using (var scope = app.Services.CreateScope()) { 
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();    // 如果表不存在创建数据库表
    }

        // 配置请求管道 UseRouting UseAuthentication UseAuthorization MapControllers
        // 解析路由-识别身份-权限校验-控制器执行-返回响应
        ConfigurePipeline(app);  
     
    app.UseHttpsRedirection();

    app.Run();

} catch (Exception ex) {
    Log.Fatal(ex, "应用程序启动失败");
}


void ConfigureServices(IServiceCollection services) {
    // 添加控制器MVC服务 配置 Json 序列化
    services.AddControllers()
        .AddJsonOptions(options => { 
            // 属性名改成驼峰输出
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //美化输出
            options.JsonSerializerOptions.WriteIndented = true;
            //枚举转字符串
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            // 序列化忽视null属性
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    // 配置 CORS 允许跨域
    services.AddCors(options =>
    {
        options.AddPolicy("SugarPolicy", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("Content-Disposition");
        });
    });

    ////添加内存缓存
    //services.AddMemoryCache();

    ////添加Http上下文访问器，用于非 Controller/Service 访问 HttpContext
    //services.AddHttpContextAccessor();

    //配置 Swagger 生成API描述信息
    services.AddEndpointsApiExplorer();
    // 生成Swagger 文档和UI
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Sugar API",
            Version = "v1",
            Description = "为 Sugar BI 提供接口服务",
            Contact = new OpenApiContact
            {
                Name = "技术支持",
                Email = "support@example.com"
            }
        });
    });

    //services.AddAuthorization();

}

void ConfigurePipeline(WebApplication app) {
    if (app.Environment.IsDevelopment()) {
        app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sugar API V1");
            c.RoutePrefix = string.Empty;
            c.DocumentTitle = "Sugar API 接口文档";
        });

    }
     

    //启用路由中间件
    app.UseRouting();
 
    //启用授权中间件
    app.UseAuthorization();

    app.UseCors("SugarPolicy");

    app.UseMiddleware<DomainExceptionMiddleware>();
    //映射控制器路由
    app.MapControllers();
}