using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class ClientGrantType
    {
        public int Id { get; set; }
        public string GrantType { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
