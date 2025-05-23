namespace ExceptionHandler.User
{
    public sealed class BadRequestInvalidDobException() : BadRequestException("Invalid DoB")
    {
    }
}
