namespace ExceptionHandler.General
{
    public sealed class NotFoundListException : NotFoundException
    {
        public NotFoundListException() : base("No result found.")
        { }
    }
}
