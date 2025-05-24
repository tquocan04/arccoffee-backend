namespace ExceptionHandler.Address
{
    public sealed class NotFoundCityException() : NotFoundException("Not found city")
    {
    }
}
