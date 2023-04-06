using System.Linq;
using System.Threading.Tasks;
using Dansnom.Dtos;
using Dansnom.Dtos.RequestModel;
using Dansnom.Dtos.ResponseModel;
using Dansnom.Entities;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;

namespace Dansnom.Implementations.Services
{

    public class ProductionRawMaterialService : IProductionRawMaterialService
    {
        private readonly IProductionRawMaterialRepository _productionRawMaterialRepository;
        private readonly IRawMaterialRepository _rawMaterialRepository;
        public ProductionRawMaterialService(IProductionRawMaterialRepository productionRawMaterialRepository, IRawMaterialRepository rawMaterialRepository)
        {
            _productionRawMaterialRepository = productionRawMaterialRepository;
            _rawMaterialRepository = rawMaterialRepository;
        }
        public async Task<ProductionsResponseModel> GetProductionByRwamaterialIdAsync(int id)
        {
            var rawMaterial = await _rawMaterialRepository.GetAsync(id);
            if (rawMaterial == null)
            {
                return new ProductionsResponseModel
                {
                    Message = "Something went wrong",
                    Success = false
                };
            }
            var productions = await _productionRawMaterialRepository.GetProductionsByRawMaterialIdAsync(id);
            return new ProductionsResponseModel
            {
                Data = productions.Select(x => new ProductionDto
                {
                    ProductDto = new ProductDto
                    {
                        Name = x.Production.Product.Name,
                        Price = x.Production.Product.Price,
                        ImageUrl = x.Production.Product.ImageUrl
                    },
                    ProductionDate = x.Production.LastModifiedOn.ToLongDateString(),
                    QuantityRequest = x.Production.QuantityRequest,
                    QuantityProduced = x.Production.QuantityProduced,
                    QuantityRemaining = x.Production.QuantityRemaining,

                }).ToList()
            };
        }
    }
}