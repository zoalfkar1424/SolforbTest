using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolforbTest.Models
{
    public class OrderItem 
    {
        public OrderItem() { }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal Quantity { get; set; }
        [Required]
        public string Unit { get; set; }
        //used to deal with deleting items in create_order's form
        [NotMapped]
        public bool IsDeleted { get; set; } = false;

    }
   
}
