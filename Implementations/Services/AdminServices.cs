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
using Microsoft.AspNetCore.Hosting;

namespace Dansnom.Implementations.Services
{

    public class AdminServices : IAdminServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRoleRepository _roleRepository;
        public AdminServices(IUserRepository userRepository, IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment, IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _webHostEnvironment = webHostEnvironment;
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

            var imageName = "";
            if (model.profileImage != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(imgPath, "Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.profileImage.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath, imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    model.profileImage.CopyTo(fileStream);
                }
            }
            var user = new User
            {
                Username = model.Username,
                ProfileImage = imageName,
                Email = model.Email,
                Password = model.Password,
            };
            var adduser = await _userRepository.CreateAsync(user);

            var role = await _roleRepository.GetAsync(x => x.Name == model.Role);
            // var roles = await _roleRepository.GetAllAsync();
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
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                IsDeleted = false,
            };
            var addAdmin = await _adminRepository.CreateAsync(admins);
            return new BaseResponse
            {
                Message = "Admin Added Successfully",
                Success = true,
            };

        }

        public async Task<BaseResponse> CompleteRegistration(CreateAdminRequestModel model)
        {
            var admin = await _adminRepository.GetAsync(a => a.User.Email == model.Email);
            if (admin == null)
            {
                return new BaseResponse
                {
                    Message = "Admin not found",
                    Success = false
                };
            }
            var imageName = "";
            if (model.profileImage != null)
            {
                var imgPath = _webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(imgPath, "Images");
                Directory.CreateDirectory(imagePath);
                var imageType = model.profileImage.ContentType.Split('/')[1];
                imageName = $"{Guid.NewGuid()}.{imageType}";
                var fullPath = Path.Combine(imagePath, imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    model.profileImage.CopyTo(fileStream);
                }
            }

            admin.User.Username = model.Username ?? admin.User.Username;
            admin.FullName = model.FullName ?? admin.FullName;
            admin.User.Email = model.Email ?? admin.User.Email;
            admin.PhoneNumber = model.PhoneNumber ?? admin.PhoneNumber;
            admin.User.ProfileImage = imageName;
            var admin1 = await _adminRepository.UpdateAsync(admin);

            return new BaseResponse
            {
                Message = "Registration completed successfully",
                Success = true
            };

        }

        public async Task<BaseResponse> DeleteAdmin(int Id)
        {
            var admin = await _adminRepository.GetAsync(admins => admins.IsDeleted == false && admins.Id == Id);
            if (admin == null)
            {
                return new BaseResponse
                {
                    Message = "Admin not found",
                    Success = false
                };
            }

            admin.IsDeleted = true;
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
                    Id = x.Id,
                    Username = x.User.Username,
                    FullName = x.User.Admin.FullName,
                    PhoneNumber = x.User.Admin.PhoneNumber,
                    ProfileImage = x.User.ProfileImage,
                    Email = x.User.Email,
                    Role = x.Role.Name,
                    Description = x.Role.Description
                }).ToList()
            };
        }
        public async Task<AdminResponseModel> FindAdminAsync(int id)
        {
            var admin = await _adminRepository.GetAdminAsync(id);
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
            var admin = await _adminRepository.GetAdminAsync(id);
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