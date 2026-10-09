using RepositoryLayer.Entity;

namespace AuthenticationService.Interface
{
    public interface IJwtService
    {
        string GenerateToken(UserEntity user);
        string GenerateResetToken(UserEntity user);
        int ValidateResetToken(string token);
    }
}