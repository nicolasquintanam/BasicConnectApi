namespace BasicConnectApi.Models;

using System.Text.Json.Serialization;

public class BaseResponse
{
    public BaseResponse(bool isSuccess, string message = "")
    {
        IsSuccess = isSuccess;
        if (string.IsNullOrEmpty(message))
        {
            if (isSuccess)
                Message = "Operation completed successfully";
            else
                Message = "An error has occurred";
        }
        else
        {
            Message = message;
        }
    }

    [JsonPropertyName("success")]
    public bool IsSuccess { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("data")]
    public object Data { get; set; } = new { };
}