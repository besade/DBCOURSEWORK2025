namespace Shop.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(int userId, string email, bool isAdmin);
    }
}
