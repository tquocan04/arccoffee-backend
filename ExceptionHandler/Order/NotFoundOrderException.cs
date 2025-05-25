namespace ExceptionHandler.Order
{
    public sealed class NotFoundOrderException(Guid id)
        : NotFoundException($"Order {id} does not exist in database.")
    {
    }
}