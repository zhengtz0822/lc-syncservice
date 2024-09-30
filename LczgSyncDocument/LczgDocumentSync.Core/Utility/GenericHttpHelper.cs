using LczgDocumentSync.Core.Dto;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LczgDocumentSync.Core.Utility;

public class GenericHttpHelper
{
    private readonly IHttpClientFactory _clientFactory;

    public GenericHttpHelper(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    // 泛型GET请求
    public async Task<T> GetAsync<T>(string url)
    {
        using var client = _clientFactory.CreateClient();
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseBody)!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"请求失败: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 同步发送
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <param name="headers"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public  T? Post<T>(string url, object? data, Dictionary<string, string>? headers = null) where T : new()
    {
        // 假设这里的_clientFactory是一个可以创建HttpClient实例的工厂方法
        HttpClient client = _clientFactory.CreateClient();

        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        string jsonContent = SerializeObjectOrJson(data); // 假设SerializeObjectOrJson是一个可以序列化对象的方法
        //string jsonContent = JsonSerializer.Serialize(data);
        StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
            if (response == null)
            {
                throw new Exception("请求失败：返回值为空!");
            }
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<T>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"请求失败: {ex.Message}");
            return default(T);
        }
    }

    // 泛型POST请求支持自定义Header
    public async Task<T?> PostAsync<T>(string url, object? data, Dictionary<string, string>? headers = null) where T : new()
    {
// 假设这里的_clientFactory是一个可以创建HttpClient实例的工厂方法
        HttpClient client = _clientFactory.CreateClient();

        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        string jsonContent = SerializeObjectOrJson(data); // 假设SerializeObjectOrJson是一个可以序列化对象的方法
//string jsonContent = JsonSerializer.Serialize(data);
        StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
            if (response == null)
            {
                throw new Exception("请求失败：返回值为空!");
            }
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<T>(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"请求失败: {ex.Message}");
            return default(T);
        }
        
    }

    private string SerializeObjectOrJson(object? obj)
    {
        if (obj is string str && IsJson(str))
        {
            return str; // 或者根据需要进行进一步处理
        }
        else
        {
            return JsonConvert.SerializeObject(obj);
        }
    }

    private bool IsJson(string jsonStr)
    {

        try
        {
            dynamic json = JsonConvert.DeserializeObject(jsonStr);
            return json != null;
        }
        catch (Exception ex)
        {
            // Handle exception if needed
            return false;
        }
    }
    

}