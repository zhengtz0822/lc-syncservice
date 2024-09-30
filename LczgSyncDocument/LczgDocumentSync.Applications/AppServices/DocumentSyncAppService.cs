using LczgDocumentSync.Applications.Model;
using LczgDocumentSync.Applications.Model.Dtos;
using LczgDocumentSync.Core.Utility;
using LczgDocumentSync.Interfaces;

namespace LczgDocumentSync.Applications.AppServices;

public class DocumentSyncAppService:IDocumentSyncAppService
{
    private readonly FileDownloadHelper _filedownloadHelper;
    
    private readonly GenericHttpHelper _httpHelper;

    private readonly MipHelper _mipHelper;

    public FileUpdateHelper FileUpdateHelper { get; set; }


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="filedownloadHelper"></param>
    /// <param name="httpHelper"></param>
    /// <param name="mipHelper"></param>
    public DocumentSyncAppService(FileDownloadHelper filedownloadHelper,GenericHttpHelper httpHelper,MipHelper mipHelper)
    {
        _filedownloadHelper = filedownloadHelper;
        _httpHelper = httpHelper;
        _mipHelper = mipHelper;
    }

    /// <summary>
    /// 同步文件
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public async void SyncFiles()
    {
        var baseDirectorydir = AppContext.BaseDirectory;
        var fullDir = $"{baseDirectorydir}Documents\\{DateTime.Now:yyyyMMdd}";
        MipResponse<MDesignAlterNoSyncDocumentDtl>? mipResponse = null;
        //拉取需要同步的数据
        try
        {
            //拉取需要同步的接口地址
             mipResponse = await _mipHelper.PostAsync<MipResponse<MDesignAlterNoSyncDocumentDtl>>("/syncDocument/GetNoSyncDocumentList", null,null,true);

        }
        catch (Exception e)
        {
            return;
        }

        if (mipResponse.success == true)
        {
            var syncList = mipResponse.data.MDisignAlterList;

            if (syncList == null)
            {
                Console.WriteLine("没有需要同步的数据！");
                return;
            }
            foreach (var syncDocumentDtl in syncList)
            {
                if (string.IsNullOrWhiteSpace(syncDocumentDtl.Url))
                {
                    continue;
                }

                try
                {
                    //获取文件名称
                    var curfileName = _filedownloadHelper.GetFileName(syncDocumentDtl.Url);
                    //重新构建详细目录
                    var fileDir = $"{fullDir}\\{syncDocumentDtl.Id}\\{syncDocumentDtl.DtlRefId}";
                    await _filedownloadHelper.DownloadFileAsync(syncDocumentDtl.Url, fileDir);
                    var accesToken = await _mipHelper.GetToken();
                    //写入TOKEN信息
                    var tokenHead = new Dictionary<string, string>();
                    tokenHead.Add("Authorization",accesToken);

                    var filepath = $"{fileDir}\\{curfileName}";
                    //请求MIP 
                    //构建文档服务所需的参数
                    var dictParam = new Dictionary<string, string>();
                    dictParam.Add("Chunks","1");
                    dictParam.Add("UserId","");//默认为系统管理员admin
                    dictParam.Add("FileName",curfileName);
                    dictParam.Add("chunk_size","0");
                    dictParam.Add("SubSysFolder","0201");   //所属的系统
                    dictParam.Add("FkIdentification",Guid.NewGuid().ToString());
                    dictParam.Add("DocType","");
                    dictParam.Add("Chunk","0");
                    dictParam.Add("Name",Guid.NewGuid().ToString());
                    dictParam.Add("Id",syncDocumentDtl.Id.ToString());
                    dictParam.Add("DtlRefId",syncDocumentDtl.DtlRefId.ToString());
                    var updateFileUrl = FileUpdateHelper.GetFullUrl("/syncDocument/Upload");
               
                    await FileUpdateHelper.UploadFileAsync(updateFileUrl, filepath,dictParam,curfileName, tokenHead);
                }
                catch (Exception e)
                {
                    continue;
                }

            }
        }
       
    }
    
}