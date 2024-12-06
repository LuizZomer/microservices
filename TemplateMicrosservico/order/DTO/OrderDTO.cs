namespace Exemplo
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int StockID { get; set; }
    }

    public class CreateOrderDTO
    {
        public int StockId { get; set; }
    }
}
