namespace E_Insurance_App.Utilities.Interface
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role);
    }
}
