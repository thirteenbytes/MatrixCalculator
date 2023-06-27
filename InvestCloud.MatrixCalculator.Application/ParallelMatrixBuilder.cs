using InvestCloud.MatrixCalculator.Application.Abstractions;
using MatrixCalculator.Domain;

namespace InvestCloud.MatrixCalculator.Application;

internal class ParallelMatrixBuilder : IMatrixBuilder<ParallelMatrixCalculator>
{
    private readonly IInvestCloudClient client;

    public ParallelMatrixBuilder(IInvestCloudClient client) =>
        this.client = client;

    public Task<ParallelMatrixCalculator> GetMatrix(int size, string dataset)
    {
        var matrix = new ParallelMatrixCalculator(size);

        Parallel.For(0, size, i =>
        {
            var getResponse = client.GetRow(dataset, i);
            if (getResponse.Result.Success)
            {
                var values = getResponse.Result.Value.ToArray<int>();
                matrix.AddRow(i, values);
            }
            else
            {
                //throw new NumbersClientException($"Get Row endpoint failed: {getResponse.Result.Cause}");
            }

        });

        return Task.FromResult(matrix);
    }
}
