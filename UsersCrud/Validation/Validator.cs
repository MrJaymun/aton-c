using System.Text.RegularExpressions;

namespace UsersCrud.Validation
{
    public static class Validator
    {
        public static bool ValidateGender(int gender)
        {
            if (gender >= 0 && gender <= 2) {
                return true;    
            }
            return false;
        }

        public static bool ValidateLogin(string login)
        {
            Regex rx = new Regex("^[a-zA-Z0-9]+$");
            return rx.Match(login).Success;
        }

        public static bool ValidateName(string name)
        {
            Regex rx = new Regex("^[А-Яа-яёЁa-zA-Z]+$");
            return rx.Match(name).Success;
        }

        public static bool ValidatePassword(string password)
        {
            Regex rx = new Regex("^[a-zA-Z0-9]+$");
            return rx.Match(password).Success;
        }
    }
}
