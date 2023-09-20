using Client;
using Common.Logging;
using Common.POCOs;
using System.Net;

internal class Program
{
    private static readonly APIConnection Connection = new("https://localhost:7238/api/");

    private static async Task Main(string[] args)
    {
        LogWriter.OutputLevel = Common.Logging.LogLevel.DEBUG;


        APIRequestErrorHandler LoginOn401 = new APIRequestErrorHandler().AddCondition(HttpStatusCode.Unauthorized, async () => await Connection.Login());


        var response = (APIResponse<WeatherForecast[]>)await APIConnection.ExecuteWithAutomaticErrorHandler(async () => await Connection.Get<WeatherForecast[]>("WeatherForecast/GetWeatherForecast"), LoginOn401);
        
        if (response != null && response.IsValid)
        {
            foreach (WeatherForecast forecast in response.Result!)
            {
                LogWriter.LogInfo(forecast.ToString());
            }
        }


        Thread.Sleep(1000);
    }

}