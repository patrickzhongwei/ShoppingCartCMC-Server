using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCartCMC.Shared;

namespace ShoppingCartCMC.Server.Shared.Identity
{
    public class MyIdentityRole : IdentityRole, IAuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MyIdentityRole"/>.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public MyIdentityRole()
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="MyIdentityRole"/>.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public MyIdentityRole(string roleName) : base(roleName)
        {

        }


        /// <summary>
        /// Initializes a new instance of <see cref="MyIdentityRole"/>.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <param name="description">Description of the role.</param>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public MyIdentityRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }





        /// <summary>
        /// Gets or sets the description for this role.
        /// </summary>
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastUpdated { get; set; }

        public DateTime UtcTimeCreated { get; set; }
        public DateTime UtcTimeLastUpdated { get; set; }

        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<IdentityUserRole<string>> Users { get; set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }
}
