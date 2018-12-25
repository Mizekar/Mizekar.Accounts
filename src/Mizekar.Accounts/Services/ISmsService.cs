using System.Threading.Tasks;

namespace Mizekar.Accounts.Services
{
    public interface ISmsService
    {
        Task<bool> SendAsync(string phoneNumber, string body);
    }
}
