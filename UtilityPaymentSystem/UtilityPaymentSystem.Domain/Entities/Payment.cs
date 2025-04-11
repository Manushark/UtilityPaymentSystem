using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentSystem.Domain.Entities
{
    [Table("Payments")]
    public class Payment
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int BillId { get; set; }
        public DateTime? PaymentDate { get; set; } 
        public decimal Amount { get; set; }
    }
}
