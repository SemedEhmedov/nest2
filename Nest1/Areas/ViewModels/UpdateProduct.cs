using System.ComponentModel.DataAnnotations.Schema;

namespace Nest1.Areas.ViewModels
{
    public class UpdateProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
