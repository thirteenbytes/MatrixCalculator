namespace InvestCloud.MatrixCalculator.Application.Abstractions;

internal interface IMatrixBuilder<T> where T : class
{
    Task<T> GetMatrix(int size, string dataset);
}
