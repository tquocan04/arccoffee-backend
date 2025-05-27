namespace ExceptionHandler.Voucher
{
    public sealed class NotFoundVoucherByCodeException(string code) 
        : NotFoundException($"Voucher {code} does not exist in database")
    {
    }
}
