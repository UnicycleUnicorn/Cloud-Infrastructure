using Common.Logging;
using Common.POCOs;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Client;

/// <summary>
/// Provides the ability to connect and make requests to a remote or local web API using the following request types:
/// <list type="bullet">
/// <item>Get: Retrieve resource from the server</item>
/// <item>Post: Create new resource on the server</item>
/// <item>Put: Update existing resource on the server</item>
/// <item>Patch: Update part of existing resource on the server</item>
/// <item>Delete: Delete existing resource</item>
/// </list>
/// </summary>
internal class APIConnection
{
    private readonly string URL; // https://localhost:7238/api/
    private readonly HttpClient Client = new();
    private string? RefreshToken = null;

    public APIConnection(string baseUrl)
    {
        URL = baseUrl;
        Client.BaseAddress = new Uri(URL);
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    #region Get
    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url) => await Get<TResult>(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url) => await Get(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url, UrlParameters urlParameters) => await Get<TResult>(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url, UrlParameters urlParameters) => await Get(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url, UrlParameters urlParameters, CancellationToken cancellationToken) => await Get<TResult>(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url, UrlParameters urlParameters, CancellationToken cancellationToken) => await Get(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Get<TResult>(string url, CancellationToken cancellationToken)
    {
        LogWriter.LogInfo("Get Issued");

        HttpResponseMessage response = await Client.GetAsync(url, cancellationToken);

        return await GenerateResult<TResult>("Get", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous get request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Get(string url, CancellationToken cancellationToken)
    {
        LogWriter.LogInfo("Get Issued");

        HttpResponseMessage response = await Client.GetAsync(url, cancellationToken);

        return GenerateResult("Get", response);
    }
    #endregion

    #region Post
    //// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, TValue content) => await Post<TResult, TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, TValue content) => await Post<TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, UrlParameters urlParameters, TValue content) => await Post<TResult, TValue>(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, UrlParameters urlParameters, TValue content) => await Post(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) => await Post<TResult, TValue>(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) => await Post(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Post<TResult, TValue>(string url, TValue content, CancellationToken cancellationToken)
    {
        var payload = JsonContent.Create(content);

        LogWriter.LogInfo("Post Issued");

        HttpResponseMessage response = await Client.PostAsync(url, payload, cancellationToken);

        return await GenerateResult<TResult>("Post", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous post request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Post<TValue>(string url, TValue content, CancellationToken cancellationToken)
    {
        var payload = JsonContent.Create(content);

        LogWriter.LogInfo("Post Issued");

        HttpResponseMessage response = await Client.PostAsync(url, payload, cancellationToken);

        return GenerateResult("Post", response);
    }
    #endregion

    #region Put
    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, TValue content) => await Put<TResult, TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, TValue content) => await Put<TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, UrlParameters urlParameters, TValue content) => await Put<TResult, TValue>(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, UrlParameters urlParameters, TValue content) => await Put(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) => await Put<TResult, TValue>(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) => await Put(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Put<TResult, TValue>(string url, TValue content, CancellationToken cancellationToken)
    {
        var payload = JsonContent.Create(content);

        LogWriter.LogInfo("Put Issued");

        HttpResponseMessage response = await Client.PutAsync(url, payload, cancellationToken);

        return await GenerateResult<TResult>("Put", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous put request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Put<TValue>(string url, TValue content, CancellationToken cancellationToken)
    {
        var payload = JsonContent.Create(content);

        LogWriter.LogInfo("Put Issued");

        HttpResponseMessage response = await Client.PutAsync(url, payload, cancellationToken);

        return GenerateResult("Put", response);
    }
    #endregion

    #region Patch
    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, TValue content) => await Patch<TResult, TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, TValue content) => await Patch<TValue>(url, content, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, UrlParameters urlParameters, TValue content) => await Patch<TResult, TValue>(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, UrlParameters urlParameters, TValue content) => await Patch(url + urlParameters.ToUrlParameters(), content);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) => await Patch<TResult, TValue>(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, UrlParameters urlParameters, TValue content, CancellationToken cancellationToken) => await Patch(url + urlParameters.ToUrlParameters(), content, cancellationToken);

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Patch<TResult, TValue>(string url, TValue content, CancellationToken cancellationToken)
    {
        var payload = JsonContent.Create(content);

        LogWriter.LogInfo("Patch Issued");

        HttpResponseMessage response = await Client.PatchAsync(url, payload, cancellationToken);

        return await GenerateResult<TResult>("Patch", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous patch request to the specified url
    /// </summary>
    /// <typeparam name="TValue">Request body POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="content">POCO representing request body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Patch<TValue>(string url, TValue content, CancellationToken cancellationToken)
    {
        var payload = JsonContent.Create(content);

        LogWriter.LogInfo("Patch Issued");

        HttpResponseMessage response = await Client.PatchAsync(url, payload, cancellationToken);

        return GenerateResult("Patch", response);
    }
    #endregion

    #region Delete
    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url) => await Delete<TResult>(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url) => await Delete(url, CancellationToken.None);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url, UrlParameters urlParameters) => await Delete<TResult>(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url, UrlParameters urlParameters) => await Delete(url + urlParameters.ToUrlParameters());

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url, UrlParameters urlParameters, CancellationToken cancellationToken) => await Delete<TResult>(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="urlParameters">Url Parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url, UrlParameters urlParameters, CancellationToken cancellationToken) => await Delete(url + urlParameters.ToUrlParameters(), cancellationToken);

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <typeparam name="TResult">Expected response POCO</typeparam>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse{TResult}"/> representing the result from the request</returns>
    public async Task<APIResponse<TResult>> Delete<TResult>(string url, CancellationToken cancellationToken)
    {
        LogWriter.LogInfo("Delete Issued");

        HttpResponseMessage response = await Client.DeleteAsync(url, cancellationToken);

        return await GenerateResult<TResult>("Delete", response, cancellationToken);
    }

    /// <summary>
    /// Generates an asyncronous delete request to the specified url
    /// </summary>
    /// <param name="url">Url to send the request to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see cref="APIResponse"/> representing the result from the request</returns>
    public async Task<APIResponse> Delete(string url, CancellationToken cancellationToken)
    {
        LogWriter.LogInfo("Delete Issued");

        HttpResponseMessage response = await Client.DeleteAsync(url, cancellationToken);

        return GenerateResult("Delete", response);
    }
    #endregion

    #region Helpers
    private static async Task<APIResponse<TResult>> GenerateResult<TResult>(string requestType, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            LogWriter.LogInfo($"{requestType} succeded");
            return new APIResponse<TResult>(response, await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken));
        }
        else
        {
            LogWriter.LogWarn($"{requestType} failed: {(int)response.StatusCode} - {response.ReasonPhrase}");
            return new APIResponse<TResult>(response);
        }
    }

    private static APIResponse GenerateResult(string requestType, HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            LogWriter.LogInfo($"{requestType} succeded");
        }
        else
        {
            LogWriter.LogWarn($"{requestType} failed: {(int)response.StatusCode} - {response.ReasonPhrase}");
        }

        return new APIResponse(response);
    }

    public void SetAuthJWT(string token) => Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    #endregion

    #region Scripts
    public async Task Login()
    {
        if (RefreshToken == null) // Refresh token does not exist
        {
            LogWriter.LogDebug("Attempting to log in using username & password");
            APIResponse<DualToken> token = await Post<DualToken, UserIdentification>("User/Authorize", new("admin", "root"));
            if (token.IsValid) // Logged in with user credentials
            {
                SetAuthJWT(token.Result!.TokenA);
                RefreshToken = token.Result!.TokenB;
                LogWriter.LogDebug("Logged in successfully");
            }
            else if (token.StatusCode == System.Net.HttpStatusCode.Unauthorized)  // Incorrect credentials
            {
                LogWriter.LogError("Failed to log in: incorrect credentials");
            }
            else if (token.StatusCode == System.Net.HttpStatusCode.Forbidden) // Incorrect privileges
            {
                LogWriter.LogError("Failed to log in: incorrect privileges");
            }
            else  // Unknown
            {
                LogWriter.LogError("Failed to log in: unkown cause");
            }
        }
        else // Refresh token exists
        {
            LogWriter.LogDebug("Attempting to refresh authorization token");
            APIResponse<DualToken> refresh = await Post<DualToken, SingleToken>("User/Refresh", new SingleToken(RefreshToken));
            if (refresh.IsValid) // Refreshed token
            {
                SetAuthJWT(refresh.Result!.TokenA);
                RefreshToken = refresh.Result!.TokenB;
                LogWriter.LogDebug("Refreshed authorization token successfully");
            }
            else if (refresh.StatusCode == System.Net.HttpStatusCode.Unauthorized) // Old refresh token
            {
                RefreshToken = null;
                LogWriter.LogError("Refresh token no longer valid");
                await Login();
            }
            else if (refresh.StatusCode == System.Net.HttpStatusCode.Forbidden) // Incorrect privileges or user not in system
            {
                RefreshToken = null;
                LogWriter.LogError("Failed to refresh authorization token: incorrect privileges");
                await Login();
            }
            else // Unknown
            {
                LogWriter.LogError("Failed to refresh authorization token: unkown cause");
            }
        }
    }
    
    public async Task<APIResponse> ExecuteRequestWithAutomatic401Handling(Func<Task<APIResponse>> apiRequest)
    {
        var result = await apiRequest.Invoke();

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await Login();
            return await apiRequest.Invoke();
        }

        return result;
    }

    public async Task<APIResponse<TResult>> ExecuteRequestWithAutomatic401Handling<TResult>(Func<Task<APIResponse<TResult>>> apiRequest)
    {
        var result = await apiRequest.Invoke();

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await Login();
            return await apiRequest.Invoke();
        }

        return result;
    }

    public static async Task<APIResponse> ExecuteWithAutomaticErrorHandler(Func<Task<APIResponse>> apiRequest, APIRequestErrorHandler errorHandler)
    {
        var result = await apiRequest.Invoke();

        var action = errorHandler.Find(result);

        if (action == null) return result;

        await action.Invoke();

        return await apiRequest.Invoke();
    }
    #endregion
}