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
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IConfiguration configuration;
    private readonly HttpClient httpClient;

    public InvestCloudClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClientFactory = httpClientFactory;
        this.httpClient = httpClientFactory.CreateClient("InvestCloudClient");
        this.configuration = configuration;
    }


    public async Task<ResultOfRowInt32> GetRow(string dataset, int idx)
    {
        var type = "row";
        var path = configuration
            .GetValue<string>("Endpoints:Get")
            .InterpolateConvert(new { dataset, type, idx });

        var response = await httpClient.GetAsync(path);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.StatusCode.ToString());
        }

        return await response.Content.ReadFromJsonAsync<ResultOfRowInt32>();

    }

    public async Task<ResultOfInt32> Initialize(int size)
    {
        var path = configuration
             .GetValue<string>("Endpoints:Initialize")
             .InterpolateConvert(new { size });

        var response = await httpClient.GetAsync(path);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.StatusCode.ToString());
        }

        return await response.Content.ReadFromJsonAsync<ResultOfInt32>();
    }

    public async Task<ResultOfString> Validate(string md5Hash)
    {
        var validatePath = configuration
                .GetValue<string>("Endpoints:Validate");

        var stringPayLoad = JsonConvert.SerializeObject(md5Hash);
        var requestContent = new StringContent(stringPayLoad, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(validatePath, requestContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.StatusCode.ToString());
        }

        return await response.Content?.ReadFromJsonAsync<ResultOfString>();
    }
}
