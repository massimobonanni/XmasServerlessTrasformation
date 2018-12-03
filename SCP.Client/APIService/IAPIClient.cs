using System.Threading.Tasks;
using SCP.Core.DTO;

namespace SCP.Client.APIService
{
    public interface IApiClient
    {
        string ApiUrl { get; set; }
        string ApiKey { get; set; }
        Task<(ApiClientResult,ChildDto)> GetChildByIdAsync(string childId);

        Task<ApiClientResult> SubmitEvaluationAsync(string childId, string firstName, string lastName,int goodness);
    }

    public enum ApiClientResult
    {
        OK=0,
        NotFound=10,
        GenericError=999
    }
}