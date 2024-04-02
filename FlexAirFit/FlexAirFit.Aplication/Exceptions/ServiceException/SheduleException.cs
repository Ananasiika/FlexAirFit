namespace FlexAirFit.Application.Exceptions.ServiceException;

public class SheduleExistsException : BaseException
{
    public SheduleExistsException() : base() { }
    public SheduleExistsException(Guid id) : base($"Shedule ID = {id} already exists") { }
    public SheduleExistsException(string message) : base(message) { }
    public SheduleExistsException(string message, Exception innerException) : base(message, innerException) { }
    public SheduleExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class SheduleNotFoundException : BaseException 
{
    public SheduleNotFoundException() : base() { }
    public SheduleNotFoundException(Guid id) : base($"Shedule ID = {id} not found") { }
    public SheduleNotFoundException(string message) : base(message) { }
    public SheduleNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public SheduleNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}