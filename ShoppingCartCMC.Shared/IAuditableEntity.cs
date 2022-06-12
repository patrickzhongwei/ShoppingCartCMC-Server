using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }
        string LastUpdatedBy { get; set; }
        DateTime TimeCreated { get; set; }
        DateTime TimeLastUpdated { get; set; }
        DateTime UtcTimeCreated { get; set; }
        DateTime UtcTimeLastUpdated { get; set; }
    }
}
