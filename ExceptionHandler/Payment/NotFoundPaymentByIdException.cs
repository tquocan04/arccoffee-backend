namespace ExceptionHandler.Payment
{
    public sealed class NotFoundPaymentByIdException(string? id)
        : NotFoundException($"Payment with id: {id} does not exist in database.")
    {
    }
}
