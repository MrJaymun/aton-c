using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrudModels;
using UsersCrudData;

namespace UserCrudServices
{
    public class UserService : IUserService
    {
        private readonly UserDataContext userDataContext;
        public UserService(UserDataContext userDataContext)
        {
            this.userDataContext = userDataContext;
        }
        public UserDto Create(string creatorLogin, string newUserLogin, string newUserPassword, string newUserName, int newUserGender, DateTime? newUserDateOfBirth, bool newUserIsAdmin)    
        {
            var newUser = new User();
            newUser.Guid = Guid.NewGuid();
            newUser.Login = newUserLogin;
            newUser.Password = PasswordWorker.HashPassword(newUserPassword);
            newUser.Name = newUserName;
            newUser.Gender = newUserGender;
            newUser.Birthday = newUserDateOfBirth;
            newUser.Admin = newUserIsAdmin;
            newUser.CreatedOn = DateTime.Now;
            newUser.CreatedBy = creatorLogin;
            userDataContext.Users.Add(newUser);
            userDataContext.SaveChanges();
            return Converter(newUser);
        }

        public UserDto UpdatePersonalInfo(string creatorLogin, string userLogin, string? newUserName, DateTime? newUserDateOfBirth, int? gender)
        {
            var user = userDataContext.Users.FirstOrDefault(p => p.Login == userLogin);
            if (user == null) return null;
            if(newUserName != null)
            {
                user.Name = newUserName;
            }
            if(newUserDateOfBirth != null)
            {
                user.Birthday = newUserDateOfBirth;
            }
            if(gender != null)
            {
                user.Gender = (int)gender;
            }
            user.ModifiedBy = creatorLogin;
            user.ModifiedOn = DateTime.Now;
            userDataContext.SaveChanges();
            return Converter(user);
        }

        public UserDto UpdatePassword(string creatorLogin, string login, string password)
        {
            var user = userDataContext.Users.FirstOrDefault(p => p.Login == login);
            if (user == null) return null;
            user.Password = PasswordWorker.HashPassword(password);
            user.ModifiedBy = creatorLogin;
            user.ModifiedOn = DateTime.Now;
            userDataContext.SaveChanges();

            return Converter(user);
        }

        public UserDto UpdateLogin(string creatorLogin, string login, string newLogin)
        {
            var user = userDataContext.Users.FirstOrDefault(p => p.Login == login);
            if (user == null) return null;
            user.Login = newLogin;
            user.ModifiedBy = creatorLogin;
            user.ModifiedOn = DateTime.Now;
            userDataContext.SaveChanges();
            return Converter(user);
        }

        public List<UserDto> GetActiveUsers()
        {
            var users = userDataContext.Users.Where(p => p.RevokedOn == null).OrderBy(x => x.CreatedOn).ToList();
            if (users == null) return null;
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                userDtos.Add(Converter(user));
            }
            return userDtos;
        }

        public UserDto GetUserByLogin(string login)
        {
           var user = userDataContext.Users.FirstOrDefault(p => p.Login == login);
            if (user == null) return null;
            return Converter(user);
        }

        public List<UserDto> GetUsersOlderThan(uint age)
        {
            var users = userDataContext.Users.Where(p => (DateTime.Now - ((DateTime)p.Birthday)).TotalDays/365 > age).ToList();
            if (users == null) return null;
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                userDtos.Add(Converter(user));
            }
            return userDtos;
        }



        public void DeleteUser(string creatorLogin,  string login, bool isSoft)
        {
            var user = userDataContext.Users.FirstOrDefault(p => p.Login == login);
            if (isSoft)
            {
                user.RevokedBy = creatorLogin;
                user.RevokedOn = DateTime.Now;
                user.ModifiedBy = creatorLogin;
                user.ModifiedOn = DateTime.Now;
            }
            else
            {
                userDataContext.Users.Remove(user);
            }
            userDataContext.SaveChanges();
        }

        public UserDto RecoverUser(string creatorLogin, string login)
        {
            var user = userDataContext.Users.FirstOrDefault(p => p.Login == login);
            if (user == null) return null;
            user.RevokedBy = null;
            user.RevokedOn = null;
            user.ModifiedBy = creatorLogin;
            user.ModifiedOn = DateTime.Now;
            userDataContext.SaveChanges();
            return Converter(user);
        }
        private UserDto Converter(User user)
        {
            var userDto = new UserDto();
            userDto.Guid = user.Guid;
            userDto.Login = user.Login;
            userDto.Password = user.Password;
            userDto.Name = user.Name;
            userDto.Gender = user.Gender;
            userDto.Birthday = user.Birthday;
            userDto.Admin = user.Admin;
            userDto.CreatedOn = user.CreatedOn;
            userDto.CreatedBy = user.CreatedBy;
            userDto.ModifiedOn = user.ModifiedOn;
            userDto.ModifiedBy = user.ModifiedBy;
            userDto.RevokedOn = user.RevokedOn;
            userDto.RevokedBy = user.RevokedBy;
           
           
            
            return userDto;

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
