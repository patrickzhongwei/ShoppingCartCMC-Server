using ShoppingCartCMC.Server.Shared.Common;
using ShoppingCartCMC.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartCMC.Server.Shared.Common
{
    public class LocaleMessage
    {
        public static string ErrorCodeMessageTemplate(int errorCode, string languageCode)
        {
            if (languageCode == "en")
            {
                switch (errorCode)
                {
                    case ErrorCode.GENERAL_ERROR:
                        return "error";
                   
                        //PW: todo..
                    default:
                        return "";
                }
            }
            else if (languageCode == "zh")
            {
                switch (errorCode)
                {
                    case ErrorCode.GENERAL_ERROR:
                        return "error";
                    //PW: todo
                    default:
                        return "";
                }
            }

            return "";
        }

        public static string Translate(string word, string languageCode)
        {
            if (languageCode == "en")
            {
                switch (word)
                {
                    case "USD":
                        return "USD";
                    case "CNY":
                        return "CNY";
                    default:
                        return word;
                }
            }
            else if (languageCode == "zh")
            {
                switch (word)
                {
                    case "USD":
                        return "美元";
                    case "CNY":
                        return "人民币";
                    default:
                        return word;
                }
            }

            return word;
        }
    }
}
