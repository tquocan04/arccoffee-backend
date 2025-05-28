namespace ExceptionHandler.Shipping
{
    public sealed class NotFoundShippingByIdException(string? id) 
        : NotFoundException($"Shipping with id: {id} does not exist in database.")
    {
    }
}
