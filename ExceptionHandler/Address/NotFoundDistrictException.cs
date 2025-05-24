namespace ExceptionHandler.Address
{
    public sealed class NotFoundDistrictException() : NotFoundException("Not found district")
    {
    }
}
