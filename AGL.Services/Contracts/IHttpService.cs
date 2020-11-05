using System;
using System.Threading.Tasks;

namespace AGL.Services.Contracts
{
    public interface IHttpService
    {
        Task<T> RetrieveJsonDataAsync<T>(string clientName);
    }
}
