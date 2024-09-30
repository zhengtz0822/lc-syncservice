using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Components;
using AntDesign.ProLayout;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Storage.SQLite;
using LczgDocumentSync.Core.Module;
using LczgDocumentSync.Core.Options;
using LczgDodumentSync.Core.Options;
using LczgSyncDocument.ScheduledTask.Tasks;
using LczgSyncDocument.ScheduledTask.Tasks.Interfaces;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


//读取 Hangfire 相关配置
var hangfireOptions = builder.Configuration.GetSection("Hangfire").Get<HangfireOptions>();
//获取配置
var appSettings = builder.Configuration.GetSection("MipSettings").Get<MipSettings>();
var deps = DependencyContext.Default;
var libraries = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith("LczgDocumentSync"));
var resultAssemblys =  libraries.Select(library => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(library.Name))).ToList();


//autofac替换标准标准DI
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
    containerBuilder =>
    {
        //注册所有的方法 通过Autofac接管
        //containerBuilder.Populate(builder.Services);
        // 注册 appSettings 到 Autofac 容器
        containerBuilder.RegisterInstance(appSettings).As<MipSettings>().SingleInstance();
        containerBuilder.RegisterInstance(hangfireOptions).As<HangfireOptions>().SingleInstance();
        containerBuilder.RegisterAssemblyModules<BaseModule>(resultAssemblys.ToArray());
    });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();


// 将数据库文件路径与当前bin目录下的data目录组合
/*无需hangfire先注释
string hangfireDbPath = Path.Combine(AppContext.BaseDirectory, "data", "hangfire.db");

//注册hangfire连接字符串
builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(hangfireDbPath));

//注册hangfire
builder.Services.AddHangfireServer();
*/
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetService<NavigationManager>()!.BaseUri)
});

builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));



builder.Services.AddHttpClient(); // 这里注册IHttpClientFactory到IServiceCollection
var option = new ScheduledTaskOptions();


// 注册 IOptions<> 以便在后面使用
builder.Services.AddOptions();




if (appSettings == null)
{
    throw new Exception("缺少必要的MIP配置信息请检查！");
}

if (hangfireOptions == null)
{
    throw new Exception("缺少必要的调度Cron配置！");
}





var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    //
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();
//使用hangfire仪表台,指定仪表台路由
//app.UseHangfireDashboard("/runtime/hangfire");

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllers();

// 使用配置的 Cron 表达式添加定时任务
var cronExpression = hangfireOptions.ScheduledTask.CronExpression;
//注册任务
//RecurringJob.AddOrUpdate<DocumentTask>("documentSync", x => x.DoWorkAsync(), cronExpression);
// RecurringJob.AddOrUpdate(
//     "myrecurringjob",
//     () => Console.WriteLine("Recurring!"),
//     Cron.Minutely);


app.Run();