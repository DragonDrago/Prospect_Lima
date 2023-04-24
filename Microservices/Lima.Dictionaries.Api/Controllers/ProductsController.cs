using Lima.Dictionaries.Api.Domain;
using Lima.Dictionaries.Api.FileService;
using Lima.Dictionaries.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lima.Dictionaries.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository productsRepository;
        private readonly IFileUploadService fileUploadService;
        private readonly IWebHostEnvironment environment;

        public ProductsController(IProductsRepository productsRepository, IFileUploadService fileUploadService, IWebHostEnvironment environment)
        {
            this.productsRepository = productsRepository;
            this.fileUploadService = fileUploadService;
            this.environment = environment;
        }

        [HttpGet("all")]
        [Authorize(Policy = "PRODUCTS_VIEW")]
        public IActionResult All([FromQuery] ProductDomain productDomain)
        {
            return Ok(productsRepository.GetAllProducts(productDomain));
        }

        [HttpPost("add")]
        [Authorize(Policy = "PRODUCT_ADD")]
        public async Task<IActionResult> Add([FromBody] ProductDomain productDomain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = new ProductDomain()
            {
                Title = productDomain.Title,
                Manufacturer = productDomain.Manufacturer,
                Country = productDomain.Country,
                Price = productDomain.Price,
                Quantity = productDomain.Quantity,
                UnitOfMeasurements = productDomain.UnitOfMeasurements,
                PhotoLink = fileUploadService.FileUploader(productDomain.Photo, environment)
            };
            return Ok(await productsRepository.Add(entity));
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "PRODUCT_EDIT")]
        public async Task<IActionResult> Edit(int id, [FromBody] ProductDomain productDomain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != productDomain.Id)
            {
                return BadRequest(ModelState);
            }

            var entity = new ProductDomain()
            {
                Id = productDomain.Id,
                Title = productDomain.Title,
                Manufacturer = productDomain.Manufacturer,
                Country = productDomain.Country,
                UnitOfMeasurements = productDomain.UnitOfMeasurements,
                PhotoLink = productDomain.Photo != null ? fileUploadService.FileUploader(productDomain.Photo, environment) : null
            };
            return Ok(await productsRepository.Update(entity));
        }

        [HttpGet("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var entity = productsRepository.GetById(id);
            if (entity != null)
            {
                await productsRepository.Delete(id);
            }
            return Ok(entity);
        }
    }
}
