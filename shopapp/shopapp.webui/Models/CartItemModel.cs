namespace shopapp.webui.Models
{
    public class CartItemModel{
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}