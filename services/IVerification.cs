using EvatVerificationApp.model;

namespace EvatVerificationApp.services
{
    public interface IVerification
    {
        Task<bool> GetVerifiedAsync(QueryDto queryData, CreateVerificationDto create);
        Task<bool> GetVerifiedAsync1_1(QueryDto_1_1 queryData, CreateVerificationDto create);

        Task<bool> GetNGVerifiedAsync1_1(NG_QueryDto_1_1 queryData, WHTVerificationDto create);
    }
}
