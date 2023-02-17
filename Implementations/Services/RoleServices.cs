using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities.Identity;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;

namespace Dansnom.Implementations.Services
{

    public class RoleServices : IRoleServices
    {
        private readonly IRoleRepository _roleRepository;
        public RoleServices(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<BaseResponse> CreateRole(CreateRoleRequestModel model)
        {
            var role = await _roleRepository.GetAsync(r => r.Name == model.Name.ToLower());
            if (role != null)
            {
                return new BaseResponse()
                {
                    Message = "Role Already Exist",
                    Success = false,
                };
            }
            var newRole = new Role
            {
                Name = model.Name.ToLower(),
                Description = model.Description,
            };
            await _roleRepository.CreateAsync(newRole);
            return new BaseResponse
            {
                Message = "Role Created Successfully",
                Success = true,
            };
        }
        public async Task<RolesResponseModel> GetAllRoleAsync()
        {
            var role = await _roleRepository.GetAllAsync();
            if (role == null)
            {
                return new RolesResponseModel
                {
                    Message = "No Roles Found",
                    Success = false,
                };
            }
            return new RolesResponseModel
            {
                Data = role.Select(x => new RoleDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                }).ToList(),
                Message = "Roles Found Successfully",
                Success = true
            };
        }
        public async Task<RoleResponseModel> GetRoleByUserId(int id)
        {
            var role = await _roleRepository.GetRoleByUserId(id);
            if (role == null)
            {
                return new RoleResponseModel
                {
                    Message = "Role not found",
                    Success = false,
                };
            }
            return new RoleResponseModel
            {
                Message = "Role found",
                Success = true,
                Data = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                }
            };
        }
        public async Task<BaseResponse> DeleteRole(int id)
        {
            var role = await _roleRepository.GetAsync(id);
            if(role == null)
            {
                return new BaseResponse
                {
                    Message = "Role not found",
                    Success = false
                };
            }
            role.IsDeleted = true;
            await _roleRepository.UpdateAsync(role);
            return new BaseResponse
            {
                Message = "Role deleted successfully",
                Success = true
            };
        }
    }
}