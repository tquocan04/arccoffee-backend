namespace ExceptionHandler.User
{
    public sealed class BadRequestLoginException() : BadRequestException("Login failed.")
    {
    }
}
