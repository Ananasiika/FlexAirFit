namespace FlexAirFit.Application.Exceptions.ServiceException;

public class ClientProductException : BaseException
{
    public ClientProductException() : base() { }
    public ClientProductException(string message) : base(message) { }
    public ClientProductException(string message, Exception innerException) : base(message, innerException) { }
    public ClientProductException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}