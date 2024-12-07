using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentSystem.Domain.Entities
{
    public class Payment
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int BillId { get; set; }
        public DateTime PaymentDate { get; }
        public decimal Amount { get; set; }
    }
}
