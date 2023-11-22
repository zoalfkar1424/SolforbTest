using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace SolforbTest.Models
{
    
    public class Order : IValidatableObject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        
        [Column(TypeName = "datetime2(7)")]
        public DateTime Date {  get; set; }
        public int ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public virtual Provider? Provider { get; set; }
        //to save order with items together
        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        //adding validation on items names
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var item in OrderItems) {
                if (Number == item.Name)
                {
                    yield return new ValidationResult("Item Name cannot be equal to Order Number");
                }
            }
        }
    }
}
