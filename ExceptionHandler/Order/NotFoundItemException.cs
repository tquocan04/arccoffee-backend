namespace ExceptionHandler.Order
{
    public sealed class NotFoundItemException() : NotFoundException("This item does not exist in your cart.")
    {
    }
}
