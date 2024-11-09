using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DemoRazorPages.Models
{
    public class Category
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Category Name.")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Display Order.")]
        [Range(1, 1000, ErrorMessage = "Please Enter between 1 to 1000.")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

    }
}
