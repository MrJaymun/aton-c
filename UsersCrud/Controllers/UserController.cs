using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using UserCrudModels;
using UserCrudServices;
using UsersCrud.Request.UserRequest;
using UsersCrud.Response.UserResponse;
using UsersCrud.Validation;

namespace UsersCrud.Controllers
{
    /// <summary>
    /// This controller has all the methods that should be done according to task
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IAdditionalService _additionalService;
        private readonly IUserService _userService;

        public UserController(IAdditionalService additionalService, IUserService userService)
        {
            _additionalService = additionalService;
            _userService = userService;
        }


        /// <summary>
        /// This method is used by admins to create new users
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     POST /User/createUser
        ///     {
        ///        "creatorLogin": "admin",
        ///        "creatorPassword": "password",
        ///        "newUserLogin": "coolLogin2009",
        ///        "newUserPassword": "12345",
        ///        "newUserName": "Alex",
        ///        "newUserGender": 1,
        ///        "newUserDateOfBirth": "2009-01-01",
        ///        "newUserIsAdmin": false
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">
        ///
        /// </response>
        /// <response code="400">
        /// User wasn`t created 
        /// </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("createUser")]
        public ActionResult<UserDto> CreateUser([System.Web.Http.FromUri] NewUserRequest request)
        {
            
            if (_additionalService.isAdmin(request.CreatorLogin, request.CreatorPassword))
            {


                if (!Validator.ValidateLogin(request.NewUserLogin)) return BadRequest("Incorrect symbols in login.");
                if (!Validator.ValidatePassword(request.NewUserPassword)) return BadRequest("Incorrect symbols in password.");
                if (!Validator.ValidateName(request.NewUserName)) return BadRequest("Incorrect symbols in name.");
                if (!Validator.ValidateGender(request.NewUserGender)) request.NewUserGender = 2;
                if (!_additionalService.isLoginAvailable(request.NewUserLogin)) return BadRequest("Login is used by another user.");
                if(request.NewUserDateOfBirth.Equals(new DateTime(0))){
                    request.NewUserDateOfBirth = null;
                }
                return StatusCode(201,new JsonResult(_userService.Create(request.CreatorLogin, request.NewUserLogin, request.NewUserPassword, request.NewUserName, request.NewUserGender, request.NewUserDateOfBirth, request.NewUserIsAdmin)));
            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }

        /// <summary>
        /// This method is used by admins to update information about user or users to update their own information
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     POST /User/createUser
        ///     {
        ///        "creatorLogin": "admin",
        ///        "creatorPassword": "password",
        ///        "userLogin": "login",
        ///        "newUserName": "Alexa",
        ///        "newUserDateOfBirth": "2022-05-18T",
        ///        "newUserGender": 1
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// Info about user was not updated
        /// </response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("updateUserPersonalInfo")]
        public ActionResult<UserDto> UpdateUserPersonalInfo([System.Web.Http.FromUri] NewPersonalInfoRequest request)
        {
            if (_additionalService.isAdmin(request.CreatorLogin, request.CreatorPassword) || _additionalService.isUserActive(request.CreatorLogin, request.CreatorPassword, request.CreatorLogin))
            {


                if (request.NewUserName != null)
                {
                    if (!Validator.ValidateName(request.NewUserName)) return BadRequest("Incorrect symbols in name.");
                }
                     
                if(request.NewUserGender != null)
                {
                    if (!Validator.ValidateGender((int)request.NewUserGender)) request.NewUserGender = 2;
                }
                var result = _userService.UpdatePersonalInfo(request.CreatorLogin, request.UserLogin, request.NewUserName, request.NewUserDateOfBirth, request.NewUserGender);
                if (result != null) return new JsonResult(result);
                return BadRequest("User which should be updated does not exist");


            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }
        /// <summary>
        /// This method is used by admins to update user`s password  or users to update their own password
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     POST /User/updateUserPassword
        ///     {
        ///        "creatorLogin": "admin",
        ///        "creatorPassword": "password",
        ///        "userLogin": "login",
        ///        "newPassword": "12345"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// User`s password was not updated
        /// </response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("updateUserPassword")]
        public ActionResult<UserDto> UpdateUserPassword([System.Web.Http.FromUri] NewPasswordRequest request)
        {
            if (_additionalService.isAdmin(request.CreatorLogin, request.CreatorPassword) || _additionalService.isUserActive(request.CreatorLogin, request.CreatorPassword, request.CreatorLogin))
            {
                if (!Validator.ValidatePassword(request.NewPassword)) return BadRequest("Incorrect symbols in password.");

                var result = _userService.UpdatePassword(request.CreatorLogin, request.UserLogin, request.NewPassword);
                if (result != null) return new JsonResult(result);
                return BadRequest("User which should be updated does not exist");

            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }


        /// <summary>
        /// This method is used by admins to update user`s login or users to update their own login
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     POST /User/updateUserLogin
        ///     {
        ///        "creatorLogin": "admin",
        ///        "creatorPassword": "password",
        ///        "userLogin": "login",
        ///        "newLogin": "AlsoLoginButBetter"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// User`s login was not updated
        /// </response>
       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("updateUserLogin")]
        public ActionResult<UserDto> UpdateUserLogin([System.Web.Http.FromUri] NewLoginRequest request)
        {
            if(request.UserLogin.Equals(request.NewLogin)) return BadRequest("New login should not match the current one.");
            if (_additionalService.isAdmin(request.CreatorLogin, request.CreatorPassword) || _additionalService.isUserActive(request.CreatorLogin, request.CreatorPassword, request.CreatorLogin))
            {
                if (!Validator.ValidateLogin(request.NewLogin)) return BadRequest("Incorrect symbols in login.");
                if (!_additionalService.isLoginAvailable(request.NewLogin))
                {
                    if (!_additionalService.isLoginAvailable(request.NewLogin)) return BadRequest("Login is used by another user.");
                }

                var result = _userService.UpdateLogin(request.CreatorLogin, request.UserLogin, request.NewLogin);
                if (result != null) return new JsonResult(result);
                return BadRequest("User which should be updated does not exist");
            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }
        /// <summary>
        /// This method is used by admins to get list of all active users
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     GET /User/getActiveUsers?creatorLogin=login&amp;creatorPassword=password
        ///     
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// Impossible to show data
        /// </response>
        /// <param name="creatorLogin">Login of admin</param>
        /// <param name="creatorPassword">Password of admin</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("getActiveUsers")]
        public ActionResult<List<UserDto>> GetActiveUsers(string creatorLogin, string creatorPassword)
        {
            if (_additionalService.isAdmin(creatorLogin, creatorPassword))
            {

                var result = _userService.GetActiveUsers();
                if (result != null) return new JsonResult(result);
                return BadRequest("There are no active users in database");
              

            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }
        /// <summary>
        /// This method is used by admins to get info about user
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     GET /User/getUserByLogin?creatorLogin=login&amp;creatorPassword=password&amp;userLogin=userLogin
        ///     
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// Impossible to show data
        /// </response>
        /// <param name="creatorLogin">Login of admin</param>
        /// <param name="creatorPassword">Password of admin</param>
        /// <param name="userLogin">Login of interested user </param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("getUserByLogin")]
        public ActionResult<UserMainInfoResponse> GetUserByLogin(string creatorLogin, string creatorPassword, string userLogin)
        {
            if (_additionalService.isAdmin(creatorLogin, creatorPassword))
            {
                var answer = UserMainInfoResponse(_userService.GetUserByLogin(userLogin));
                if (answer != null) return new JsonResult(answer);
                return BadRequest("User does not exist");

            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }
        /// <summary>
        /// This method is used by users to get information about themselves
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     GET /User/getMyInfo?userLogin=login&amp;userPassword=password
        ///     
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// Impossible to show data
        /// </response>
        /// <param name="userLogin">Login of user</param>
        /// <param name="userPassword">Password of user</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("getMyInfo")]
        public ActionResult<UserDto> GetMyInfo( string userLogin, string userPassword)
        {
            if (_additionalService.isUserActive(userLogin, userPassword, userLogin))
            {
                //Не создавал отдельного метода, он избыточен, так как проверка на пароль проводится выше.
               
                return new JsonResult(_userService.GetUserByLogin(userLogin));
               

            }
            else
            {
                return BadRequest("There is no active user in database with this login and password.");

            }
        }
        /// <summary>
        /// This method is used by admins to get list of users older than age
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     GET /User/getUsersOlderThan?creatorLogin=login&amp;creatorPassword=password&amp;age=0
        ///       
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// Impossible to show data
        /// </response>
        /// <param name="creatorLogin">Login of admin</param>
        /// <param name="creatorPassword">Password of admin</param>
        /// <param name="age">Age the users must be older than</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("getUsersOlderThan")]
        public ActionResult<List<UserDto>> GetUsersOlderThan(string creatorLogin, string creatorPassword, int age)
        {
            if(age < 0)
            {
                return BadRequest("Age must be non-negative number.");
            }
            if (_additionalService.isAdmin(creatorLogin, creatorPassword))
            {

                return new JsonResult(_userService.GetUsersOlderThan((uint)age));

            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }
        /// <summary>
        /// This method is used by admins to delete user 
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     DELETE /User/deleteUser
        ///     {
        ///        "creatorLogin": "admin",
        ///        "creatorPassword": "password",
        ///        "userLogin": "login",
        ///        "IsSoft": false
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// User was not deleted
        /// </response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("deleteUser")]
        public ActionResult<string> DeleteUser([System.Web.Http.FromUri] UserToDeleteRequest request)
        {
            if (_additionalService.isAdmin(request.CreatorLogin, request.CreatorPassword))
            {
                               
                if (!_additionalService.isUserActive(request.CreatorLogin, request.CreatorPassword, request.UserLogin))
                {

                    return BadRequest("User is already deleted or does not exist");
                }
                
                _userService.DeleteUser(request.CreatorLogin, request.UserLogin, request.IsSoft);
                if (request.IsSoft)
                {
                    return Ok("User is successfully deleted using soft delete");
                }
                else
                {
                    return Ok("User is successfully deleted using hard delete");
                }
            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }
        /// <summary>
        /// This method is used by admins to recover user
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     POST /User/recoverUser
        ///     {
        ///        "creatorLogin": "admin",
        ///        "creatorPassword": "password",
        ///        "userLogin": "login",
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">
        ///
        /// </response>
        /// <response code="400">
        /// User`s password was not recovered
        /// </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("recoverUser")]
        public ActionResult<UserDto> RecoverUser([System.Web.Http.FromUri] UserToRecoverRequest request)
        {
           
            if (_additionalService.isAdmin(request.CreatorLogin, request.CreatorPassword) )
            {
                if (_additionalService.isUserActive(request.CreatorLogin, request.CreatorPassword, request.UserLogin)) return BadRequest("User is not revoked.");

                var answer = _userService.RecoverUser(request.CreatorLogin, request.UserLogin);
                if (answer != null) return new JsonResult(answer);
                return BadRequest("User does not exist");

            }
            else
            {
                return BadRequest("User does not have permission to do that.");

            }
        }

        private UserMainInfoResponse UserMainInfoResponse(UserDto userDto)
        {
            if (userDto == null) return null;
            var user = new UserMainInfoResponse();
            user.Login = userDto.Login;
            user.Gender = userDto.Gender;
            user.Birthday = userDto.Birthday;
            if(userDto.RevokedOn != null)
            {
                user.IsActive = false;
            }
            else
            {
                user.IsActive = true;
            }
            return user;
        }

        
    }
}
