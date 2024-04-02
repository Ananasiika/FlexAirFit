namespace FlexAirFit.Application.Exceptions.ServiceException;

public class AdminExistsException : BaseException
{
    public AdminExistsException() : base() { }
    public AdminExistsException(Guid id) : base($"Admin ID = {id} already exists") { }
    public AdminExistsException(string message) : base(message) { }
    public AdminExistsException(string message, Exception innerException) : base(message, innerException) { }
    public AdminExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class AdminNotFoundException : BaseException 
{
    public AdminNotFoundException() : base() { }
    public AdminNotFoundException(Guid id) : base($"Admin ID = {id} not found") { }
    public AdminNotFoundException(string message) : base(message) { }
    public AdminNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public AdminNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}