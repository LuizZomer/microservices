using Microsoft.AspNetCore.Mvc;
using Exemplo;
using static Exemplo.DataContext;

namespace Exemplo
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IServExemplo _servExemplo;

        public class UpdateStockRequest
        {
            public int NewQuantity { get; set; }
        }

        public class UpdateProductNameRequest
        {
            public string NewName { get; set; }
        }

        public StockController(IServExemplo servExemplo)
        {
            _servExemplo = servExemplo;
        }

        // Método POST para criar um novo Stock
        [HttpPost]
        public async Task<IActionResult> AdicionarStock([FromBody] StockDTO novoStock)
        {
            try
            {
                if (novoStock == null)
                    return BadRequest("Dados inválidos.");

                if (novoStock.ProductId <= 0)
                    return BadRequest("ProductId é inválido.");

                // Convert StockDTO to StockItem
                var stockItem = new StockItem
                {
                    ProductId = novoStock.ProductId,
                    Quantity = novoStock.Quantity
                };

                // Pass the StockItem to the service
                await _servExemplo.AdicionarStockAsync(stockItem);

                return CreatedAtAction(nameof(AdicionarStock), new { id = stockItem.Id }, stockItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // Método GET para listar todos os StockItems
        [HttpGet]
        [Route("listAll")]
        public async Task<IActionResult> ListAll()
        {
            try
            {
                // Chama o método assíncrono
                var stockItems = await _servExemplo.GetAllStockAsync();

                if (stockItems == null || !stockItems.Any())
                    return NotFound("Nenhum item de estoque encontrado.");

                return Ok(stockItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            try
            {
                // Busca o item de estoque pelo ProductId
                var stockItem = await _servExemplo.GetStockByProductIdAsync(productId);


                if (stockItem == null)
                {
                    return NotFound("Item de estoque não encontrado para o ProductId fornecido.");
                }

                // Faz a requisição para outra API para buscar detalhes do produto
                var productDetails = await _servExemplo.GetProductDetailsFromApiAsync(productId);
                //Console.WriteLine(productDetails);

                if (productDetails == null)
                {
                    return NotFound("a.");
                }

                // Combina os dados do estoque com os detalhes do produto
                var result = new
                {
                    Stock = stockItem,
                    ProductDetails = productDetails
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // Método DELETE para remover um StockItem
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            try
            {
                var success = await _servExemplo.DeleteStockAsync(id);
                if (!success)
                    return NotFound("Item de estoque não encontrado.");

                return NoContent(); // Retorna um status 204 (No Content) quando deletado com sucesso
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // Método PATCH para atualizar a quantidade de um StockItem
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchStock(int id, [FromBody] UpdateStockRequest request)
        {
            if (request == null || request.NewQuantity <= 0)
            {
                return BadRequest("A quantidade fornecida é inválida.");
            }

            // Chama o serviço para atualizar a quantidade do produto no estoque
            await _servExemplo.UpdateStockQuantityAsync(id, request.NewQuantity);
            return NoContent();
        }

        [HttpPatch("{id}/update-name")]
        public async Task<IActionResult> PatchProductName(int id, [FromBody] UpdateProductNameRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NewName))
            {
                return BadRequest("O nome fornecido é inválido.");
            }

            try
            {
                // Chama o serviço para atualizar o nome do produto no estoque
                var success = await _servExemplo.UpdateStockProductNameAsync(id, request.NewName);

                if (!success)
                {
                    return NotFound("Produto não encontrado no estoque.");
                }

                return NoContent(); // Sucesso, retorna 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
