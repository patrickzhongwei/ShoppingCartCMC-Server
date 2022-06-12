using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingCartCMC.Shared.Common;
using ServiceStack.Redis;
using static ShoppingCartCMC.Shared.Common.SystemEnum;

namespace ShoppingCartCMC.Server.Shared.Common
{
    public class IdGenerator : IIdGenerator
    {
       
        public IdGenerator()
        {
        }

        public long GetNextId(IdType type)
        {

            return 0;
        }
    }
}
