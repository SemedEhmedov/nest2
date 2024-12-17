using System.ComponentModel.DataAnnotations.Schema;

namespace Nest1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<TagProduct> TagProducts { get; set; }
    }
}
