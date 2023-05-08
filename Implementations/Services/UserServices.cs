using System.Linq;
using System.Threading.Tasks;
using Dansnom.Auth;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using Microsoft.Extensions.Configuration;

namespace Dansnom.Implementations.Services
{

    public class UserServices : IUserServices
    {
        private readonly IConfiguration _config;
        private readonly IJWTAuthenticationManager _tokenService;
        private string generatedToken = null;
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository, IConfiguration config, IJWTAuthenticationManager tokenService)
        {
            _userRepository = userRepository;
            _config = config;
            _tokenService = tokenService;
        }
        public async Task<UserResponseModel> Login(LoginRequestModel model)
        {

            var userRole = await _userRepository.LoginAsync(model.Email, model.Password);
            if (userRole != null)
            {
                var userDto = new UserDto
                {
                    Email = userRole.User.Email,
                    Id = userRole.UserId,
                    Role = userRole.Role.Name,
                };
                return new UserResponseModel
                {
                    Success = true,
                    Message = $"Sucessfully logged in as {userRole.Role.Name}",
                    Data = new UserDto
                    {
                        Email = userRole.User.Email,
                        Id = userRole.UserId,
                        Role = userRole.Role.Name,
                        UserName = userRole.User.Username,
                        Image = userRole.User.ProfileImage,
                        Token = generatedToken = _tokenService.GenerateToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), userDto)
                    }
                };
            }
            return new UserResponseModel
            {
                Success = false,
                Message = "Loggin Failed",
            };
        }

        public async Task<UsersResponseModel> GetUsersByRoleAsync(string role)
        {
            var users = await _userRepository.GetUserByRoleAsync(role.ToLower());
            if (users.Count == 0)
            {
                return new UsersResponseModel
                {
                    Message = $"No User found for the role {role}",
                    Success = false
                };
            }

            return new UsersResponseModel
            {
                Data = users.Select(x => new UserDto
                {
                    Role = x.Role.Name,
                    Image = x.User.ProfileImage,
                    Id = x.User.Id,
                    UserName = x.User.Username
                }).ToList(),
                Message = $"Users with {role} found successfully",
                Success = true
            };
        }
        public async Task<UserResponseModel> GetUserByTokenAsync(string token)
        {
            var user = await _userRepository.GetAsync(x => x.Token == token);
            if (user == null)
            {
                return new UserResponseModel
                {
                    Message = "User not found",
                    Success = true
                };
            }
            return new UserResponseModel
            {
                Message = "User found successfully",
                Success = true,
                Data = new UserDto
                {
                    Email = user.Email,
                }
            };
        }
    }
}