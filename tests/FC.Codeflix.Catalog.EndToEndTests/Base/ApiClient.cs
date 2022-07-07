using FC.Codeflix.Catalog.Api.Configurations.Policies;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _defaultSerializerOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _defaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new JsonSnakeCasePolicy(),
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    )
        where TOutput : class
    {
        var response = await _httpClient.PostAsync(
            route, 
            new StringContent(
                JsonSerializer.Serialize(
                    payload,
                    _defaultSerializerOptions
                ),
                Encoding.UTF8,
                "application/json"
            )
        );
        var output = await ReadAndDeserializeOutput<TOutput>(response);
        return (response, output);
    }


    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route,
        object? queryStringParametersObject = null
    ) where TOutput : class
    {
        var url = PrepareGetRoute(route, queryStringParametersObject);
        var response = await _httpClient.GetAsync(url);
        var output = await ReadAndDeserializeOutput<TOutput>(response);
        return (response, output);
    }


    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(
        string route
    )
        where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(
            route
        );
        var output = await ReadAndDeserializeOutput<TOutput>(response);
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(
        string route,
        object payload
    )
        where TOutput : class
    {
        var response = await _httpClient.PutAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(
                    payload, 
                    _defaultSerializerOptions
                ),
                Encoding.UTF8,
                "application/json"
            )
        );
        var output =  await ReadAndDeserializeOutput<TOutput>(response);
        return (response, output);
    }

    private async Task<TOutput?> ReadAndDeserializeOutput<TOutput>(HttpResponseMessage response)
    {
        var outputString = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(outputString)) return default;
        var output = JsonSerializer.Deserialize<TOutput>(
            outputString,
            _defaultSerializerOptions
        );
        return output;
    }


    private string PrepareGetRoute(
        string route,
        object? queryStringParametersObject
    )
    {
        if (queryStringParametersObject is null)
            return route;
        var parametersJson = JsonSerializer.Serialize(queryStringParametersObject, _defaultSerializerOptions);
        var parametersDictionary = Newtonsoft.Json.JsonConvert
            .DeserializeObject<Dictionary<string, string>>(parametersJson);
        return QueryHelpers.AddQueryString(route, parametersDictionary!);
    }
}
