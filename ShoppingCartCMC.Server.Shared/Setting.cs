using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared
{
    public class Setting
    {
        public static TimeSpan StaleRedisCacheTolerance = new TimeSpan(0, 0, 15);

        public static TimeSpan DefaultRedisCacheLifeTime = new TimeSpan(100, 0, 0); //PW: 100 hours, 
    }
}
