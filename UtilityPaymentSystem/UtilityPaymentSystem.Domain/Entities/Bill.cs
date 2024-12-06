using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentSystem.Domain.Entities
{
    public class Bill
    {
        public int BillId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public Service Service { get; set; }
    }
}
