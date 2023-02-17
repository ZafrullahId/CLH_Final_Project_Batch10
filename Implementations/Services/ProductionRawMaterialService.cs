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
    public class ProductionRawMaterialService
    {
        private readonly IProductionRawMaterialRepository _productionRawMaterialRepository;
        public ProductionRawMaterialService(IProductionRawMaterialRepository productionRawMaterialRepository)
        {
            _productionRawMaterialRepository = productionRawMaterialRepository;
        }
    }
}