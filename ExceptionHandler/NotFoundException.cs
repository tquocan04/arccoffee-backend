namespace ExceptionHandler
{
    public abstract class NotFoundException(string message) : Exception(message)
    {
    }
}
