using System.Text.Json.Serialization;
using LczgDocumentSync.Core.Dto;

namespace LczgDocumentSync.Applications.Model.Dtos;


public class MipResponse<T> 
{
    [JsonPropertyName("data")]
    public MDisignAlterListDto<T> data { get; set; }
    
    [JsonPropertyName("success")]
    public bool success { get; set; }
    
    [JsonPropertyName("message")]

    public string message { get; set; }

    [JsonPropertyName("exception")]
    public string exception { get; set; }
    
    [JsonPropertyName("code")]

    public string code { get; set; }
}


public class NewMipResponse<T> 
{
    
    [JsonPropertyName("Success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("Message")]

    public string Message { get; set; }

    [JsonPropertyName("Exception")]
    public string Exception { get; set; }
    
    [JsonPropertyName("Code")]

    public string Code { get; set; }
}


public class MDisignAlterListDto<T>
{
    [JsonPropertyName("MDisignAlterList")]
    public List<T> MDisignAlterList { get; set; }
}

[Serializable]
public class ErpResponse<T>
{
    
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("Data")]
    public List<T> Data { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("Code")]
    public string Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("Message")]
    public string Message { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("Timestamp")]
    public long Timestamp { get; set; }
}