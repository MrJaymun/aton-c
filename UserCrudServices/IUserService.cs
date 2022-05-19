using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrudModels;

namespace UserCrudServices
{
    public interface IUserService : IDisposable
    {
        UserDto Create( string creatorLogin, string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime? newUserDateOfBirth, bool newUserIsAdmin);

        UserDto UpdatePersonalInfo(string creatorLogin, string userLogin, string? newUserName, DateTime? newUserDateOfBirth, int? gender);

        UserDto UpdatePassword(string creatorLogin, string login, string password);
        UserDto UpdateLogin(string creatorLogin, string login, string newLogin);

        List<UserDto> GetActiveUsers();
        UserDto GetUserByLogin(string login);

        List<UserDto> GetUsersOlderThan(uint age);

        void DeleteUser(string creatorLogin, string login, bool isSoft);

        UserDto RecoverUser(string creatorLogin, string login);
    }
}
