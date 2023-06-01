namespace Dtos.RequestModels
{
    public class  CreatePaymentRequestModel
    {
        public int Amount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}