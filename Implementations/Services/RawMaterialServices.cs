using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using Dansnom.Enums;
using System;

namespace Dansnom.Implementations.Services
{
    public class RawMaterialServices : IRwavMaterialServuce
    {
        private readonly IRawMaterialRepository _rawMaterialRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductionRawMaterialRepository _productionRawMaterialRepository;
        private readonly IAdminRepository _adminRepository;
        public RawMaterialServices(IRawMaterialRepository rawMaterialRepository, IProductRepository productionRepository, IProductionRawMaterialRepository productionRawMaterialRepository,IAdminRepository adminRepository)
        {
            _rawMaterialRepository = rawMaterialRepository;
            _productRepository = productionRepository;
            _productionRawMaterialRepository = productionRawMaterialRepository;
            _adminRepository = adminRepository;
        }
        public async Task<BaseResponse> CreateRawMaterial(CreateRawMaterialRequestModel model,int id)
        {
            var admin = await _adminRepository.GetAdminByUserIdAsync(id);
            if (admin == null)
            {
                return new BaseResponse
                {
                    Message = "Manager not found",
                    Success = false
                };
            }
            var rawMaterial = new RawMaterial
            {
                AdminId = admin.Id,
                QuantiityBought = model.QuantiityBought,
                QuantiityRemaining = model.QuantiityBought,
                Cost = model.Cost,
                Name = model.Name,
                AdditionalMessage = model.AdditionalMessage,
                ApprovalStatus = ApprovalStatus.Pending
            };
            await _rawMaterialRepository.CreateAsync(rawMaterial);
            return new BaseResponse
            {
                Message = $"A new Raw Material request has been created for {rawMaterial.Name}",
                Success = true
            };
        }

