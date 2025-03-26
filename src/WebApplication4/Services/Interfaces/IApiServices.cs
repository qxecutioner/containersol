using WebApplication4.Models;

namespace WebApplication4.Services.Interfaces
{
    public interface IApiServices<T>
    {
        Task<T> GetTransferData(string url);
    }
}
