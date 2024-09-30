using LczgDocumentSync.Interfaces;
using LczgSyncDocument.ScheduledTask.Tasks.Interfaces;

namespace LczgSyncDocument.ScheduledTask.Tasks;

/// <summary>
/// 
/// </summary>
public class DocumentTask:ITask
{

    private IDocumentSyncAppService _documentSyncAppService;
    
    public DocumentTask(IDocumentSyncAppService documentSyncAppService)
    {
        _documentSyncAppService = documentSyncAppService;
    }

    /// <summary>
    /// 下载一个文件
    /// </summary>
    public void DoWorkAsync()
    {
        _documentSyncAppService.SyncFiles();
    }
}