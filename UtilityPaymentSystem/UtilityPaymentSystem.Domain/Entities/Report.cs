using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentSystem.Domain.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string ReportData { get; set; }
    }
}
