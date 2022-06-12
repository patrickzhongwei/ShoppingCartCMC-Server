using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Common
{
    public class ErrorCode
    {
        // general
        public const int SUCCESS = 0;        //success

        public const int CCY_UNSUPPORT = -1000; //PW: currecny unsupport

        public const int CCYPAIR_UNSUPPORT = -1001; //PW: currency pair unsupport

        public const int RATE_CHANGED = -1002; //PW: detect rate changed when validate order being placed

        public const int CARTPRICE_WRONG = -1003; //PW: detect cart-item price changed when validate order being placed

        public const int SHIPPINGFEE_WRONG = -1004; //PW: detect wrong shipping fee when validate order being placed

        public const int CARTTOTAL_WRONG = -1005; //PW: detect wrong shopping cart total when validate order being placed

        public const int GENERAL_ERROR = -1;       //unknown 	

    }
}
