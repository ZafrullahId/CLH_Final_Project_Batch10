using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Auth;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Entities.Identity;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using DansnomEmailServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Dansnom.Implementations.Services
{

    public class AdminServices : IAdminServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRoleRepository _roleRepository;
        private readonly IMailServices _mailService;
        private readonly IConfiguration _config;
        private readonly IJWTAuthenticationManager _tokenService;
        private string generatedToken = null;
        public AdminServices(IUserRepository userRepository, IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment, IRoleRepository roleRepository, IMailServices mailService, IJWTAuthenticationManager jWTAuthenticationManager, IConfiguration configuration)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
            _tokenService = jWTAuthenticationManager;
            _config = configuration;
        }
            
        public async Task<BaseResponse> AddAdmin(CreateAdminRequestModel model)
        {
            var admin = await _userRepository.GetAsync(a => a.Email == model.Email);
            if (admin != null)
            {
                return new BaseResponse()
                {
                    Message = "User Already Exist",
                    Success = false,
                };
            }
            var manager = await _adminRepository.GetAdminByRoleAsync(model.Role);
            if (manager != null)
            {
                return new BaseResponse
                {
                    Message = $"can't add a new manager with this role because the former {model.Role} is still active on the system",
                    Success = false
                };
            }
            var user = new User
            {
                Email = model.Email,
                IsDeleted = true,
            };
                
            var adduser = await _userRepository.CreateAsync(user);
            var userDto = new UserDto
            {
                Id = adduser.Id,
                Email = adduser.Email,
                Role = model.Role
            };
            adduser.Token = generatedToken = _tokenService.GenerateToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(),userDto);
            var role = await _roleRepository.GetAsync(x => x.Name.ToLower() == model.Role.ToLower());
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Role not found",
                    Success = false
                };
            }

            var userRole = new UserRole
            {
                UserId = adduser.Id,
                RoleId = role.Id,
            };

            adduser.UserRoles.Add(userRole);
            await _userRepository.UpdateAsync(adduser);

            var admins = new Admin
            {
                UserId = adduser.Id,
                User = adduser,
                IsDeleted = false,
            };
            var addAdmin = await _adminRepository.CreateAsync(admins);
             var mailRequest = new MailRequest
            {
                Subject = "Complete Your Registration",
                ToEmail = user.Email,
                ToName = model.FullName,
                HtmlContent = $"<!DOCTYPE html><html><head><meta charset=\"utf-8\"><meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\"><title>Email Confirmation</title><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"><style type=\"text/css\">@media screen {{@font-face {{font-family: 'Source Sans Pro';font-style: normal;font-weight: 400;src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');}}@font-face {{font-family: 'Source Sans Pro';font-style: normal;font-weight: 700;src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');}}body,table,td,a {{-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;}}table,td {{mso-table-rspace: 0pt;mso-table-lspace: 0pt;}}img {{-ms-interpolation-mode: bicubic;}}a[x-apple-data-detectors] {{font-family: inherit !important;font-size: inherit !important;font-weight: inherit !important;line-height: inherit !important;color: inherit !important;text-decoration: none !important;}}div[style*=\"margin: 16px 0;\"] {{margin: 0 !important;}}body {{width: 100% !important;height: 100% !important;padding: 0 !important;margin: 0 !important;}}table {{border-collapse: collapse !important;}}a {{color: #1a82e2;}}img {{height: auto;line-height: 100%;text-decoration: none;border: 0;outline: none;}}</style></head><body style=\"background-color: #e9ecef;\"><div class=\"preheader\" style=\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\">A preheader is the short summary text that follows the subject line when an email is viewed in the inbox.</div><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td align=\"center\" bgcolor=\"#e9ecef\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\"><tr><td align=\"center\" valign=\"top\" style=\"padding: 36px 24px;\"><a href=\"https://sendgrid.com\" target=\"_blank\" style=\"display: inline-block;\"><img src=\"https://media.licdn.com/dms/image/C510BAQHtR8AdDc-aJg/company-logo_200_200/0/1519909536138?e=2147483647&v=beta&t=n-uF8UVHI5jdSuAZ61e6OVnV1n8PWocgp3lZ0igTpyg\" alt=\"Logo\" border=\"0\" width=\"100\" height=\"100\" style=\"display: block;border-radius: 50%;\"></a></td></tr></table></td></tr><tr><td align=\"center\" bgcolor=\"#e9ecef\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\"><tr><td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\"><h2 style=\"margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;\">Confirm Your Email Address</h2></td></tr></table></td></tr><tr><td align=\"center\" bgcolor=\"#e9ecef\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\"><tr><td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\"><p style=\"margin: 0;\">Tap the button below to confirm your email address. If you didn't create an account with <strong>Dansnom</strong>, you can safely delete this email.</p></td></tr><tr><td align=\"left\" bgcolor=\"#ffffff\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td align=\"center\" bgcolor=\"#ffffff\" style=\"padding: 12px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"center\" bgcolor=\"#1a82e2\" style=\"border-radius: 6px;\"><a href=\"http://127.0.0.1:5500/FrontEnd/AdminFrontEnd/completeRegistration.html?token={adduser.Token}\" target=\"_blank\" style=\"display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;\">Confirm</a></td></tr></table></td></tr></table></td></tr></td></tr></table><body></html>"
            };
            _mailService.SendEMailAsync(mailRequest);
            return new BaseResponse
            {
                Message = "Admin Added Successfully",
                Success = true,
            };

        }

        public async Task<BaseResponse> CompleteRegistration(CompleteManagerRegistrationRequestModel model)
        {
            var admin = await _adminRepository.GetAdminByEmailAsync(model.Email);
            if (admin == null)
            {
                return new BaseResponse
                {
                    Message = "Admin not found",
                    Success = false
                };
            }
            var imageName = "";
            if (model.ProfileImage != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(imgPath, "Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.ProfileImage.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath, imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            admin.User.Username ??= model.Username;
            admin.FullName = model.FullName ?? admin.FullName;
            admin.User.Email = model.Email ?? admin.User.Email;
            admin.PhoneNumber = model.PhoneNumber ?? admin.PhoneNumber;
            admin.User.ProfileImage = imageName;
            admin.User.IsDeleted = false;
            admin.User.Password = model.Password;
            var admin1 = await _adminRepository.UpdateAsync(admin);

            return new BaseResponse
            {
                Message = "Registration completed successfully",
                Success = true
            };

        }

        public async Task<BaseResponse> DeleteAdmin(int Id)
        {
            var admin = await _adminRepository.GetAdminByUserIdAsync(Id);
            if (admin == null)
            {
                return new BaseResponse
                {
                    Message = "Admin not found",
                    Success = false
                };
            }

            admin.User.IsDeleted = true;
            await _adminRepository.UpdateAsync(admin);
            return new BaseResponse
            {
                Message = "Administrator Successfully Deleted",
                Success = true
            };
        }

        public async Task<AdminsResponseModel> GetAllAdmins()
        {
            var admins = await _adminRepository.GetAllAdminsAsync();
            if (admins == null)
            {
                return new AdminsResponseModel
                {
                    Message = "No Admin yet",
                    Success = false
                };
            }
            return new AdminsResponseModel
            {
                Message = "Admins Found",
                Success = true,
                Data = admins.Select(x => new AdminDto
                {
                    Id = x.User.Id,
                    Username = x.User.Username,
                    FullName = x.User.Admin.FullName,
                    PhoneNumber = x.User.Admin.PhoneNumber,
                    ProfileImage = x.User.ProfileImage,
                    Email = x.User.Email,
                    Role = x.Role.Name,
                    Description = x.Role.Description
                    // ❄️
                }).ToList()
            };
        }
        public async Task<AdminResponseModel> FindAdminAsync(int id)
        {
            var admin = await _adminRepository.GetAdminByUserIdAsync(id);
            if (admin == null)
            {
                return new AdminResponseModel
                {
                    Message = "Admin not found",
                    Success = false
                };
            }
            return new AdminResponseModel
            {
                Message = "Admin Successfully found",
                Success = true,
                Data = new AdminDto
                {
                    Id = admin.Id,
                    Username = admin.User.Username,
                    FullName = admin.FullName,
                    PhoneNumber = admin.PhoneNumber,
                    ProfileImage = admin.User.ProfileImage,
                    Email = admin.User.Email,
                }
            };
        }
        public async Task<AdminResponseModel> UpdateProfile(UpdateAdminRequestModel model, int id)
        {
            var admin = await _adminRepository.GetAdminByUserIdAsync(id);
            if (admin == null)
            {
                return new AdminResponseModel
                {
                    Message = "Admin not found",
                    Success = false
                };
            }
            var imageName = "";
            if (model.ImageUrl != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(imgPath, "Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.ImageUrl.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath, imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
                admin.User.ProfileImage = imageName;
            }
            else if (model.ImageUrl == null)
            {
                admin.User.ProfileImage = admin.User.ProfileImage;
            }
            admin.User.Username = model.Username ?? admin.User.Username;
            admin.FullName = model.FullName ?? admin.FullName;
            admin.User.Email = model.Email ?? admin.User.Email;
            admin.PhoneNumber = model.PhoneNumber ?? admin.PhoneNumber;
            var admin1 = await _adminRepository.UpdateAsync(admin);
            return new AdminResponseModel
            {
                Message = "Profile Updated Successfully",
                Success = true,
                Data = new AdminDto
                {
                    Id = admin1.Id,
                    FullName = admin1.FullName,
                    Username = admin1.User.Username,
                    PhoneNumber = admin1.PhoneNumber,
                    Email = admin1.User.Email,
                    ProfileImage = admin1.User.ProfileImage,
                }
            };
        }
    }
}