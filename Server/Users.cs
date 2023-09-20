using System.Diagnostics.CodeAnalysis;

namespace Server;

public static class Users
{
    private static readonly Dictionary<string, (string, string[])> users = new();

    public static bool AddUser(string username, string password, string[] roles)
    {
        if (users.ContainsKey(username)) return false;
        users[username] = (password, roles);
        return true;
    }

    static Users()
    {
        _ = AddUser("user", "pass", Array.Empty<string>());
        _ = AddUser("admin", "root", new string[] { "admin" });
    }

    public static bool GetRoles(string username, [NotNullWhen(true)] out string[]? roles)
    {
        if (users.ContainsKey(username))
        {
            (string, string[]) user = users[username];
            roles = user.Item2;
            return true;
        }

        roles = null;
        return false;
    }

    public static bool ValidateAndGetRoles(string username, string password, [NotNullWhen(true)] out string[]? roles)
    {

        if (users.ContainsKey(username))
        {
            (string, string[]) user = users[username];
            if (user.Item1 == password)
            {
                roles = user.Item2;
                return true;
            }
        }

        roles = null;
        return false;
    }
}
