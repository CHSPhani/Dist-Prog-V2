using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegModule.Utilities
{
    public class ParseUtils
    {
        public static string SResult = "Search Results";
        public static string PStr = "Primary structure";
        public static string CHierar = "Class hierarchy";
        public static string DProps = "Data Properties";
        public static string SCls = "Sub Classes";
        public static string ORstr = "Object Restrictions";
        public static string Inst = "Intsnaces";

        public static List<string> GetStdStrings()
        {
            List<string> StdStrings = new List<string>();
            StdStrings.Add(ParseUtils.SResult);
            StdStrings.Add(ParseUtils.PStr);
            StdStrings.Add(ParseUtils.CHierar);
            StdStrings.Add(ParseUtils.DProps);
            StdStrings.Add(ParseUtils.SCls);
            StdStrings.Add(ParseUtils.ORstr);
            StdStrings.Add(ParseUtils.Inst);
            return StdStrings;
        }
        public static Dictionary<string, List<string>> ParseSearchString(string result)
        {
            Dictionary<string, List<string>> sResult = new Dictionary<string, List<string>>();
            List<string> StdStrings = ParseUtils.GetStdStrings();
            List<string> pResult = result.Split('\n').ToList<string>();
            List<string> iResults = new List<string>();
            string kStr = string.Empty;
            foreach(string prs in pResult )
            {
                if (string.IsNullOrEmpty(prs))
                    continue;
                if(StdStrings.Contains(prs.Trim()))
                {
                    if (!string.IsNullOrEmpty(kStr))
                    {
                        sResult[kStr] = iResults;
                        kStr = string.Empty;
                    }
                    kStr = prs.Trim();
                    iResults = new List<string>();
                }
                else
                {
                    iResults.Add(prs.Trim());
                }
            }
            return sResult;
        }
    }
}
