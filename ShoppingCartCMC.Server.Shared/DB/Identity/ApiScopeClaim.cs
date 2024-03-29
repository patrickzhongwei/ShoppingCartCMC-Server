﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class ApiScopeClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ScopeId { get; set; }

        public virtual ApiScope Scope { get; set; }
    }
}
