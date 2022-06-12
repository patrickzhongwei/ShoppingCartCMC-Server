using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Shared.Common
{
    public static class ClaimTypeConstants
    {
        public const string Locale = "ui_locale";
        public const string FullName = "full_name"; //PW: logged-in user's full name, maybe staff or client.
        public const string IsStaff = "is_staff";
    }
}
