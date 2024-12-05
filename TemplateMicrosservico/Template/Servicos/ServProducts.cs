using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Template.Infra;
using static MyProject.DataContext;

namespace MyProject
{
    public interface IServProducts
    {
        Task<IEnumerable<ProductsDTO>> GetAllProductsAsync();
        Task<ProductsDTO> GetProductByIdAsync(int id);
        Task<ProductsDTO> CreateProductAsync(ProductsDTO productDto); 
        Task<bool> DeleteProductAsync(int id); 
        Task<ProductsDTO> UpdateProductAsync(int id, ProductsDTO productDto); 
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
            Console.WriteLine("aa");
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
            // Busca o produto no banco de dados
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return null; // Produto não encontrado

            // Verifica se o nome foi alterado e faz a requisição ao serviço externo
            if (!string.IsNullOrWhiteSpace(productDto.Name) && product.Name != productDto.Name)
            {
                try
                {
                    // Configurando o HttpClient (instância deve ser injetada no construtor da classe)
                    var httpClient = new HttpClient();

                    // Corpo da requisição com o novo nome
                    var patchContent = new StringContent(
                        JsonSerializer.Serialize(new { newName = productDto.Name }),
                        Encoding.UTF8,
                        "application/json"
                    );

                    // URL do serviço externo para atualizar o nome (ajuste conforme necessário)
                    var externalServiceUrl = $"http://localhost:5001/api/Stock/{id}/update-name";

                    // Fazendo a requisição PATCH ao serviço externo
                    var response = await httpClient.PatchAsync(externalServiceUrl, patchContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Erro ao atualizar nome no serviço externo: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro na atualização externa: {ex.Message}");
                }
            }

            // Atualizando os campos do produto localmente
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;

            // Salvando as mudanças no banco de dados
            await _context.SaveChangesAsync();

            // Retornando o produto atualizado como DTO
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

