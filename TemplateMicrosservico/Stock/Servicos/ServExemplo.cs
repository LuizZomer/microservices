using System.Net.Http;
using System.Text.Json;
using Exemplo;
using Microsoft.EntityFrameworkCore;
using Template.Infra;
using static Exemplo.DataContext;
using Stock.Models;
using System.Text;
using exemplo;

namespace Exemplo
{
    public interface IServExemplo
    {
        Task AdicionarStockAsync(StockItem stockItem);
        Task<IEnumerable<StockItem>> GetAllStockAsync();
        Task<bool> DeleteStockAsync(int id);
        Task<bool> UpdateStockQuantityAsync(int id, int newQuantity);
        Task<bool> UpdateProductNameAsync(int stockId, string newName);
        Task<bool> UpdateStockProductNameAsync(int id, string newName);
        Task<StockItem> GetStockByProductIdAsync(int productId);
        Task<ProductDTO> GetProductDetailsFromApiAsync(int productId);

    }


    public class ServExemplo : IServExemplo
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;

        public ServExemplo(DataContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task AdicionarStockAsync(StockItem stockItem)
        {
            try
            {
                var productId = stockItem.ProductId;
                var response = await _httpClient.GetAsync($"http://localhost:5089/api/Products/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var productResponse = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignora diferenças de maiúsculas/minúsculas
                    };
                    var product = JsonSerializer.Deserialize<ProductDTO>(productResponse, options);
                    Console.WriteLine(product.Name);

                    if (product != null)
                    {
                        stockItem.ProductName = product.Name;

                        _context.Stocks.Add(stockItem);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Produto inválido.");
                    }
                }
                else
                {
                    throw new Exception("Produto inexistente.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar o estoque: {ex.Message}");
            }
        }
        public async Task<IEnumerable<StockItem>> GetAllStockAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<StockItem> GetStockByProductIdAsync(int productId)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);
        }

        public async Task<ProductDTO> GetProductDetailsFromApiAsync(int productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5089/api/Products/{productId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignora diferenças de maiúsculas/minúsculas
                    };
                    return JsonSerializer.Deserialize<ProductDTO>(responseContent, options);
                }
                else
                {
                    Console.WriteLine($"Failed to fetch product details. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Deserialization Error: {jsonEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching product details: {ex.Message}");
                return null;
            }
        }


        // Método para excluir um item de estoque
        public async Task<bool> DeleteStockAsync(int id)
        {
            var stockItem = await _context.Stocks.FindAsync(id);
            if (stockItem == null)
                return false;

            _context.Stocks.Remove(stockItem);
            await _context.SaveChangesAsync();
            return true;
        }

        // Método para atualizar a quantidade de um item de estoque
        public async Task<bool> UpdateStockQuantityAsync(int id, int newQuantity)
        {
            var stockItem = await _context.Stocks.FindAsync(id);
            if (stockItem == null)
                return false;

            stockItem.Quantity = newQuantity; // Atualiza a quantidade
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductNameAsync(int stockId, string newName)
        {
            var stockItem = await _context.Stocks.FindAsync(stockId);
            if (stockItem == null)
                return false;

            // Agora, vamos fazer uma requisição PATCH à API de produtos para atualizar o nome do produto
            var response = await _httpClient.PatchAsync($"http://localhost:5089/api/Products/{stockItem.ProductId}",
                new StringContent(JsonSerializer.Serialize(new { name = newName }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                // Se a atualização foi bem-sucedida na API de produtos, retornamos true
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateStockProductNameAsync(int productId, string newName)
        {
            Console.WriteLine(productId);
            // Busca o item no estoque pelo ProductId
            var stockItem = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);

            if (stockItem == null)
                return false; // Retorna false se nenhum item for encontrado com o ProductId

            // Atualiza o nome do produto no item do estoque
            stockItem.ProductName = newName;

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();
            return true; // Retorna true indicando sucesso
        }
    }
}
