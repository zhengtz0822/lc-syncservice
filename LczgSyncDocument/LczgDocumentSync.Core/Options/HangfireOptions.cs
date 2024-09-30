using LczgDodumentSync.Core.Options;

namespace LczgDocumentSync.Core.Options;

/// <summary>
/// 定义HangfireOptions来绑定配置
/// </summary>
public class HangfireOptions
{
    public ScheduledTaskOptions ScheduledTask { get; set; }
}