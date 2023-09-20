using System.Net;

namespace Client;

public class APIResponse<TResult> : APIResponse
{
    public TResult? Result { private set; get; }

    public bool IsValid => WasSuccessful && Result != null;

    public APIResponse(HttpResponseMessage httpResponse, TResult? result) : base(httpResponse) => Result = result;

    public APIResponse(HttpResponseMessage httpResponse) : base(httpResponse) => Result = default;
}

public class APIResponse
{
    public HttpResponseMessage HttpResponse { private set; get; }

    public HttpStatusCode StatusCode => HttpResponse.StatusCode;

    public bool WasSuccessful => HttpResponse.IsSuccessStatusCode;

    public APIResponse(HttpResponseMessage httpResponse) => HttpResponse = httpResponse;
}