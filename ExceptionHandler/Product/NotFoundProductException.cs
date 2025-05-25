namespace ExceptionHandler.Product
{
    public sealed class NotFoundProductException(Guid id) 
        : NotFoundException($"Product {id} does not exist in database.")
    {
    }
}
