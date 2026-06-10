namespace curtains_be.Common;

public class ResponseMessage
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static ResponseMessage Ok(string message = "Success") => new() { Success = true, Message = message };
    public static ResponseMessage Fail(string message) => new() { Success = false, Message = message };
}


