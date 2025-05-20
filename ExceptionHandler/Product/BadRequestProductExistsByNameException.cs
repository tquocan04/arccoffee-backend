namespace ExceptionHandler.Product
{
    public sealed class BadRequestProductExistsByNameException(string name) 
        : BadRequestException($"Product {name} already exists")
    {
    }
}
