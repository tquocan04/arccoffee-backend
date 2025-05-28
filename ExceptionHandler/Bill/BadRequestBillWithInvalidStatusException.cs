namespace ExceptionHandler.Bill
{
    public sealed class BadRequestBillWithInvalidStatusException() 
        : BadRequestException("This order does not have a pending status.")
    {
    }
}
