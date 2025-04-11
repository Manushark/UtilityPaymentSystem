using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentSystem.Domain.Entities
{
    [Table("Services")]
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
    }
}