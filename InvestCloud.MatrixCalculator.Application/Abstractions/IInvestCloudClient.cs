using InvestCloud.MatrixCalculator.Application.Models;

namespace InvestCloud.MatrixCalculator.Application.Abstractions;

public interface IInvestCloudClient
{
    Task<ResultOfInt32> Initialize(int size);
    Task<ResultOfRowInt32> GetRow(string dataset, int idx);
    Task<ResultOfString> Validate(string md5Hash);
}
