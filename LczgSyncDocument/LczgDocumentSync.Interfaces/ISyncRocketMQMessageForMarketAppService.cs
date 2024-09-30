using NewLife.RocketMQ.Protocol;

namespace LczgDocumentSync.Interfaces;

/// <summary>
/// 同步RocketMQ服务接口
/// </summary>
public interface ISyncRocketMQMessageForMarketAppService
{
    /// <summary>
    /// 开始同步
    /// </summary>
    void StartSync();

    /// <summary>
    /// 停止同步
    /// </summary>
    void StopSync();
    
    bool GetIsStart();
}