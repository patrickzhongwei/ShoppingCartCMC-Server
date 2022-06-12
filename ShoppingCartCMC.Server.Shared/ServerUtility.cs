using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ShoppingCartCMC.Server.Shared
{
    public class ServerUtility
    {
        public static DateTime Utc19700101 = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc); //PW: when DateTime substract, their "kinds" are not considered at all!!!


        public static DateTime UtcFromEpoch(long ms)
        {
            return Utc19700101.AddSeconds(ms / 1000);
        }


        public static long EpochFromUtc(DateTime utc) //PW: milliseconds since Utc 1970-1-1
        {
            return (long)(utc - ServerUtility.Utc19700101).TotalMilliseconds;
        }




        public static int DaysDiff(DateTime from, DateTime to) //PW: if from<to, positive; if from>to, negative.
        {
            if (from.Kind == DateTimeKind.Utc && to.Kind == DateTimeKind.Utc)
            {
                return to.ToLocalTime().Date.Subtract(from.ToLocalTime().Date).Days;
            }
            else if (from.Kind == DateTimeKind.Utc && to.Kind != DateTimeKind.Utc)
            {
                return to.Date.Subtract(from.ToLocalTime().Date).Days;
            }
            else if (from.Kind != DateTimeKind.Utc && to.Kind == DateTimeKind.Utc)
            {
                return to.ToLocalTime().Date.Subtract(from.Date).Days;
            }
            else //PW: all treated as local, regardless local or unspecified.
            {
                return to.Date.Subtract(from.Date).Days;
            }
        }

        public static bool IsWeekEnd(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                return true;
            else
                return false;
        }



        public static bool IsAllowedIP(string ip, List<string> ipWhiteList)
        {
            if (ipWhiteList.Contains(ip))
            {
                return true;
            }
            else //PW: handle IP range such as '192.168.1.0', only last segment allow to be zero!
            {
                foreach (string whiteIP in ipWhiteList)
                {
                    string[] spearator = { "." };
                    string[] segments = whiteIP.Split(spearator, StringSplitOptions.RemoveEmptyEntries);

                    if (segments.Length == 4)
                    {
                        if (segments[3].Trim() == "0")
                        {
                            if (ip.StartsWith(segments[0] + "." + segments[1] + "." + segments[2]))
                                return true;
                        }
                    }
                }

                return false;
            }
        }


        public static bool IsInternalRequest(IConfiguration config, string remoteIpAddress)
        {
            //try
            //{
            //    //PW: check if IP is in white list
            //    string internalIpWhiteList = config.GetValue<string>("InternalIpWhiteList");
            //    string[] spearator = { ",", " " };
            //    string[] ips = internalIpWhiteList.Split(spearator, StringSplitOptions.RemoveEmptyEntries);
            //    List<string> ipList = new List<string>(ips);

            //    if (ServerUtility.IsAllowedIP(remoteIpAddress, ipList))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

            return true;
        }


        public static bool IsValidPhoneNumber(string number) //PW: (64)1324077
        {
            if (number == null)
                return false;

            int openBracketIndex = number.IndexOf("(");
            int closeBracketIndex = number.IndexOf(")");

            if (openBracketIndex != 0)
            {
                return false;
            }
            else if (closeBracketIndex <= (openBracketIndex + 1))
            {
                return false;
            }
            else
            {
                string countryCode = number.Substring(openBracketIndex + 1, closeBracketIndex - 1);
                string localNumber = number.Substring(closeBracketIndex + 1);

                long convertResult;
                bool convert1Done = long.TryParse(countryCode, out convertResult);
                bool convert2Done = long.TryParse(localNumber, out convertResult);

                if (convert1Done && convert2Done)
                {
                    return true;
                }
            }

            return false;
        }


        
    }
}
