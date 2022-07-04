namespace Library.eCommerce.Models
{
    public partial class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public bool Bogo { get; set; }
        public virtual decimal TotalPrice { get; set; }

        public Product()
        {

        }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Description: {Description} - Price: ${Price} - Bogo: {Bogo}";
        }
    }
}
