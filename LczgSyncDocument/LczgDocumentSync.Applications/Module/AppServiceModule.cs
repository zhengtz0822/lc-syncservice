using Autofac;
using LczgDocumentSync.Applications.AppServices;
using LczgDocumentSync.Applications.Consumer;
using LczgDocumentSync.Core.Module;
using LczgDocumentSync.Interfaces;
using NewLife.Log;
using NewLife.RocketMQ;

namespace LczgDocumentSync.Applications.Module;

public class AppServiceModule:BaseModule
{
    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="builder"></param>
    protected override void Load(ContainerBuilder builder)
    {
        //注册文档服务
        builder.RegisterType<DocumentSyncAppService>().As<IDocumentSyncAppService>().PropertiesAutowired();
        builder.RegisterType<SyncRocketMqMessageAppService>().As<ISyncRocketMqMessageAppService>()
            .PropertiesAutowired();
        builder.RegisterType<SyncRocketMqMessageForDevAppService>().As<ISyncRocketMqMessageForDevAppService>()
            .PropertiesAutowired();
        builder.RegisterType<SyncRocketMqMessageForMarketAppService>().As<ISyncRocketMQMessageForMarketAppService>()
            .PropertiesAutowired();
        
        builder.Register(c => new ErpConsumer()
        {
            Topic = "ERP",
            NameServerAddress = "192.168.10.62:9876",
            FromLastOffset = false,
            BatchSize = 1,
            Log = XTrace.Log,
            AclOptions = new AclOptions()
            {
                SecretKey = "iam@2023",
                AccessKey = "iamrocketmq"
            },
            Group = "mysoftERP",
            Tags = ["appRoleAdd","appRoleModify","appRoleDelete","modifyAccountAppRoles"]
        }).As<ErpConsumer>();
        builder.Register(c => new IaMdevConsumer()
        {
            Topic = "IAMdev",
            NameServerAddress = "192.168.10.62:9876",
            FromLastOffset =true,
            BatchSize = 1,
            Log = XTrace.Log,
            AclOptions = new AclOptions()
            {
                SecretKey = "iam@2023",
                AccessKey = "iamrocketmq"
            },
            Group = "mysoftERP",
            Tags = ["accountAdd","accountModify"]
        }).As<IaMdevConsumer>();
        builder.Register(c => new MarketConsumer()
        {
            Topic = "market",
            NameServerAddress = "192.168.10.62:9876",
            FromLastOffset = false,
            BatchSize = 1,
            Log = XTrace.Log,
            AclOptions = new AclOptions()
            {
                SecretKey = "iam@2023",
                AccessKey = "iamrocketmq"
            },
            Group = "mysoftERP",
            Tags = ["appRoleAdd","appRoleModify","appRoleDelete","modifyAccountAppRoles"]
        }).As<MarketConsumer>();
    }
}