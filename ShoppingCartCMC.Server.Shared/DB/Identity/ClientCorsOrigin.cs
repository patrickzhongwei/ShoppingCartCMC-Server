using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class ClientCorsOrigin
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
