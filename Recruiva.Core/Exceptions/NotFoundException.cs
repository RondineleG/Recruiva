namespace Recruiva.Web.Exceptions;

public class NotFoundException(string message, string errorCode = "NOT_FOUND") : CustomException(message, errorCode)
{ }