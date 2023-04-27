using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace MiniBudgetApp.Models
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;


        //[Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]

        [Required(ErrorMessage ="teste")]
        [RequiredGuid(ErrorMessage = "Please select a category.")]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        [NotMapped] 
        public string? CategoryTitleWithIcon 
        { 
            get
            { 
                return Category==null ? "" : Category.Icon + " " + Category.Name;
            } 
        }
        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C2");
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NonEmptyGuidAttribute : ValidationAttribute
    {

        public override bool IsValid(Object value)
        {
            bool result = true;

            if ((Guid)value == Guid.Empty)
                result = false;

            return result;
        }

     
    }

    public class RequiredGuidAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var guid = CastToGuidOrDefault(value);

            return !Equals(guid, default(Guid));
        }

        private static Guid CastToGuidOrDefault(object value)
        {
            try
            {
                return (Guid)value;
            }
            catch (Exception e)
            {
                if (e is InvalidCastException || e is NullReferenceException) return default(Guid);
                throw;
            }
        }
    }

}
