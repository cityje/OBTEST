using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NSwag;
using OBTEST.Controllers;
using OBTEST.DBContext;
using OBTEST.Helpers;
using OBTEST.Models;
using OBTEST.Until;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllers();

// 了解更多关于配置Swagger/OpenAPI的信息：https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TestAPI",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms")
    });
});


// 加载配置
var Configuration = builder.Configuration;

// 获取数据库配置
UtilDB_Core.GetDBIConfiguration(Configuration);

// 添加数据库上下文
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// 注册用户信息服务
builder.Services.AddScoped<UserInfo>(); // 注册 UserInfoMiddleware

// 修改 JSON 开头小写
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// 增加认证设置
builder.Services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);


var app = builder.Build();

// 配置中间件
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // 添加配置以包含认证输入字段
        c.InjectJavascript("/swagger-ui-auth.js", "text/javascript");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 添加中间件用户信息
app.UseMiddleware<UserInfoMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
