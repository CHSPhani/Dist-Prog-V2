using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PRess any key to continue");
            Console.ReadLine();

            string uri = "net.tcp://localhost:6565/MessageService";
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<IMessageService>(binding);
            var endPoint = new EndpointAddress(uri);
            var proxy = channel.CreateChannel(endPoint);
            var result = proxy.GetMessages();
            if (result != null)
            {
                result.ToList().ForEach((p) => { Console.WriteLine(p); });
            }

            Console.ReadLine();
            Console.WriteLine("PRess any key to continue");
            Console.ReadLine();

            string uri2 = "net.tcp://localhost:6565/SMSService";
            NetTcpBinding binding2 = new NetTcpBinding(SecurityMode.None);
            var channel2 = new ChannelFactory<ISMSService>(binding2);
            var endPoint2 = new EndpointAddress(uri2);
            var proxy2 = channel2.CreateChannel(endPoint2);
            var result2 = proxy2.DoSomeThing();
            if (result2 != null)
            {
                Console.WriteLine(result2.Message); // as LargeWorkResult).Message);
            }

            Console.ReadLine();
            Console.WriteLine("PRess any key to continue");
            Console.ReadLine();

            string uri3 = "net.tcp://localhost:6565/ValidateSets";
            NetTcpBinding binding3 = new NetTcpBinding(SecurityMode.None);
            var channel3 = new ChannelFactory<IValidationService>(binding3);
            var endPoint3 = new EndpointAddress(uri3);
            var proxy3 = channel3.CreateChannel(endPoint3);
            var result3 = proxy3.ValidateSets(new LargeWorkResult() { Message = "Msg" });
            if(result3 != null)
            {
                Console.WriteLine(result3);
            }
            Console.ReadLine();
        }
    }
}
