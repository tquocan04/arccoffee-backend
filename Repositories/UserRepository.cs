using Repository.Contracts;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool CheckValidDob(int day, int month, int year)
        {
            if (year < 1 || year > DateTime.Now.Year)
                return false;

            if (month < 1 || month > 12)
                return false;

            try
            {
                var dob = new DateTime(year, month, day);

                if (dob > DateTime.Now)
                    return false;

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}
