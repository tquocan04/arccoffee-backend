namespace ExceptionHandler.Voucher
{
    public sealed class BadRequestVoucherCodeAlreadyExistsException() : BadRequestException("This code already exists.")
    {
    }
}
