using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared.Pricing
{
    public interface IPriceFeed
    {
        void Start();
        void SetUpdateFrequency(double value);
        double GetUpdateFrequency();
    }
}
