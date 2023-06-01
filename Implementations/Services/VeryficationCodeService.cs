using System;
using System.Threading.Tasks;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Dtos.ResponseModels;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using DansnomEmailServices;

namespace Dansnom.Implementations.Services
{
    public class VeryficationCodeService : IVerificationCodeService
    {
        private readonly IMailServices _mailService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVerificationCodeRepository _verificationCodeRepository;
        private readonly IUserRepository _userRepository;
        public VeryficationCodeService(IVerificationCodeRepository verificationCodeRepository, ICustomerRepository customerRepository, IMailServices mailServices, IUserRepository userRepository)
        {
            _mailService = mailServices;
            _customerRepository = customerRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse> VerifyCode(int id, int verificationcode)
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
            else if ((DateTime.Now - code.CreatedOn).TotalSeconds > 200)
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
        public async Task<BaseResponse> UpdateVeryficationCodeAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Customer not found",
                    Success = false
                };
            }
            var code = await _verificationCodeRepository.GetAsync(x => x.CustomerId == id);
            if (code == null)
            {
                return new BaseResponse
                {
                    Message = "No code has been sent to you before",
                    Success = false
                };
            }
            int random = new Random().Next(10000, 99999);
            code.Code = random;
            code.CreatedOn = DateTime.Now;
            var mailRequest = new MailRequest
            {
                Subject = "Confirmation Code",
                ToEmail = customer.User.Email,
                ToName = customer.FullName,
                HtmlContent = $"<html><body><h1>Hello {customer.FullName}, Welcome to Dansnom Farm Limited.</h1><h4>Your confirmation code is {code.Code} to continue with the registration</h4></body></html>",
            };
            _mailService.SendEMailAsync(mailRequest);
            await _verificationCodeRepository.UpdateAsync(code);
            return new BaseResponse
            {
                Message = "Code Successfully resent",
                Success = true
            };
        }
        public async Task<ResetPasswordResponseModel> SendForgetPasswordVerificationCode(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null)
            {
                return new ResetPasswordResponseModel
                {
                    Message = "Email not found",
                    Success = false
                };
            }
            var code = await _verificationCodeRepository.GetAsync(x => x.CustomerId == customer.Id);
            if (code == null)
            {
                return new ResetPasswordResponseModel
                {
                    Message = "No Code has been sent to at registration point",
                    Success = false
                };
            }
            int random = new Random().Next(10000, 99999);
            code.Code = random;
            code.CreatedOn = DateTime.Now;
            var mailRequest = new MailRequest
            {
                Subject = "Reset Password",
                ToEmail = customer.User.Email,
                ToName = customer.FullName,
                HtmlContent = $"<html><body><h1>Hello {customer.FullName}, Welcome</h1><h4>Your Password reset code is {code.Code} to reset your password</h4></body></html>",
            };
            _mailService.SendEMailAsync(mailRequest);
            customer.User.IsDeleted = true;
            await _customerRepository.UpdateAsync(customer);
            await _verificationCodeRepository.UpdateAsync(code);
            return new ResetPasswordResponseModel
            {
                Id = customer.Id,
                Message = "Reset Password Code Successfully Reset",
                Success = true
            };
        }
    }
}