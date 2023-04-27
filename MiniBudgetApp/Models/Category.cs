using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBudgetApp.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage ="Name is required")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expense";

        [NotMapped] 
        public string TitlewithIcon {
            get
            {
                return this.Icon + " " + this.Name;
            } 
        }

    }
}
