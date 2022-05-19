using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersCrudData;

namespace UserCrudServices
{
    public class AdditionalService : IAdditionalService
    {
        private readonly UserDataContext userDataContext;
        public AdditionalService(UserDataContext userDataContext)
        {
            this.userDataContext = userDataContext;
        }
        public bool isAdmin(string adminLogin, string adminPassword)
        {
           
            try
            {
                var admin = userDataContext.Users.FirstOrDefault(p => p.Login == adminLogin);
                if(admin == null)
                {
                    return false;
                }
                else
                {
                    if(!PasswordWorker.VerifyHashedPassword(admin.Password, adminPassword)){
                        return false;
                    }
                    return admin.Admin;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
          
        }

        public bool isUserActive(string authorLogin, string authorPassword, string userLogin)
        {
            try
            {
              
                var admin = userDataContext.Users.FirstOrDefault(p => p.Login == authorLogin);
                var user = userDataContext.Users.FirstOrDefault(p => p.Login == userLogin);
                if(user == null) return false;

              
                return PasswordWorker.VerifyHashedPassword(admin.Password, authorPassword) && user.RevokedOn == null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool isLoginAvailable(string login)
        {
           var user = userDataContext.Users.FirstOrDefault(p => p.Login == login);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    userDataContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
