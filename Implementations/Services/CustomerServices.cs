using System.Threading.Tasks;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Interface.Repositories;
using Dansnom.Dtos.RequestModel;
using Dansnom.Entities;
using Dansnom.Entities.Identity;
using Dansnom.Dtos;
using Dansnom.Interface.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using DansnomEmailServices;
using Microsoft.AspNetCore.Mvc;

namespace Dansnom.Implementations.Services
{

    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRoleRepository _roleRepository;
        private readonly IMailServices _mailService;
        private readonly IverificationCodeRepository _verificationCodeRepository;
        public CustomerServices(ICustomerRepository customerRepository, IUserRepository userRepository, IWebHostEnvironment webHostEnvironment, IRoleRepository roleRepository, IMailServices mailServices, IverificationCodeRepository verificationCodeRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _roleRepository = roleRepository;
            _mailService = mailServices;
            _verificationCodeRepository = verificationCodeRepository;
        }
        public async Task<CustomerReponseModel> RegisterAsync(CreateCustomerRequestModel model)
        {
            int random = new Random().Next(10000, 99999);
            var exist = await _customerRepository.ExistsAsync(x => x.User.Email == model.Email);
            if (exist)
            {
                return new CustomerReponseModel
                {
                    Message = "Email already in use",
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
            }
            var user = new User
            {
                ProfileImage = imageName,
                Username = model.Username,
                Email = model.Email,
                Password = model.password,
                IsDeleted = true
            };

            var role = await _roleRepository.GetAsync(x => x.Name == "customer");
            if (role == null)
            {
                return new CustomerReponseModel
                {
                    Message = "Role not found",
                    Success = false
                };
            }
            var addUser = await _userRepository.CreateAsync(user);
            var userRole = new UserRole
            {
                UserId = addUser.Id,
                RoleId = role.Id,
            };
            addUser.UserRoles.Add(userRole);
            await _userRepository.UpdateAsync(addUser);

            var customer = new Customer
            {
                UserId = addUser.Id,
                User = addUser,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                IsDeleted = false,
            };
            var cust = await _customerRepository.CreateAsync(customer);

            var code = new VerificationCode
            {
                Code = random,
                CustomerId = cust.Id
            };
            await _verificationCodeRepository.CreateAsync(code);

            var mailRequest = new MailRequest
            {
                Subject = "Confirmation Code",
                ToEmail = user.Email,
                ToName = customer.FullName,
                HtmlContent = $"<html><body><h1>Hello {customer.FullName}, Welcome to Dansnom Farm Limited.</h1><h4>Your confirmation code is {code.Code} to continue with the registration</h4></body></html>",
            };
            _mailService.SendEMailAsync(mailRequest);

            return new CustomerReponseModel
            {
                Message = "Successfully Registered",
                Success = true,
                Data = new CustomerDto
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    Username = customer.User.Username,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.User.Email,
                    ImageUrl = customer.User.ProfileImage,
                }
            };
        }

        public async Task<BaseResponse> VerifyCode(int id,int verificationcode)
        {
            var code = await _verificationCodeRepository.GetAsync(x => x.Customer.Id == id && x.Code == verificationcode);
            if (code == null)
            {
                return new BaseResponse
                {
                    Message = "invalid code",
                    Success = false
                };
            }
            else if ((DateTime.Now - code.CreatedOn ).TotalSeconds > 200)
            {
                return new BaseResponse
                {
                    Message = "Code Expired",
                    Success = false,
                };
            }
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            customer.User.IsDeleted = false;
            await _customerRepository.UpdateAsync(customer);
            return new BaseResponse
            {
                Message = "Email Successfully Verified",
                Success = true,
            };
        }

        public async Task<CustomerReponseModel> UpdateProfile(UpdateCustomerRequestModel model, int id)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(id);
            if (customer == null)
            {
                return new CustomerReponseModel
                {
                    Message = "Customer not found",
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
            }
            customer.User.Username = model.Username ?? customer.User.Username;
            customer.FullName = model.FullName ?? customer.FullName;
            customer.User.Email = model.Email ?? customer.User.Email;
            customer.PhoneNumber = model.PhoneNumber ?? customer.PhoneNumber;
            customer.User.ProfileImage = imageName;
            var cust = await _customerRepository.UpdateAsync(customer);
            return new CustomerReponseModel
            {
                Message = "Profile Updated Successfully",
                Success = true,
                Data = new CustomerDto
                {
                    Id = cust.Id,
                    FullName = cust.FullName,
                    Username = cust.User.Username,
                    PhoneNumber = cust.PhoneNumber,
                    Email = cust.User.Email,
                    ImageUrl = cust.User.ProfileImage,
                }
            };
        }
        public async Task<CustomerReponseModel> GetByidAsnc(int id)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(id);
            if (customer == null)
            {
                return new CustomerReponseModel
                {
                    Message = "Customer not found",
                    Success = false
                };
            }
            return new CustomerReponseModel
            {
                Message = "Customer Profile found",
                Success = true,
                Data = new CustomerDto
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    Username = customer.User.Username,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.User.Email,
                    ImageUrl = customer.User.ProfileImage,
                }
            };
        }
        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(id);
            if (customer == null)
            {
                return new CustomerReponseModel
                {
                    Message = "Customer not found",
                    Success = false
                };
            }
            customer.IsDeleted = true;
            await _customerRepository.UpdateAsync(customer);
            return new BaseResponse
            {
                Message = "Customer Dealeted successfully",
                Success = true
            };
        }

    }
}