namespace FlexAirFit.Web2._0.Dto.Dto.Auth;

public class ErrorResponse
{
    public string Message { get; set; }

    public ErrorResponse(string message)
    {
        Message = message;
    }
}