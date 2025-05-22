namespace ExceptionHandler.User
{
    public sealed class BadRequestUserExistsByEmailException(string email) : BadRequestException($"User with {email} already existed")
    {
    }
}
