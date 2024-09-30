namespace LczgDocumentSync.Core.Utility;

public class FileDownloadHelper
{
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientFactory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public FileDownloadHelper(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    }

    /// <summary>
    /// 从远程URL下载文件到本地路径
    /// </summary>
    /// <param name="url">文件的远程URL</param>
    /// <param name="baseDirectory">本地保存文件的路径</param>
    public async Task DownloadFileAsync(string url, string baseDirectory)
    {
        
        // 解析URL获取文件名
        string fileName = Path.GetFileName(new Uri(url).LocalPath);

        if (fileName == "previewFile")
        {
            int lastSlashIndex = url.LastIndexOf('/');

            if (lastSlashIndex != -1)
            {
                string result = url.Substring(lastSlashIndex + 1);
                fileName = result;
            }
        }
        
        string destinationPath = Path.Combine(baseDirectory, fileName);
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentException("目标URL不能为空.", nameof(url));
        }

        if (string.IsNullOrEmpty(baseDirectory))
        {
            throw new ArgumentException("存储路径不能为空", nameof(baseDirectory));
        }
        if (!Directory.Exists(baseDirectory))
        {
            Directory.CreateDirectory(baseDirectory);
        }

        try
        {
            using var client = _clientFactory.CreateClient();
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            throw;
        }
    }


    /// <summary>
    /// 获取文件名称
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <returns></returns>
    public string GetFileName(string fileUrl)
    {
        var fileName = String.Empty;
        int lastSlashIndex = fileUrl.LastIndexOf('/');

        if (lastSlashIndex != -1)
        {
            string result = fileUrl.Substring(lastSlashIndex + 1);
            fileName = result;
        }

        return fileName;
    }
}