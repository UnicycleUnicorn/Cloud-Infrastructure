using Common.Logging;

namespace Server.Security;

public class NaiveTokenStore : AbstractTokenStore
{
    public readonly Dictionary<string, (string username, DateTime expiration)> TokenStorage = new();

    public NaiveTokenStore(TimeSpan defaultAuthoriationExpiration, TimeSpan defaultRefreshExpiration, TimeSpan clockSkew) : base(defaultAuthoriationExpiration, defaultRefreshExpiration, clockSkew) {}

    public override (string token, string username, DateTime expiration)? RemoveRefreshToken(string token)
    {
        if (TokenStorage.Remove(token, out (string username, DateTime expiration) value))
        {
            LogWriter.LogInfo("Removing old refresh token");
            return (token, value.username, value.expiration);
        }

        return null;
    }

    public override bool StoreRefreshToken(string token, string username, DateTime expiration)
    {
        LogWriter.LogInfo("Storing new refresh token");
        return TokenStorage.TryAdd(token, (username, expiration));
    }
}
