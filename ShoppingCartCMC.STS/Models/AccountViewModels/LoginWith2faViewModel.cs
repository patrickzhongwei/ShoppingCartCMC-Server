using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCMC.STS.Models.AccountViewModels
{
    public class LoginWith2faViewModel : LoginWith2faInputModel
    {
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
