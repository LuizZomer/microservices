using Microsoft.EntityFrameworkCore;
using Template.Infra;
using static MyProject.DataContext;

namespace MyProject
{
    public interface IServProducts
    {
        Task<IEnumerable<ProductsDTO>> GetAllProductsAsync();
        Task<ProductsDTO> GetProductByIdAsync(int id);
        Task<ProductsDTO> CreateProductAsync(ProductsDTO productDto); // Novo método
        Task<bool> DeleteProductAsync(int id); // Novo método
        Task<ProductsDTO> UpdateProductAsync(int id, ProductsDTO productDto); // Novo método

    }

    public class ServProducts : IServProducts
    {
        private readonly DataContext _context;

        public ServProducts(DataContext context)
        {
            _context = context;
        }

        // Método para obter todos os produtos
        public async Task<IEnumerable<ProductsDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            // Convertendo a lista de produtos para DTOs
            var productsDto = products.Select(p => new ProductsDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description
            });

            return productsDto;
        }

        // Implementação do método GetProductByIdAsync
        public async Task<ProductsDTO> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return null;

            return new ProductsDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
        }

        // Método CreateProductAsync já mostrado
        public async Task<ProductsDTO> CreateProductAsync(ProductsDTO productDto)
        {
            var productEntity = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description
            };

            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();

            return new ProductsDTO
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Price = productEntity.Price,
                Description = productEntity.Description
            };
        }

        // Implementação do DeleteProductAsync
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false; // Produto não encontrado

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true; // Produto excluído com sucesso
        }

        // Implementação do UpdateProductAsync
        public async Task<ProductsDTO> UpdateProductAsync(int id, ProductsDTO productDto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return null; // Produto não encontrado

            // Atualizando os campos do produto
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;

            // Salvando as mudanças no banco
            await _context.SaveChangesAsync();

            return new ProductsDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
        }
    }

}
