using E_Insurance_App.Utilities.Interface;

namespace E_Insurance_App.Utilities.Implementation
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            //return BCrypt.HashPassword(password, BCrypt.GenerateSalt(12));
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while hashing password.", ex);
            }
        }

        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            //return BCrypt.Verify(inputPassword, hashedPassword);
            try
            {
                return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while verifying password.", ex);
            }
        }
    }
}

