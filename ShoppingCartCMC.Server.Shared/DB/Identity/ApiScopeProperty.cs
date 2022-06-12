using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class ApiScopeProperty
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ScopeId { get; set; }

        public virtual ApiScope Scope { get; set; }
    }
}
