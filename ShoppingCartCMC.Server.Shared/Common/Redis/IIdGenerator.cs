using ShoppingCartCMC.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;
using static ShoppingCartCMC.Shared.Common.SystemEnum;

namespace ShoppingCartCMC.Server.Shared.Common
{
    public interface IIdGenerator
    {
        long GetNextId(IdType type);
    }
}
