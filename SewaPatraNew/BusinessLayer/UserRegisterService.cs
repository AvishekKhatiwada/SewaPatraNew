using SewaPatra.DataAccess;
using SewaPatra.Models;

namespace SewaPatra.BusinessLayer
{
    public class UserRegisterService
    {
        private readonly UserRepository _userRepository;
        public UserRegisterService(UserRepository repository)
        {
            _userRepository = repository;
        }
        public UserRegister GetUserByNumber(string number)
        {
            return _userRepository.GetUserByNumber(number);
        }
        public UserRegister GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }
        public bool InsertUser(UserRegister user)
        {
            if (_userRepository.GetUserByNumber(user.Number) != null || _userRepository.GetUserByEmail(user.Email) != null)
            {
                return false; // User already exists
            }
            user.Password = UserRegister.HashPassword(user.Password);
            _userRepository.AddUser(user);
            return true;
        }
        public UserRegister AuthenticateUser(string username, string password)
        {
            UserRegister user = _userRepository.GetUserByNumber(username);

            if (user != null && Login.VerifyPassword(password, user.Password))
            {
                return user;
            }

            return null;
        }
        public bool UpdateUserPassword(UserRegister user)
        {
            _userRepository.ChangePassword(user);
            return true;
        }
    }
}
