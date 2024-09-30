using System.Text.Json.Serialization;
using LczgDocumentSync.Core.Dto;

namespace LczgDocumentSync.Applications.Model;

/// <summary>
/// 
/// </summary>
public class MDesignAlterNoSyncDocumentDtl:BaseDto
{

    /// <summary>
    /// 维修变更 或完工确认ID
    /// </summary>
    [JsonPropertyName("Id")]
    public Guid Id { get; set; }

    /// <summary>
    /// URL 记录ID
    /// </summary>
    [JsonPropertyName("DtlRefId")]
    public Guid DtlRefId { get; set; }


    /// <summary>
    /// 第三方传入的附件URL
    /// </summary>
    [JsonPropertyName("Url")]
    public string Url { get; set; }
}