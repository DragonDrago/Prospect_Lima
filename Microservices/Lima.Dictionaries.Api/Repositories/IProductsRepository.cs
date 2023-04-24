using Lima.Dictionaries.Api.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Lima.Dictionaries.Api.Repositories
{
    public interface IProductsRepository
    {
        Task<int> Add(ProductDomain product);
        Task<int> Update(ProductDomain product);
        Task<ProductDomain> GetById(int id);
        IEnumerable<ProductDomain> GetAllProducts(ProductDomain product);
        Task Delete(int id);
    }
}
