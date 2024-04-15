namespace FlexAirFit.Application.Exceptions.ServiceException;

public class ProductExistsException : BaseException
{
    public ProductExistsException() : base() { }
    public ProductExistsException(Guid id) : base($"Product ID = {id} already exists") { }
    public ProductExistsException(string message) : base(message) { }
    public ProductExistsException(string message, Exception innerException) : base(message, innerException) { }
    public ProductExistsException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
public class ProductNotFoundException : BaseException 
{
    public ProductNotFoundException() : base() { }
    public ProductNotFoundException(Guid id) : base($"Product ID = {id} not found") { }
    public ProductNotFoundException(string message) : base(message) { }
    public ProductNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public ProductNotFoundException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}