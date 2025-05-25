namespace ExceptionHandler.User
{
    public sealed class BadRequestChangePasswordException : BadRequestException
    {
        public BadRequestChangePasswordException() : base("Password change failed") { }
    }
    
    public sealed class BadRequestCurrentPasswordException : BadRequestException
    {
        public BadRequestCurrentPasswordException() : base("Invalid current password.") { }
    }
}
