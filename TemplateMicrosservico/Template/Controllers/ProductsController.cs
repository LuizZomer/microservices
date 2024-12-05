using MyProject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static MyProject.DataContext;

namespace MyProject
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {

        public class UpdateProductNameRequest
        {
            public string NewName { get; set; }
        }

        private readonly IServProducts _servProducts;

        public ProductsController(IServProducts servProducts)
        {
            _servProducts = servProducts;
        }

        // Criar um novo produto
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductsDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Chame o serviço para criar o produto
                var createdProduct = await _servProducts.CreateProductAsync(productDto);

                if (createdProduct == null)
                    return BadRequest("Erro ao criar o produto.");

                return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }

        // Obter todos os produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> GetAll()
        {
            try
            {
                // Usando o serviço para obter todos os produtos
                var products = await _servProducts.GetAllProductsAsync();

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }

        // Obter um produto por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsDTO>> GetById(int id)
        {
            try
            {
                var productDto = await _servProducts.GetProductByIdAsync(id);

                if (productDto == null)
                    return NotFound("Produto não encontrado.");

                return Ok(productDto);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }

        // Novo endpoint DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _servProducts.DeleteProductAsync(id);

            if (!isDeleted)
                return NotFound("Produto não encontrado.");

            return NoContent(); // Retorna 204 No Content, indicando que o recurso foi excluído com sucesso
        }

        // Novo endpoint PUT para atualizar produto
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductsDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedProduct = await _servProducts.UpdateProductAsync(id, productDto);

            if (updatedProduct == null)
                return NotFound("Produto não encontrado.");

            return Ok(updatedProduct); // Retorna o produto atualizado
        }
        }
    }

