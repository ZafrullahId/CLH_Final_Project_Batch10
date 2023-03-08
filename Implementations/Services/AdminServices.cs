using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Entities.Identity;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using DansnomEmailServices;
using Microsoft.AspNetCore.Hosting;

namespace Dansnom.Implementations.Services
{

    public class AdminServices : IAdminServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRoleRepository _roleRepository;
        private readonly IMailServices _mailService;
        public AdminServices(IUserRepository userRepository, IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment, IRoleRepository roleRepository, IMailServices mailService)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
        }

        public async Task<BaseResponse> AddAdmin(CreateAdminRequestModel model)
        {
            var admin = await _adminRepository.GetAsync(a => a.User.Email == model.Email);
            if (admin != null)
            {
                return new BaseResponse()
                {
                    Message = "Admin Already Exist",
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
                IsDeleted = true
            };
            var adduser = await _userRepository.CreateAsync(user);
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
                HtmlContent = $"<html><body><h1>Hello {model.FullName}, Welcome to Dansnom Farm Limited.</h1><h4>Your email has been registered with Dansnom but your registration is not yet complete.</h4><h5>To complete your registration click <a href=\"http://127.0.0.1:5500/AdminFrontEnd/completeRegistration.html?email={model.Email}\">here</a></h5></body></html>",
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

            admin.User.Username = model.Username ?? admin.User.Username;
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