namespace FlexAirFit.Application.Exceptions.ServiceException;

public class MembershipExistsException : BaseException
{
    public MembershipExistsException() : base() { }
    public MembershipExistsException(Guid id) : base($"Membership ID = {id} already exists") { }
    public MembershipExistsException(string message) : base(message) { }
    public MembershipExistsException(string message, Exception innerException) : base(message, innerException) { }
    public MembershipExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class MembershipNotFoundException : BaseException 
{
    public MembershipNotFoundException() : base() { }
    public MembershipNotFoundException(Guid id) : base($"Membership ID = {id} not found") { }
    public MembershipNotFoundException(string message) : base(message) { }
    public MembershipNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public MembershipNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}