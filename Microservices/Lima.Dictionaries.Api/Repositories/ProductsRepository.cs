using Lima.Dictionaries.Api.DatabaseContext;
using Lima.Dictionaries.Api.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DictionaryDbContext dictionaryDbContext;

        public ProductsRepository(DictionaryDbContext dictionaryDbContext)
        {
            this.dictionaryDbContext = dictionaryDbContext;
        }

        public async Task<int> Add(ProductDomain product)
        {
            var entity = new ProductDomain()
            {
                Title = product.Title,
                Manufacturer = product.Manufacturer,
                Country = product.Country,
                Price = product.Price,
                Quantity = product.Quantity,
                UnitOfMeasurements = product.UnitOfMeasurements,
                PhotoLink = product.PhotoLink
            };
            await dictionaryDbContext.ProductDomains.AddAsync(entity);
            await dictionaryDbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task Delete(int id)
        {
            var entity = await dictionaryDbContext.ProductDomains.FindAsync(id);
            if (entity != null)
            {
                System.IO.File.Delete(entity.PhotoLink);
                dictionaryDbContext.ProductDomains.Remove(entity);
                await dictionaryDbContext.SaveChangesAsync();
            }
        }

        public IEnumerable<ProductDomain> GetAllProducts(ProductDomain product)
        {
            var entities = dictionaryDbContext.ProductDomains.AsQueryable();
            if (product != null)
            {
                if (product.Id > 0)
                {
                    entities = entities.Where(w => w.Id == product.Id);
                }
                if (!string.IsNullOrEmpty(product.Title?.Trim()))
                {
                    string fullName = product.Title.ToLower().Trim();
                    entities = entities.Where(w => w.Title.ToLower().Contains(fullName));
                }
                if (!string.IsNullOrEmpty(product.Manufacturer?.Trim()))
                {
                    string attachedTo = product.Manufacturer.ToLower().Trim();
                    entities = entities.Where(w => w.Manufacturer.ToLower().Contains(attachedTo));
                }
                if (!string.IsNullOrEmpty(product.Country?.Trim()))
                {
                    string jobTitle = product.Country.ToLower().Trim();
                    entities = entities.Where(w => w.Country.ToLower().Contains(jobTitle));
                }
                if (!string.IsNullOrEmpty(product.UnitOfMeasurements?.Trim()))
                {
                    string status = product.UnitOfMeasurements.ToLower().Trim();
                    entities = entities.Where(w => w.UnitOfMeasurements.ToLower().Contains(status));
                }
                if (!string.IsNullOrEmpty(product.Price.ToString()?.Trim()))
                {
                    string status = product.Price.ToString().ToLower().Trim();
                    entities = entities.Where(w => w.Price.ToString().ToLower().Contains(status));
                }
                if (!string.IsNullOrEmpty(product.Quantity.ToString()?.Trim()))
                {
                    string status = product.Quantity.ToString().ToLower().Trim();
                    entities = entities.Where(w => w.Quantity.ToString().ToLower().Contains(status));
                }
            }
            return entities.ToList();
        }

        public async Task<ProductDomain> GetById(int id)
        {
            return await dictionaryDbContext.ProductDomains.FindAsync(id);
        }

        public async Task<int> Update(ProductDomain product)
        {
            var dbProduct = await dictionaryDbContext.ProductDomains.FindAsync(product.Id);
            if (dbProduct != null)
            {
                dbProduct.Title = product.Title;
                dbProduct.Manufacturer = product.Manufacturer;
                dbProduct.Country = product.Country;
                dbProduct.Price = product.Price;
                dbProduct.Quantity = product.Quantity;
                dbProduct.UnitOfMeasurements = product.UnitOfMeasurements;
                if (product.PhotoLink != null)
                {
                    System.IO.File.Delete(dbProduct.PhotoLink);
                    dbProduct.PhotoLink = product.PhotoLink;
                }
            }
            await dictionaryDbContext.SaveChangesAsync();
            return dbProduct.Id;
        }
    }
}
