using System.Text.Json.Serialization;

namespace LczgDocumentSync.Core.Options;

[Serializable]
public class MipSettings
{
    /// <summary>
    /// MIP地址
    /// </summary>
    public string MipUrl { get; set; }

    /// <summary>
    /// MIP 身份认证API URL
    /// </summary>
    public string MIPApiAuthApiUrl { get; set; }

    
    public string client_id { get; set; }

    
    /// <summary>
    /// 
    /// </summary>
    public string client_secret { get; set; }

    /// <summary>
    /// 成本 API
    /// </summary>
    public string ForwardApiUrlForErp { get; set; }
    /// <summary>
    /// 售楼 api
    /// </summary>
    public string ForwardApiUrlForSlxt { get; set; }
    
}