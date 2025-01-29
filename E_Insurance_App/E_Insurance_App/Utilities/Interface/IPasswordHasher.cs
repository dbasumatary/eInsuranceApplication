namespace E_Insurance_App.Utilities.Interface
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string inputPassword, string hashedPassword);
    }
}
