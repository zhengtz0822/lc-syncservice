namespace LczgDocumentSync.Core.Dto;

public class MipAccesToken
{
    /// <summary>
    /// 
    /// </summary>
    public string access_token { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int expires_in { get; set; }


    public string token_type { get; set; }


    public string scope { get; set; }
}