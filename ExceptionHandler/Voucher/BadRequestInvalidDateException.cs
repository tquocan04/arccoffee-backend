namespace ExceptionHandler.Voucher
{
    public sealed class BadRequestInvalidDateException() : BadRequestException("Invalid date")
    {
    }
    
    public sealed class BadRequestInvalidExpiredDateException() : BadRequestException("Invalid expired date")
    {
    }
}
