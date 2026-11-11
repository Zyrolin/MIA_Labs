namespace MyWebApi.Exceptions;

public class NotFoundException(string message) : Exception(message);
public class NoContentException(string message) : Exception(message);
public class BusinessRuleException(string message) : Exception(message);
public class ValidationException(string message) : Exception(message);