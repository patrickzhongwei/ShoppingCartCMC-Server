using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared.Common
{
    public class SystemEnum
    {
        public enum RedisMessageType : int
        {
            Unknown = 0,            
            NewTrade = 1,
            TradeEOD = 2,
        }


        public enum IdType
        {
            Order = 1
        }

    }
}
