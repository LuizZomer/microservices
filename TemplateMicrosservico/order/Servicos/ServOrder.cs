using System.Net.Http;
using System.Text.Json;
using Exemplo;
using Template.Infra;
using static Exemplo.DataContext;
using System.Text.Json;
using Order.DTO;
using Microsoft.EntityFrameworkCore;

namespace Exemplo
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int stockId);
        Task<OrderWithStockDTO?> GetOrderByIdAsync(int id);
        Task UpdateOrderAsync(int orderId);
        Task DeleteOrderAsync(int orderId);
    }



    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;

        public OrderService(DataContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task CreateOrderAsync(int stockId)
        {
            // Verificar se o item existe no estoque
            var stockApiUrl = $"http://localhost:5001/api/Stock/by-product/{stockId}";
            var response = await _httpClient.GetAsync(stockApiUrl);

            Console.WriteLine(response.Content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("O item não foi encontrado no estoque.");
            }

            // Configurar o JsonSerializer para ser case insensitive
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserializar a resposta para verificar o quantity
            var stockResponse = await response.Content.ReadAsStringAsync();
            var stockItem = JsonSerializer.Deserialize<StockResponseDTO>(stockResponse, options);

            if (stockItem == null || stockItem.Stock.Quantity <= 0)
            {
                throw new Exception("O item não está disponível no estoque.");
            }

            // Criar a nova ordem
            var order = new OrderItem
            {
                StockId = stockId,
                OrderDate = DateTime.Now // Ou deixe que o valor padrão do banco trate isso.
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderWithStockDTO?> GetOrderByIdAsync(int id)
        {
            // Buscar a ordem no banco de dados
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return null; // Ordem não encontrada
            }

            // Verificar os dados do estoque chamando a API
            var stockApiUrl = $"http://localhost:5001/api/Stock/by-product/{order.StockId}";
            var response = await _httpClient.GetAsync(stockApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return null; // Não conseguiu pegar o estoque
            }

            var stockResponse = await response.Content.ReadAsStringAsync();
            var stockItem = JsonSerializer.Deserialize<StockResponseDTO>(stockResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (stockItem == null)
            {
                return null; // Estoque não encontrado ou inválido
            }

            // Montar o DTO final com as informações da ordem, estoque e produto
            var orderWithStock = new OrderWithStockDTO
            {
                Id = order.Id,
                StockId = order.StockId,
                OrderDate = order.OrderDate,
                Status = "Pending", // Aqui você pode ajustar o status, se necessário

                // Preenchendo os dados do estoque e do produto
                Stock = new StockDTO
                {
                    Id = stockItem.Stock.Id,
                    Quantity = stockItem.Stock.Quantity,
                    ProductId = stockItem.Stock.ProductId,
                    ProductName = stockItem.Stock.ProductName
                },

                ProductDetails = stockItem.ProductDetails
            };

            return orderWithStock;
        }

        public async Task UpdateOrderAsync(int orderId)
        {
            var orderEntity = await _context.Orders.FindAsync(orderId);

            if (orderEntity == null)
            {
                throw new Exception("Order not found.");
            }

            Console.WriteLine(orderEntity.Status);

            if (orderEntity.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                orderEntity.Status = "Completed";
            }
            else if (orderEntity.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
            {
                orderEntity.Status = "Pending";
            }
            else
            {
                throw new InvalidOperationException("Status inválido para alternar.");
            }

            _context.Entry(orderEntity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var orderEntity = await _context.Orders.FindAsync(orderId);

            if (orderEntity != null)
            {
                // Remove a ordem do contexto
                _context.Orders.Remove(orderEntity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order not found.");
            }
        }
    }

}
