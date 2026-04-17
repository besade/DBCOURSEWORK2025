using Shop.Domain.Models;

namespace Shop.Domain.Interfaces.IFactories
{
    public interface ICategoryFactory
    {
        Task<Category> CreateAsync(string name);
    }
}
