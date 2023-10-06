using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.Identity.Client;

namespace Expenses_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        //CategoryId
        [Range(1, int.MaxValue,ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        public int Amount { get; set; }
        [Column(TypeName="nvarchar(75)")]
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // return title with icon for transaction based on category
        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category == null? " " : Category.Icon + " " + Category.Title;
            }
        } 
        //return amount based on type of transaction
        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense")? "- " :  "+ ") + Amount.ToString("C", CultureInfo.GetCultureInfo("en-ie"));
            }
        }

    }
}
