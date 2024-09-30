namespace LczgDocumentSync.Applications.Consumer;

public class ErpConsumer:NewLife.RocketMQ.Consumer
{
    /// <summary>
    /// 最大重试次数
    /// </summary>
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// 是否启动
    /// </summary>
    public bool IsStart { get; set; }
}