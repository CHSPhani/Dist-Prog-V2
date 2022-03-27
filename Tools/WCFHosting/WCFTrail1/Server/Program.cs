using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            
            IMessageService message = new MessageService();
            ServiceHost host = new ServiceHost(message, new Uri("net.tcp://localhost:6565/MessageService"));
            var binding = new NetTcpBinding(SecurityMode.None);
            //binding.PortSharingEnabled = true;
            host.AddServiceEndpoint(typeof(IMessageService), binding, "");
            host.Opened += Host_Opened;
            host.Open();
            Console.ReadLine();

            ISMSService smss = new SMSService();
            ServiceHost host2 = new ServiceHost(smss, new Uri("net.tcp://localhost:6565/SMSService"));
            var binding2 = new NetTcpBinding(SecurityMode.None);
            //binding2.PortSharingEnabled = true;
            host2.AddServiceEndpoint(typeof(ISMSService), binding2, "");
            host2.Opened += Host2_Opened;
            host2.Open();
            Console.ReadLine();

            IValidationService vds = new ValidateSets();
            ServiceHost host3 = new ServiceHost(vds, new Uri("net.tcp://localhost:6565/ValidateSets"));
            var binding3 = new NetTcpBinding(SecurityMode.None);
            host3.AddServiceEndpoint(typeof(IValidationService), binding3, "");
            host3.Opened += Host3_Opened;
            host3.Open();
            Console.ReadLine();

            // Configure a binding with TCP port sharing enabled
            //NetTcpBinding binding = new NetTcpBinding();
            //binding.PortSharingEnabled = true;

            // Start a service on a fixed TCP port
            //ServiceHost host = new ServiceHost(typeof(CalculatorService));
            //ushort salt = (ushort)new Random().Next();
            //string address = $"net.tcp://localhost:9000/calculator/{salt}";
            //host.AddServiceEndpoint(typeof(ICalculator), binding, address);
            //host.Open();
        }

        private static void Host3_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Validation Service opened");
        }

        private static void Host2_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Host2 opened");
        }

        private static void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("MEssage Service Started");
        }
    }
}


/*
 * var uris = new Uri[2];
            string address = "net.tcp://localhost:6565/MessageService";
            IMessageService message = new MessageService();
            uris[0] = new Uri(address);
            ServiceHost host = new ServiceHost(message, uris);
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IMessageService), binding, "");
            host.Opened += Host_Opened;
            host.Open();
            Console.ReadLine();
 * 
 * */
