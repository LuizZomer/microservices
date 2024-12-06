namespace Order.DTO
{
    public class StockDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }

    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class StockResponseDTO
    {
        public StockDTO Stock { get; set; }
        public ProductDetailsDTO ProductDetails { get; set; }
    }

    public class OrderWithStockDTO
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Mudando para string, para manter o valor "pending"

        public StockDTO Stock { get; set; }
        public ProductDetailsDTO ProductDetails { get; set; }
    }

}
