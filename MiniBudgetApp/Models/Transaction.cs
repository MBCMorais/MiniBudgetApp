using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBudgetApp.Models
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
     
    }
}
