using Microsoft.AspNetCore.Identity;
using ShoppingCartCMC.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShoppingCartCMC.Server.Shared.Identity
{
    public class MyIdentityUser : IdentityUser, IAuditableEntity
    {
        public virtual string FriendlyName
        {
            get
            {
                string friendlyName = string.IsNullOrWhiteSpace(FullName) ? UserName : FullName;

                if (!string.IsNullOrWhiteSpace(Title))
                    friendlyName = $"{Title} {friendlyName}";

                return friendlyName;
            }
        }

        
        public bool IsVIP { get; set; }

        public bool IsStaff { get; set; }

        public string FullName { get; set; }

        public string BaseGuid { get; set; }

        public string Notes { get; set; } 

        public string Configuration { get; set; }

        public string Title { get; set; }

        public string Company { get; set; }

        public string ServicePlan { get; set; }

        public string BaseCurrency { get; set; }

        public bool IsEnabled { get; set; }

        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeLastUpdated { get; set; }

        public DateTime UtcTimeCreated { get; set; }

        public DateTime UtcTimeLastUpdated { get; set; }
    }
}
