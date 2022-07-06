namespace Library.eCommerce.Models
{
    public partial class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Id { get; set; }
        public bool Bogo { get; set; }
        public virtual double TotalPrice { get; set; }
        // unique ID that is given to a product and is kept throughout cart/inventory
        public int UID { get; set; }
        public Product()
        {

        }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Description: {Description} - Price: ${Price} - Bogo: {Bogo}";
        }
    }
}
