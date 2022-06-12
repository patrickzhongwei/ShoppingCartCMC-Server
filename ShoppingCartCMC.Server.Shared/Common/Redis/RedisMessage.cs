using System;
using System.Collections.Generic;
using System.Text;
using static ShoppingCartCMC.Shared.Common.SystemEnum;

namespace ShoppingCartCMC.Server.Shared.Common.Redis
{
    public class RedisMessage
    {
        public RedisMessageType Type { get; set; }

        public DateTime UtcTimeStamp { get; set; }

        public string BodyTypeFullName { get; set; }

        public string JsonBody { get; set; }
    }
}
