namespace ExceptionHandler.User
{
    public class NotFoundUserByGoogleIdException() : NotFoundException("User with this googleId does not exist in database.")
    {
    }
}
