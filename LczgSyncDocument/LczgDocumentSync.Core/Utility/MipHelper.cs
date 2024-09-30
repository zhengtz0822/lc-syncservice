using LczgDocumentSync.Core.Dto;
using LczgDocumentSync.Core.Options;

namespace LczgDocumentSync.Core.Utility;

public class MipHelper
{
    private readonly GenericHttpHelper _httpHelper;
    
    /// <summary>
    /// 通过属性注入
    /// </summary>
    public MipSettings AppSettings { get; set; }

    public MipHelper(GenericHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    /// <summary>
    /// 获取Token
    /// </summary>
    /// <returns></returns>
    public  async Task<string> GetToken()
    {
        var mipurl = AppSettings.MipUrl;
        var apiurl = AppSettings.MIPApiAuthApiUrl;
        var baseAddress = new Uri(mipurl);
        var fullUri = new Uri(baseAddress, apiurl);

        var data = new
        {
            client_id = AppSettings.client_id,
            client_secret = AppSettings.client_secret
        };
        var result = await _httpHelper.PostAsync<MipAccesToken>(fullUri.ToString(), data);
        return $"Bearer {result?.access_token}";
    }

    /// <summary>
    /// 获取Token
    /// </summary>
    /// <returns></returns>
    public  string GetTokenNow()
    {
        var mipurl = AppSettings.MipUrl;
        var apiurl = AppSettings.MIPApiAuthApiUrl;
        var baseAddress = new Uri(mipurl);
        var fullUri = new Uri(baseAddress, apiurl);

        var data = new
        {
            client_id = AppSettings.client_id,
            client_secret = AppSettings.client_secret
        };
        var result =  _httpHelper.Post<MipAccesToken>(fullUri.ToString(), data);
        return $"Bearer {result?.access_token}";
    }
    /// <summary>
    /// 根据指定的method 调用不同的API
    /// </summary>
    /// <param name="methodUrl"></param>
    /// <param name="data"></param>
    /// <param name="headers">头部</param>
    /// <param name="isAuth">是否有API身份校验</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T?> PostAsync<T>(string methodUrl, object? data, Dictionary<string, string>? headers = null,bool isAuth =true) where T : new()
    {
        headers ??= new Dictionary<string, string>();
        
        if (isAuth == true)
        {
            var accesToken = await this.GetToken();
            headers.Add("Authorization",accesToken);
        }
        //构建URL
        var fullUrl = GetFullUrl(methodUrl);

        var result = await _httpHelper.PostAsync<T>(fullUrl, data, headers);
        
        return result;
    }
    /// <summary>
    /// 根据指定的method 调用不同的API
    /// </summary>
    /// <param name="methodUrl"></param>
    /// <param name="data"></param>
    /// <param name="headers">头部</param>
    /// <param name="isAuth">是否有API身份校验</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public  T? Post<T>(string methodUrl, object? data, Dictionary<string, string>? headers = null,bool isAuth =true) where T : new()
    {
        headers ??= new Dictionary<string, string>();
        
        if (isAuth == true)
        {
            var accesToken =  this.GetTokenNow();
            headers.Add("Authorization",accesToken);
        }
        //构建URL
        var fullUrl = GetFullUrl(methodUrl);

        var result =  _httpHelper.Post<T>(fullUrl, data, headers);
        
        return result;
    }

    /// <summary>
    /// 传输多部分表单数据
    /// </summary>
    /// <param name="methodUrl"></param>
    /// <param name="paramDic"></param>
    /// <param name="headers"></param>
    /// <param name="isAuth"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    // public async Task<T> PostMutilPartFormData<T>(string methodUrl, Dictionary<string, string> paramDic,
    //     Dictionary<string, string>? headers = null, bool isAuth = true) where T : BaseDto, new()
    // {
    //     
    // }
    
    /// <summary>
    /// 获取完整的API地址
    /// </summary>
    /// <param name="methodUrl"></param>
    /// <returns></returns>
    private string GetFullUrl(string methodUrl)
    {
        var mipurl = AppSettings.MipUrl;
        var apiurl = methodUrl;
        var baseAddress = new Uri(mipurl);
        var fullUri = new Uri(baseAddress, apiurl);

        return fullUri.ToString();
    }
}