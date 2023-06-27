using InvestCloud.MatrixCalculator.Application.Abstractions;
using InvestCloud.MatrixCalculator.Application.Extensions;
using InvestCloud.MatrixCalculator.Application.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace InvestCloud.MatrixCalculator.Application;

public class InvestCloudClient : IInvestCloudClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public InvestCloudClient(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task<ResultOfRowInt32> GetRow(string dataset, int idx)
    {
        var type = "row";
        var getPath = configuration
            .GetValue<string>("Endpoints:Get")
            .InterpolateConvert(new { dataset, type, idx });

        return await httpClient.GetFromJsonAsync<ResultOfRowInt32>(getPath);
    }

    public async Task<ResultOfInt32> Initialize(int size)
    {
        var initializePath = configuration
             .GetValue<string>("Endpoints:Initialize")
             .InterpolateConvert(new { size });

        return await httpClient.GetFromJsonAsync<ResultOfInt32>(initializePath);
    }

    public async Task<ResultOfString> Validate(string md5Hash)
    {
        var validatePath = configuration
                .GetValue<string>("Endpoints:Validate");

        var stringPayLoad = JsonConvert.SerializeObject(md5Hash);
        var requestContent = new StringContent(stringPayLoad, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(validatePath, requestContent);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ResultOfString>(responseContent);
    }
}
