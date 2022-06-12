using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsVip { get; set; }
        public bool IsStaff { get; set; }
        public string FullName { get; set; }
        public string Notes { get; set; }
        public string Configuration { get; set; }
        public string Title { get; set; }
        public string BaseCurrency { get; set; }
        public string Company { get; set; }
        public string ServicePlan { get; set; }
        public bool IsEnabled { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime UtcTimeCreated { get; set; }
        public DateTime UtcTimeLastUpdated { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastUpdated { get; set; }
        public string BaseGuid { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
    }
}
