namespace ExceptionHandler.Shipping
{
    public sealed class NotFoundShippingListException : NotFoundException
    {
        public NotFoundShippingListException() : base("No result found.") { }
    }
}
