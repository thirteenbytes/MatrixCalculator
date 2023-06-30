using InvestCloud.MatrixCalculator.Application.Abstractions;
using InvestCloud.MatrixCalculator.Application.Extensions;
using MatrixCalculator.Domain;
using Serilog;

namespace InvestCloud.MatrixCalculator.Application;

public class InvestCloudService : IInvestCloudService
{
    private readonly ILogger logger;
    private readonly IInvestCloudClient client;

    public InvestCloudService(ILogger logger, IInvestCloudClient client)
    {
        this.logger = logger;
        this.client = client;
    }

    public async Task Run(int sizeOfMatrix)
    {
        try
        {
            logger.Information($"Step 1: Initializing and build the square matrices ({sizeOfMatrix} x {sizeOfMatrix})");
            (ParallelMatrix matrixA, ParallelMatrix matrixB) = await GetMatrices(sizeOfMatrix);

            logger.Information($"Step 2: Multiple the two matrices");
            var matrixResult = matrixA * matrixB;

            var a = matrixResult.ToString();
            logger.Information($"Step 3: Create the MD5 Hash for validation.");
            var md5Validation = matrixResult
                .ToString()
                .ToMD5();

            logger.Information($"Step 4: Validating the MD5 Hash {md5Validation}");

            var passPhrase = await client.Validate(md5Validation);

            if (!passPhrase.Success)
            {
                throw new Exception($"Validation error occurred: {passPhrase.Cause}");
            }

            logger.Information($"Step 5: Passphrase: {passPhrase.Value}");
        }
        catch (Exception ex)
        {
            logger.Error($"An error occurred while run {ex.Message}");
        }

    }

    private async Task<(ParallelMatrix, ParallelMatrix)> GetMatrices(int size)
    {
        var initCallResponse = await client.Initialize(size);
        if (initCallResponse.Success)
        {
            var matrixBuilder = new ParallelMatrixBuilder(client);
            var matrixA = await matrixBuilder.GetMatrix(size, "A");
            var matrixB = await matrixBuilder.GetMatrix(size, "B");

            return (matrixA, matrixB);
        }

        throw new Exception(initCallResponse.Cause);
    }
}