namespace ExceptionHandler.Product
{
    public sealed class NotFoundProductException(Guid id) 
        : NotFoundException($"Category {id} does not exist in database.")
    {
    }
}
