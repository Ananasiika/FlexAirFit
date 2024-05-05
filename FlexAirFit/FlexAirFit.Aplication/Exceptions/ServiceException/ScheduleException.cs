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

public class ScheduleTimeIncorrectedException : BaseException 
{
    public ScheduleTimeIncorrectedException() : base() { }
    public ScheduleTimeIncorrectedException(Guid id) : base($"Schedule ID = {id}, time is incorrect") { }
    public ScheduleTimeIncorrectedException(string message) : base(message) { }
    public ScheduleTimeIncorrectedException(string message, Exception innerException) : base(message, innerException) { }
    public ScheduleTimeIncorrectedException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class TrainerAlreadyHasScheduleException : BaseException 
{
    public TrainerAlreadyHasScheduleException() : base() { }
    public TrainerAlreadyHasScheduleException(Guid id) : base($"Schedule ID = {id}, trainer already has schedule in this time") { }
    public TrainerAlreadyHasScheduleException(string message) : base(message) { }
    public TrainerAlreadyHasScheduleException(string message, Exception innerException) : base(message, innerException) { }
    public TrainerAlreadyHasScheduleException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class ClientAlreadyHasScheduleException : BaseException 
{
    public ClientAlreadyHasScheduleException() : base() { }
    public ClientAlreadyHasScheduleException(Guid id) : base($"Schedule ID = {id}, client already has schedule in this time") { }
    public ClientAlreadyHasScheduleException(string message) : base(message) { }
    public ClientAlreadyHasScheduleException(string message, Exception innerException) : base(message, innerException) { }
    public ClientAlreadyHasScheduleException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}