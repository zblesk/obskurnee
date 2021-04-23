using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public interface IMailerService
    {
        Task SendHtmlMail(string subject, string body, params string[] recipients);
    }
}
