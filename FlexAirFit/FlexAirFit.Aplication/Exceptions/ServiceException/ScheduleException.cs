namespace FlexAirFit.Application.Exceptions.ServiceException;

public class ScheduleExistsException : BaseException
{
    public ScheduleExistsException() : base() { }
    public ScheduleExistsException(Guid id) : base($"Schedule ID = {id} already exists") { }
    public ScheduleExistsException(string message) : base(message) { }
    public ScheduleExistsException(string message, Exception innerException) : base(message, innerException) { }
    public ScheduleExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class ScheduleNotFoundException : BaseException 
{
    public ScheduleNotFoundException() : base() { }
    public ScheduleNotFoundException(Guid id) : base($"Schedule ID = {id} not found") { }
    public ScheduleNotFoundException(string message) : base(message) { }
    public ScheduleNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public ScheduleNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}