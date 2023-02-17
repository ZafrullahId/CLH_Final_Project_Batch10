
using Dansnom.Dtos.RequestModel;

namespace DansnomEmailServices
{
    public interface IMailServices
    {
       public void SendEMailAsync(MailRequest mailRequest);
    }
}
