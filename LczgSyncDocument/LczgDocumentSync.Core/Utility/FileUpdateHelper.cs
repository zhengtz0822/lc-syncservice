using LczgDocumentSync.Core.Options;

namespace LczgDocumentSync.Core.Utility;

public class FileUpdateHelper
{
    /// <summary>
    /// 属性注入
    /// </summary>
    public IHttpClientFactory ClientFactory { get; set; }
    /// <summary>
    /// 通过属性注入
    /// </summary>
    public MipSettings AppSettings { get; set; }
    
    public async Task<string?> UploadFileAsync(string apiUrl, string filePath, Dictionary<string, string> paramDic,
        string fileName, Dictionary<string, string>? headers = null)
    {
        try
        {
            // 创建HttpClient实例
            using var httpClient = ClientFactory.CreateClient();
            
            
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            
            // 打开文件并读取为Stream
            byte[] fileContent = await File.ReadAllBytesAsync(filePath); // 替换为实际文件路径
            
            // 构建MultipartFormDataContent对象以支持multipart/form-data
            using var formDataContent = new MultipartFormDataContent();

            foreach (var param in paramDic)
            {
                formDataContent.Add(new StringContent(param.Value),param.Key);
            }
            
            // 添加文件内容
            formDataContent.Add(new ByteArrayContent(fileContent, 0, fileContent.Length), "file", fileName);
            //
            // 发送POST请求
            var response = await httpClient.PostAsync(apiUrl, formDataContent);

            // 确保响应成功
            response.EnsureSuccessStatusCode();

            // 如果服务器返回了内容，这里可以读取并返回
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 获取完整的API地址
    /// </summary>
    /// <param name="methodUrl"></param>
    /// <returns></returns>
    public string GetFullUrl(string methodUrl)
    {
        var mipurl = AppSettings.MipUrl;
        var apiurl = methodUrl;
        var baseAddress = new Uri(mipurl);
        var fullUri = new Uri(baseAddress, apiurl);

        return fullUri.ToString();
    }
}