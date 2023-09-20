namespace Client;

public class UrlParameters
{
    public List<(string key, string value)>? Parameters { get; private set; } = null;

    public static UrlParameters Empty => new();

    public UrlParameters() { }

    public UrlParameters(params (string key, string value)[] parameters)
    {
        Parameters = new();
        Parameters.AddRange(parameters);
    }

    public UrlParameters Add(params (string key, string value)[] parameters)
    {
        Parameters ??= new();

        Parameters.AddRange(parameters);

        return this;
    }

    public string ToUrlParameters()
    {
        string urlExtension = "";
        if (Parameters != null)
        {
            urlExtension = "?";
            foreach ((string key, string value) in Parameters)
            {
                urlExtension += $"{key}={value}&";
            }
            urlExtension = urlExtension.Remove(urlExtension.Length - 1);
        }

        return urlExtension;
    }
}
