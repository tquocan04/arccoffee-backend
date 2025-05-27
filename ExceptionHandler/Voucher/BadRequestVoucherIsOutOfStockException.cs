namespace ExceptionHandler.Voucher
{
    public sealed class BadRequestVoucherIsOutOfStockException() : BadRequestException($"This voucher is out of stock.")
    {
    }
}
