using WebApplication4.Models;

namespace WebApplication4.Services.Interfaces
{
    public interface IApiServices
    {
        Task<TransferData> GetTransferData(string url);
    }
}
