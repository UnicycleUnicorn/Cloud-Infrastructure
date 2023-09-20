namespace Common.POCOs;
public class UserIdentification
{
    public UserIdentification(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}
