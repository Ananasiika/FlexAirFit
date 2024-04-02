namespace FlexAirFit.Application.Exceptions.ServiceException;

public class BonusExistsException : BaseException
{
    public BonusExistsException() : base() { }
    public BonusExistsException(Guid id) : base($"Bonus ID = {id} already exists") { }
    public BonusExistsException(string message) : base(message) { }
    public BonusExistsException(string message, Exception innerException) : base(message, innerException) { }
    public BonusExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class BonusNotFoundException : BaseException 
{
    public BonusNotFoundException() : base() { }
    public BonusNotFoundException(Guid id) : base($"Bonus ID = {id} not found") { }
    public BonusNotFoundException(string message) : base(message) { }
    public BonusNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public BonusNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class BonusByClientNotFoundException : BaseException 
{
    public BonusByClientNotFoundException() : base() { }
    public BonusByClientNotFoundException(Guid id) : base($"Client with ID = {id} has no bonuses") { }
    public BonusByClientNotFoundException(string message) : base(message) { }
    public BonusByClientNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public BonusByClientNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}