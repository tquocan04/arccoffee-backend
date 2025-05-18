namespace ExceptionHandler.Category
{
    public sealed class NotFoundCategoryListException : NotFoundException
    {
        public NotFoundCategoryListException() : base("No result found.")
        { }
    }
}
