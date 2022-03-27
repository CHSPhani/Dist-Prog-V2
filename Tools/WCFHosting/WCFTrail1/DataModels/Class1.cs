using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    [DataContract]
    public class LargeWorkResult
    {
        string msg;
        [DataMember]
        public string Message
        {
            get
            {
                return this.msg;
            }
            set
            {
                this.msg = value;
            }
        }
    }

    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        string[] GetMessages();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MessageService : IMessageService
    {
        public string[] GetMessages()
        {
            return new string[] { "server1", "server2" };
        }
    }

    [ServiceContract]
    public interface ISMSService
    {
        [OperationContract]
        LargeWorkResult DoSomeThing();
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SMSService : ISMSService
    {
        public LargeWorkResult DoSomeThing()
        {
            LargeWorkResult lwRes = new LargeWorkResult();

            for (int i = 0; i < 100; i++)
            {

            }
            lwRes.Message = "Iam Done";
            return lwRes;
        }
    }

    [ServiceContract]
    public interface IValidationService
    {
        [OperationContract]
        string ValidateSets(LargeWorkResult lwSet);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ValidateSets : IValidationService
    {
        string IValidationService.ValidateSets(LargeWorkResult lwSet)
        {
            if (string.IsNullOrEmpty(lwSet.Message))
                return "NotGood";
            return "Validation";
        }
    }
}
