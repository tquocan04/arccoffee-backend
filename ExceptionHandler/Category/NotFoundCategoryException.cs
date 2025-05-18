namespace ExceptionHandler.Category
{
    public sealed class NotFoundCategoryException : NotFoundException
    {
        public NotFoundCategoryException(Guid id) : base($"Category {id} does not exist in database.")
        { }
    }
}
