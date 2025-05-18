namespace ExceptionHandler.Category
{
    public sealed class BadRequestCategoryNameIsExistedException : BadRequestException
    {
        public BadRequestCategoryNameIsExistedException(string name) : base($"Category {name} already exists") { }
    }
}
