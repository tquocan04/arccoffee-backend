namespace ExceptionHandler.Bill
{
    public sealed class BadRequestBillByTotalPriceException() : BadRequestException("This order has an invalid total price.")
    {
    }
}
