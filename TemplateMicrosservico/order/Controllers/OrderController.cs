using Microsoft.AspNetCore.Mvc;

namespace Exemplo
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        // Injeta o serviço pelo construtor
        public OrderController(IOrderService servOrder)
        {
            _orderService = servOrder;
        }


        public class CreateOrderDTO
        {
            public int StockId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            try
            {
                if (createOrderDTO == null || createOrderDTO.StockId <= 0)
                    return BadRequest("Invalid StockId.");

                // Call the service to create the order
                await _orderService.CreateOrderAsync(createOrderDTO.StockId);

                return CreatedAtAction(nameof(CreateOrder), new { stockId = createOrderDTO.StockId }, createOrderDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid order ID.");

                // Chama o serviço para obter a ordem com dados de estoque e produto
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                    return NotFound("Order not found.");

                return Ok(order); // Retorna os dados completos da ordem
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateOrderStatus(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid order ID.");

                // Chama o serviço para atualizar o status da ordem
                await _orderService.UpdateOrderAsync(id);  // Passando apenas o ID da ordem

                return NoContent();  // Retorna 204 No Content se a atualização for bem-sucedida
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid order ID.");

                // Chama o serviço para excluir a ordem
                await _orderService.DeleteOrderAsync(id);

                return NoContent(); // Retorna status 204 (sem conteúdo) indicando que a exclusão foi bem-sucedida
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Message = e.Message });
            }
        }


    }
}
