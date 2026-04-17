using Shop.Domain.Models;

namespace Shop.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(Customer user);
    }
}
