namespace FlexAirFit.Application.Exceptions.ServiceException;

public class ClientExistsException : BaseException
{
    public ClientExistsException() : base() { }
    public ClientExistsException(Guid id) : base($"Client ID = {id} already exists") { }
    public ClientExistsException(string message) : base(message) { }
    public ClientExistsException(string message, Exception innerException) : base(message, innerException) { }
    public ClientExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class ClientNotFoundException : BaseException 
{
    public ClientNotFoundException() : base() { }
    public ClientNotFoundException(Guid id) : base($"Client ID = {id} not found") { }
    public ClientNotFoundException(string message) : base(message) { }
    public ClientNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public ClientNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class InvalidFreezingException : BaseException
{
    public InvalidFreezingException() : base() { }
    public InvalidFreezingException(string message) : base(message) { }
    public InvalidFreezingException(string message, Exception innerException) : base(message, innerException) { }
    public InvalidFreezingException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}