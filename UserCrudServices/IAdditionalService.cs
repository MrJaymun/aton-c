using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCrudServices
{
    public interface IAdditionalService : IDisposable
    {
        bool isAdmin(string adminLogin, string adminPassword);
        bool isUserActive(string authorLogin, string authorPassword, string userLogin);

        bool isLoginAvailable(string login);
    }
}
