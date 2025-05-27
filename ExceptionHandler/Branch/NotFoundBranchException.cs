namespace ExceptionHandler.Branch
{
    public sealed class NotFoundBranchException() : NotFoundException($"This branch does not exist in database.")
    {
    }
}
