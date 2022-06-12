using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartCMC.Shared
{
    public class AuditableEntity : IAuditableEntity
    {
        [MaxLength(256)]
        public string CreatedBy { get; set; }
        [MaxLength(256)]
        public string LastUpdatedBy { get; set; }
        public DateTime TimeLastUpdated { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime UtcTimeLastUpdated { get; set; }
        public DateTime UtcTimeCreated { get; set; }
    }
}
