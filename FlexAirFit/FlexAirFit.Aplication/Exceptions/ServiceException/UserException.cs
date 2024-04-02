namespace FlexAirFit.Application.Exceptions.ServiceException;

public class UserExistsException : BaseException
{
    public UserExistsException() : base() { }
    public UserExistsException(Guid id) : base($"User ID = {id} already exists") { }
    public UserExistsException(string message) : base(message) { }
    public UserExistsException(string message, Exception innerException) : base(message, innerException) { }
    public UserExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class UserNotFoundException : BaseException 
{
    public UserNotFoundException() : base() { }
    public UserNotFoundException(Guid id) : base($"User ID = {id} not found") { }
    public UserNotFoundException(string message) : base(message) { }
    public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public UserNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class UserRegisteredException : BaseException 
{
    public UserRegisteredException() : base() { }
    public UserRegisteredException(string message) : base(message) { }
    public UserRegisteredException(string message, Exception innerException) : base(message, innerException) { }
    public UserRegisteredException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class UserCredentialsException : BaseException 
{
    public UserCredentialsException() : base() { }
    public UserCredentialsException(string message) : base(message) { }
    public UserCredentialsException(string message, Exception innerException) : base(message, innerException) { }
    public UserCredentialsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}