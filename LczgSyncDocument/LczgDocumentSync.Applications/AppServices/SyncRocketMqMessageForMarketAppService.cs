using LczgDocumentSync.Applications.Consumer;
using LczgDocumentSync.Applications.Model;
using LczgDocumentSync.Applications.Model.Dtos;
using LczgDocumentSync.Core.Options;
using LczgDocumentSync.Core.Utility;
using LczgDocumentSync.Interfaces;
using NewLife;
using NewLife.Log;
using NewLife.RocketMQ.Protocol;

namespace LczgDocumentSync.Applications.AppServices;

/// <summary>
/// 
/// </summary>
public class SyncRocketMqMessageForMarketAppService:ISyncRocketMQMessageForMarketAppService
{
    /// <summary>
    /// 通过属性注入
    /// </summary>
    public MipSettings AppSettings { get; set; }
    
    private readonly MarketConsumer _consumer;
    
    private readonly MipHelper _mipHelper;
    
    private readonly GenericHttpHelper _httpHelper;
    /// <summary>
    /// 构造函数
    /// </summary>
    public SyncRocketMqMessageForMarketAppService(MarketConsumer consumer,MipHelper mipHelper,GenericHttpHelper httpHelper)
    {
        _consumer = consumer;
        _mipHelper = mipHelper;
        _httpHelper = httpHelper;
    }
    /// <summary>
    /// 开始服务
    /// </summary>
    public void StartSync()
    {
        _consumer.OnConsume = (q, ms) =>
        {
            //消费标识
            var conSumeResult = true;
            var retryCount = _consumer.MaxRetryCount;
            foreach (var item in ms.ToList())
            {
                //不再标签内的直接过滤掉
                if (_consumer.Tags.Contains(item.Tags) == false)
                {
                    return true;
                }
                XTrace.WriteLine($"消息：主键【{item.Keys}】，messageId:{item.MsgId}，产生时间【{item.BornTimestamp.ToDateTime()}】，内容【{item.Body.ToStr()}】");
                for (int i = 0; i <= retryCount ; i++)
                {
                    try
                    {
                    
                        var isok =  SendMipInfo(item);
                        conSumeResult = isok;
                        if (isok == false)
                        {
                            XTrace.WriteLine(i == 0
                                ? $"异常消息：主键【{item.Keys}】，messageId:{item.MsgId}，产生时间【{item.BornTimestamp.ToDateTime()}】，内容【{item.Body.ToStr()}】"
                                : $"异常消息：重试次数{i},主键【{item.Keys}】，messageId:{item.MsgId}，产生时间【{item.BornTimestamp.ToDateTime()}】，内容【{item.Body.ToStr()}】");
                        }
                    }
                    catch (Exception e)
                    {
                        XTrace.WriteLine(i == 0
                            ? $"异常消息：主键【{item.Keys}】，messageId:{item.MsgId}，产生时间【{item.BornTimestamp.ToDateTime()}】，内容【{item.Body.ToStr()}】"
                            : $"异常消息：重试次数{i},主键【{item.Keys}】，messageId:{item.MsgId}，产生时间【{item.BornTimestamp.ToDateTime()}】，内容【{item.Body.ToStr()}】");
                        XTrace.WriteException(e);
                        conSumeResult =  false;
                    }

                    //如果执行成功直接结束循环
                    if (conSumeResult == true)
                    {
                        break;
                    }
                    //如果失败要重试则等待1000毫秒重试
                    Thread.Sleep(1000);
                }
                //重试次数跑完，无论如何都确认为true,不再重试
                conSumeResult = true;
            }
            return conSumeResult;
        };
        _consumer.Start();
        _consumer.IsStart = true;
    }
    
    /// <summary>
    /// 获取开启状态
    /// </summary>
    /// <returns></returns>
    public bool GetIsStart()
    {
        return _consumer.IsStart;
    }
    /// <summary>
    /// DEV Topic 要转发2个系统
    /// </summary>
    /// <param name="messageExt"></param>
    /// <returns></returns>
    protected  bool SendMipInfo(MessageExt messageExt)
    {

        var isOk = false;
        var jsonBodyStr = messageExt.Body.ToStr();
        var messageContent = new QueueMessageContent()
        {
            TagName = messageExt.Tags,
            TopicName = messageExt.Topic,
            MessageContent = jsonBodyStr
        };
        
        //
        var result = _mipHelper.Post<NewMipResponse<String>>(AppSettings.ForwardApiUrlForSlxt, messageContent);
        if (result == null ||  result.Success == false )
        {
            isOk = false;
        }
        else
        {
            isOk = true;
        }
        return isOk;
    }
    
    
    /// <summary>
    /// 停止服务
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void StopSync()
    {
        _consumer.Stop();
    }
    
}
