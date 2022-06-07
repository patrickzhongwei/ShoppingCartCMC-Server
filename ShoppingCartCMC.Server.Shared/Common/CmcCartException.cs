using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Common
{
    public class CmcCartException: Exception
    {
        public int ErrorCode { get; set; }

        public CmcCartException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
