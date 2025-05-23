namespace Repository.Contracts
{
    public interface IUserRepository
    {
        bool CheckValidDob(int day, int month, int year);
    }
}
