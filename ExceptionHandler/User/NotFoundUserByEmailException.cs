namespace ExceptionHandler.User
{
    public sealed class NotFoundUserByEmailException(string email) 
        : NotFoundException($"User with email: {email} does not exist in database.")
    {
    }
}
