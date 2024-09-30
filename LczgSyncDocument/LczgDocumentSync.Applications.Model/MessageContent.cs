namespace LczgDocumentSync.Applications.Model;

[Serializable]
public class QueueMessageContent
{
    /// <summary>
    /// 主题名称
    /// </summary>
    public string TopicName { get; set;}

    /// <summary>
    /// 标识名称
    /// </summary>
    public string TagName { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; }
}