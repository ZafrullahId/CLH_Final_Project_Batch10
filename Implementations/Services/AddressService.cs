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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public async Task<AddressResponseModel> GetAddressAsync(int id)
        {
            var address = await _addressRepository.GetAsync(id);
            if (address == null)
            {
                return new AddressResponseModel
                {
                    Message = "Address not found",
                    Success = false
                };
            }
            return new AddressResponseModel
            {
                Message = "Address found",
                Success = true,
                Data = new AddressDto
                {
                    State = address.State,
                    City = address.City,
                    Street = address.Street,
                    AdditionalDetails = address.AdditionalDetails,
                    PostalCode = address.PostalCode
                }
            };
        }
    }
}