namespace Common.POCOs;
public class DualToken
{
    public string TokenA { get; set; } // Typically Authorization Token
    public string TokenB { get; set; } // Typically Refresh Token

    public DualToken(string tokenA, string tokenB)
    {
        TokenA = tokenA;
        TokenB = tokenB;
    }
}
