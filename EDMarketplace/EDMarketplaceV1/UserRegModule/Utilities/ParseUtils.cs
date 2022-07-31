using ContractDataModels;
using DataSerailizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserRegModule.Utilities
{
    public class ServiceUtils
    {
        public static List<string> GetUserRoles()
        {
            List<string> uRoles = new List<string>();
            string sResult = string.Empty;
            try
            {
                string uri = "net.tcp://localhost:6565/KGConsoleModel";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IObtainSearchResults>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                sResult = proxy.GetSearchResults("users");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + string.Format("Exception happend when calling Service for getting Serach details from KG. Details {0}", ex.Message));
                
            }
            if (string.IsNullOrEmpty(sResult))
            {
                Console.WriteLine(Environment.NewLine + string.Format("NO Search Results are obtained for Users from KG."));                
            }
            else
            {
                //Update status
                Console.WriteLine("Search Results obtained for Users from KG.");
                //Parse string and construct an object.
                Dictionary<string, List<string>> sResults = ParseUtils.ParseSearchString(sResult);
                uRoles = sResults[ParseUtils.SCls];
                //I am adding new Roles
                uRoles.Add("AlgorithmDeveloper*");
            }
            return uRoles;
        }
        public static List<string> GetPVDetails()
        {
            List<string> pvd = new List<string>();
            List<CircuitEntry> pvs = new List<CircuitEntry>();
            try
            {
                string uri = "net.tcp://localhost:6565/SendPVDetails";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<ISendPVInfo>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                pvs = proxy.SendPVInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not able to Obtain Individuals from Knowledge Graph. Exception is " + ex.Message);
            }
            if (pvs.Count == 0)
            {
                Console.WriteLine("Not able to obtain PVS");
            }
            else
            {
                Console.WriteLine("Obtained Details about PV Details.");
            }
            foreach(CircuitEntry cep in pvs)
            {
                pvd.Add(cep.CEName);
            }
            return pvd;
        }
        public static List<string> ReturnInstanceResult(string kgSearchStr)
        {
            string sResult = string.Empty;
            List<string> instances = new List<string>();
            try
            {
                string uri = "net.tcp://localhost:6565/KGConsoleModel";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IObtainSearchResults>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                sResult = proxy.GetSearchResults(kgSearchStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + string.Format("Exception happend when calling Service for getting Serach details from KG. Details {0}", ex.Message));
                return null;
            }
            if (string.IsNullOrEmpty(sResult))
            {
                Console.WriteLine(Environment.NewLine + string.Format("NO Search Results are obtained for Users from KG."));
                return null;
            }
            else
            {
                //Update status
                Console.WriteLine(Environment.NewLine + string.Format("Search Results obtained for Users from KG."));
                //Parse string and construct an object.
                Dictionary<string, List<string>> sResults = ParseUtils.ParseSearchString(sResult);
                //need to return what is wanted.
                instances = sResults[ParseUtils.Inst];
            }
            return instances;
        }
    }
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
            if(!string.IsNullOrEmpty(kStr))
            {
                sResult[kStr] = iResults;
                kStr = string.Empty;
            }
            return sResult;
        }
    }
}
