namespace ExceptionHandler.Address
{
    public sealed class NotFoundRegionException() : NotFoundException("Not found region")
    {
    }
}
