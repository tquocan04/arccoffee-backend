namespace ExceptionHandler.Product
{
    public sealed class BadRequestProductOutOfStockException() 
        : BadRequestException("This product is out of stock.")
    {
    }
}