        public async Task<RawMaterialsResponseModel> GetAllAprovedRawMateralsForTheYear(int year)
        {
            var rawMaterial = await _rawMaterialRepository.GetAllAprovedRawMaterialForTheYearAsync(year);
            if (rawMaterial.Count == 0)
            {
                return new RawMaterialsResponseModel
                {
                    Message = $"No approved raw materials for the year {year}",
                    Success = false
                };
            }

             foreach (var item in rawMaterial)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }
            return new RawMaterialsResponseModel
            {
                Message = "Production Cost found",
                Success = true,
                Data = rawMaterial.Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    QuantiityBought = x.QuantiityBought,
                    QuantiityRemaining = x.QuantiityRemaining,
                    Name = x.Name,
                    AdditionalMessage = x.AdditionalMessage,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ManagerImage = x.Admin.User.ProfileImage,
                    ManagerName = x.Admin.User.Username,

                }).ToList()
            };
        }
        public async Task<RawMaterialsResponseModel> GetAllRejectedRawMaterialForTheYearAsync(int year)
        {
            var rawMaterial = await _rawMaterialRepository.GetAllRejectedRawMaterialForTheYearAsync(year);
            if (rawMaterial.Count == 0)
            {
                return new RawMaterialsResponseModel
                {
                    Message = $"No Unapproved raw materials for the year {year}",
                    Success = false
                };
            }
            foreach (var item in rawMaterial)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }
            return new RawMaterialsResponseModel
            {
                Message = "Production Cost found",
                Success = true,
                Data = rawMaterial.Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    QuantiityBought = x.QuantiityBought,
                    QuantiityRemaining = x.QuantiityRemaining,
                    Name = x.Name,
                    AdditionalMessage = x.AdditionalMessage,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ManagerImage = x.Admin.User.ProfileImage,
                    ManagerName = x.Admin.User.Username,
                }).ToList()
            };
        }
        public async Task<RawMaterialsResponseModel> GetAllApprovedRawMaterialAsync()
        {
            var rawMaterial = await _rawMaterialRepository.GetAllApprovedRawMaterialsAsync();
            if (rawMaterial.Count == 0)
            {
                return new RawMaterialsResponseModel
                {
                    Message = $"No Unapproved raw material",
                    Success = false
                };
            }
            foreach (var item in rawMaterial)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }
            return new RawMaterialsResponseModel
            {
                Message = "Production Cost found",
                Success = true,
                Data = rawMaterial.Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    QuantiityBought = x.QuantiityBought,
                    QuantiityRemaining = x.QuantiityRemaining,
                    Name = x.Name,
                    AdditionalMessage = x.AdditionalMessage,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ManagerImage = x.Admin.User.ProfileImage,
                    ManagerName = x.Admin.User.Username,
                }).ToList()
            };
        }
        public async Task<RawMaterialsResponseModel> GetAllAprovedRawMaterialsForTheMonthAsync(int month, int year)
        {
            var rawMaterial = await _rawMaterialRepository.GetAllAprovedRawMaterialForTheMonthAsync(month, year);
            if (rawMaterial.Count == 0)
            {
                return new RawMaterialsResponseModel
                {
                    Message = $"No approved raw materials for the year {year}",
                    Success = false
                };
            }
            foreach (var item in rawMaterial)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }
            return new RawMaterialsResponseModel
            {
                Message = "Production Cost found",
                Success = true,
                Data = rawMaterial.Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    QuantiityBought = x.QuantiityBought,
                    QuantiityRemaining = x.QuantiityRemaining,
                    Name = x.Name,
                    AdditionalMessage = x.AdditionalMessage,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ManagerImage = x.Admin.User.ProfileImage,
                    ManagerName = x.Admin.User.Username,
                }).ToList()
            };
        }
        public async Task<RawMaterialsResponseModel> GetAllRejectedRawMaterialForTheMonthAsync(int month)
        {
            var rawMaterial = await _rawMaterialRepository.GetAllRejectedRawMaterialForTheMonthAsync(month);
            if (rawMaterial.Count == 0)
            {
                return new RawMaterialsResponseModel
                {
                    Message = $"No Unapproved raw materials for the month {month}",
                    Success = false
                };
            }

            foreach (var item in rawMaterial)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }
            return new RawMaterialsResponseModel
            {
                Message = "Production Cost found",
                Success = true,
                Data = rawMaterial.Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    QuantiityBought = x.QuantiityBought,
                    QuantiityRemaining = x.QuantiityRemaining,
                    Name = x.Name,
                    AdditionalMessage = x.AdditionalMessage,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ManagerImage = x.Admin.User.ProfileImage,
                    ManagerName = x.Admin.User.Username,

                }).ToList()
            };
        }
        public async Task<RawMaterialsResponseModel> GetAllPendingRawMaterial()
        {
            var rawMaterials = await _rawMaterialRepository.GetAllPendingRawMaterialsAsync();
            if (rawMaterials.Count == 0)
            {
                return new RawMaterialsResponseModel
                {
                    Message = "No Pending raw materials for the year ",
                    Success = false
                };
            }

            foreach (var item in rawMaterials)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }

            return new RawMaterialsResponseModel
            {
                Message = "Pending raw material found",
                Success = true,
                Data = rawMaterials.Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    QuantiityBought = x.QuantiityBought,
                    QuantiityRemaining = x.QuantiityRemaining,
                    Name = x.Name,
                    AdditionalMessage = x.AdditionalMessage,
                    PostedTime = x.RequestTime,
                    EnumApprovalStatus = x.ApprovalStatus,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ManagerImage = x.Admin.User.ProfileImage,
                    ManagerName = x.Admin.User.Username,
                }).ToList()
            };
        }
        public async Task<RawMaterialsResponseModel> GetAllRawMaterials()
        {
            var rawMaterials = await _rawMaterialRepository.GetAllRawMaterialAsync();
            if (rawMaterials.Count == 0)
            {
                return new RawMaterialsResponseModel
                {
                    Message = "No raw materials found",
                    Success = false
                };
            }

            foreach (var item in rawMaterials)
            {
                if ((DateTime.Now - item.CreatedOn).TotalSeconds < 60)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalSeconds + " " + "Sec ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalSeconds > 60 && (DateTime.Now - item.CreatedOn).TotalHours < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalMinutes + " " + "Mins ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalMinutes > 60 && (DateTime.Now - item.CreatedOn).TotalDays < 1)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalHours + " " + "Hours ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalHours > 24 && (DateTime.Now - item.CreatedOn).TotalDays < 30)
                {
                    item.RequestTime = (int)(DateTime.Now - item.CreatedOn).TotalDays + " " + "Days ago";
                }
                if ((DateTime.Now - item.CreatedOn).TotalDays > 30 && (DateTime.Now - item.CreatedOn).TotalDays <= 365)
                {
                    item.RequestTime = ((int)(DateTime.Now - item.CreatedOn).TotalDays / 30) + " " + "Months ago";
                }
            }

            return new RawMaterialsResponseModel
            {
                Message = "All requested raw materials found",
                Success = true,
                Data = rawMaterials.Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Cost = x.Cost,
                    QuantiityBought = x.QuantiityBought,
                    QuantiityRemaining = x.QuantiityRemaining,
                    Name = x.Name,
                    AdditionalMessage = x.AdditionalMessage,
                    PostedTime = x.RequestTime,
                    StringApprovalStatus = x.ApprovalStatus.ToString(),
                    EnumApprovalStatus = x.ApprovalStatus,
                    CreatedTime = x.CreatedOn.ToLongDateString(),
                    ManagerImage = x.Admin.User.ProfileImage,
                    ManagerName = x.Admin.User.Username,
                    // ManagerRole = x.Admin.User.
                }).ToList()
            };
        }

        public async Task<RawMaterialResponseModel> GetRawAsync(int id)
        {
            var raw = await _rawMaterialRepository.GetAsync(id);
            if (raw == null)
            {
                return new RawMaterialResponseModel
                {
                    Message = "Raw Material not found",
                    Success = false
                };
            }
            return new RawMaterialResponseModel
            {
                Message = "Raw Material found successfully",
                Success = true,
                Data = new RawMaterialDto
                {
                    Id = raw.Id,
                    Cost = raw.Cost,
                    QuantiityBought = raw.QuantiityBought,
                    QuantiityRemaining = raw.QuantiityRemaining,
                    Name = raw.Name,
                    AdditionalMessage = raw.AdditionalMessage,
                    PostedTime = raw.RequestTime,
                    EnumApprovalStatus = raw.ApprovalStatus
                    
                }
            };
        }

        public async Task<RawMaterialResponseModel> CalculateRawMaterialCostForTheMonth()
        {
            var cost = await _rawMaterialRepository.GetSumOfAprovedRawMaterialForTheMonthAsync();
            if (cost == 0m)
            {
                return new RawMaterialResponseModel
                {
                    Message = "No cost for raw material for this month",
                    Success = false
                };
            }
            return new RawMaterialResponseModel
            {
                Data = new RawMaterialDto
                {
                    Cost = cost
                }
            };
        }
        public async Task<RawMaterialResponseModel> CalculateRawMaterialCostForThYear()
        {
            var cost = await _rawMaterialRepository.GetSumOfAprovedRawMaterialForTheYearAsync();
            if (cost == 0m)
            {
                return new RawMaterialResponseModel
                {
                    Message = "No cost for raw material for this month",
                    Success = false
                };
            }
            return new RawMaterialResponseModel
            {
                Message = "Sucessfully calculated",
                Success = true,
                Data = new RawMaterialDto
                {
                    Cost = cost
                }
            };
        }
        public async Task<BaseResponse> ApproveRawMaterialAsync(int id)
        {
            var rawMaterial = await _rawMaterialRepository.GetAsync(x => x.Id == id && x.IsDeleted == false);
            if (rawMaterial == null)
            {
                return new BaseResponse
                {
                    Message = "Raw Material request not found",
                    Success = false
                };
            }
            rawMaterial.ApprovalStatus = ApprovalStatus.Approved;
            await _rawMaterialRepository.UpdateAsync(rawMaterial);
            return new BaseResponse
            {
                Message = "Raw Material approved Successfully",
                Success = true
            };
        }
        public async Task<BaseResponse> RejectRawMaterialAsync(int id,RejectRequestRequestModel model)
        {
            var rawMaterial = await _rawMaterialRepository.GetAsync(x => x.Id == id && x.IsDeleted == false);
            if (rawMaterial == null)
            {
                return new BaseResponse
                {
                    Message = "Raw Material not found",
                    Success = false
                };
            }
            if (rawMaterial.ApprovalStatus == ApprovalStatus.Approved)
            {
                return new BaseResponse
                {
                    Message = "This request has been approved already",
                    Success = false
                };
            }
            rawMaterial.ApprovalStatus = ApprovalStatus.Rejected;
            rawMaterial.AdditionalMessage = model.Message;
            await _rawMaterialRepository.UpdateAsync(rawMaterial);
            return new BaseResponse
            {
                Message = "Raw Material rejected Successfully",
                Success = true
            };
        }
        public async Task<BaseResponse> UpdateRawMaterialRequestAsync(int id, UpdateRawMaterialRequestModel model)
        {
            var rawMaterial = await _rawMaterialRepository.GetAsync(x => x.Id == id && x.IsDeleted == false);
            if (rawMaterial.ApprovalStatus == ApprovalStatus.Approved)
            {
                return new BaseResponse
                {
                    Message = "This request has been approved and can't be updated",
                    Success = false
                };
            }
            rawMaterial.Cost = model.Cost;
            rawMaterial.Name = model.Name ?? rawMaterial.Name;
            rawMaterial.QuantiityRemaining += (model.QuantiityBought - rawMaterial.QuantiityBought);
            rawMaterial.QuantiityBought = model.QuantiityBought;
            rawMaterial.AdditionalMessage = model.AdditionalMessage ?? rawMaterial.AdditionalMessage;
            rawMaterial.ApprovalStatus = ApprovalStatus.Pending;
            await _rawMaterialRepository.UpdateAsync(rawMaterial);
            return new BaseResponse
            {
                Message = "Successfully Updated",
                Success = true
            };
        }

        public async Task<BaseResponse> DeleteRawMaterialRequestAsync(int id)
        {
            var rawMaterialRequest = await _rawMaterialRepository.GetAsync(id);
            
            if (rawMaterialRequest == null)
            {
                return new BaseResponse
                {
                    Message = "Request not found",
                    Success = false
                };
            }
            if (rawMaterialRequest.ApprovalStatus == ApprovalStatus.Approved)
            {
                return new BaseResponse
                {
                    Message = "This request has been approved and cannot be deleted",
                    Success = false
                };
            }
            rawMaterialRequest.IsDeleted = true;
            await _rawMaterialRepository.UpdateAsync(rawMaterialRequest);
            return new BaseResponse
            {
                Message = "Request Successfuly Deleted",
                Success = true
            };
        }
    }
}