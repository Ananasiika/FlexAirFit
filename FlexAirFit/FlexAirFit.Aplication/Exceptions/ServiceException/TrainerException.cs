namespace FlexAirFit.Application.Exceptions.ServiceException;

public class TrainerExistsException : BaseException
{
    public TrainerExistsException() : base() { }
    public TrainerExistsException(Guid id) : base($"Trainer ID = {id} already exists") { }
    public TrainerExistsException(string message) : base(message) { }
    public TrainerExistsException(string message, Exception innerException) : base(message, innerException) { }
    public TrainerExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class TrainerNotFoundException : BaseException 
{
    public TrainerNotFoundException() : base() { }
    public TrainerNotFoundException(Guid id) : base($"Trainer ID = {id} not found") { }
    public TrainerNotFoundException(string message) : base(message) { }
    public TrainerNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public TrainerNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class TrainerUserNotFoundException : BaseException 
{
    public TrainerUserNotFoundException() : base() { }
    public TrainerUserNotFoundException(Guid id) : base($"Trainer with UserID = {id} not found") { }
    public TrainerUserNotFoundException(string message) : base(message) { }
    public TrainerUserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public TrainerUserNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}