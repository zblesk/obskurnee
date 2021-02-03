using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public interface IMailerService
    {
        Task SendMail(string subject, string body, params string[] recipients);
    }
}
