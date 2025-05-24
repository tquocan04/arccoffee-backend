namespace ExceptionHandler.Address
{
    public sealed class NotFoundAddressException(Guid objId) : NotFoundException($"Not found address of object {objId}")
    {
    }
}
