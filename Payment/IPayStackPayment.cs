using System.Threading.Tasks;
using Dansnom.Dtos;
using Dtos.RequestModels;

namespace Dansnom.Payment
{
    public interface IPayStackPayment
    {
        Task<string> InitiatePayment(CreatePaymentRequestModel model, int userId, int orderId);
        Task<string> GetTransactionRecieptAsync(string transactionReference);
    }
}