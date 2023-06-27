namespace InvestCloud.MatrixCalculator.Application.Abstractions;

public interface IInvestCloudService
{
    Task Run(int sizeOfMatrix);
}
