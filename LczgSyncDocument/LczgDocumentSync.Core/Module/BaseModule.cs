using Autofac;
using LczgDocumentSync.Core.Utility;

namespace LczgDocumentSync.Core.Module;

public abstract class BaseModule:Autofac.Module
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="builder"></param>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<FileDownloadHelper>().PropertiesAutowired();
        builder.RegisterType<GenericHttpHelper>().PropertiesAutowired();
        builder.RegisterType<MipHelper>().PropertiesAutowired();
        builder.RegisterType<FileUpdateHelper>().PropertiesAutowired();
    }
}