namespace FlexAirFit.Application.Exceptions.ServiceException;

public class WorkoutExistsException : BaseException
{
    public WorkoutExistsException() : base() { }
    public WorkoutExistsException(Guid id) : base($"Workout ID = {id} already exists") { }
    public WorkoutExistsException(string message) : base(message) { }
    public WorkoutExistsException(string message, Exception innerException) : base(message, innerException) { }
    public WorkoutExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class WorkoutNotFoundException : BaseException 
{
    public WorkoutNotFoundException() : base() { }
    public WorkoutNotFoundException(Guid id) : base($"Workout ID = {id} not found") { }
    public WorkoutNotFoundException(string message) : base(message) { }
    public WorkoutNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public WorkoutNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}