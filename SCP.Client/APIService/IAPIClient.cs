using System.Threading.Tasks;
using SCP.Core.DTO;

namespace SCP.Client.APIService
{
    public interface IApiClient
    {
        string ApiUrl { get; set; }
        string ApiKey { get; set; }
        Task<ChildDto> GetChildByIdAsync(string childId);
    }
}