namespace LeadChina.PM.IdentityServer.Interfaces.Services
{
    public interface IPasswordService
    {
        bool ValidatePassword(string password);
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}
