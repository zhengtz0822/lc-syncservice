using Autofac;
using Hangfire;
using LczgDocumentSync.Core.Module;
using LczgDocumentSync.Core.Options;
using LczgSyncDocument.ScheduledTask.Tasks;

namespace LczgSyncDocument.ScheduledTask.Module;

/// <summary>
/// 
/// </summary>
public class ScheduledTaskModule:BaseModule
{
    
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    // protected override void Load(ContainerBuilder builder)
    // {
    //     builder.RegisterBuildCallback(container =>
    //     {
    //         //从AUTOFAC中读取配置
    //         var hangfireOptions = container.Resolve<HangfireOptions>();
    //         //读取hangfire实例
    //         var recurringJobManager = container.Resolve<IRecurringJobManager>();
    //         AddOrUpdateRecurringJobs(hangfireOptions,recurringJobManager);
    //     });
    // }
    //
    // /// <summary>
    // /// 注册调度服务
    // /// </summary>
    // /// <param name="hangfireOptions"></param>
    // /// <param name="recurringJobManager"></param>
    // private void AddOrUpdateRecurringJobs(HangfireOptions hangfireOptions,IRecurringJobManager recurringJobManager)
    // {
    //     recurringJobManager.AddOrUpdate<DocumentTask>("documentSync", x => x.DoWorkAsync(), hangfireOptions.ScheduledTask.CronExpression);
    // }
    
}