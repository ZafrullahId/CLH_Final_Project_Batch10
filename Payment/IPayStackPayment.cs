using System.Threading.Tasks;
using Dtos.RequestModels;

namespace Dansnom.Payment
{
    public interface IPayStackPayment
    {
        Task<string> InitiatePayment(CreatePaymentRequestModel model, int userId, int orderId);
    }
}