using DataSerailizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace ContractDataModels
{
    [ServiceContract]
    public interface IValidateService
    {
        [OperationContract]
        List<string> ValidateFiles(string folderPath);
    }

    [ServiceContract]
    public interface IUploadIndividuals
    {
        [OperationContract]
        List<string> UploadIndividuals(List<CircuitEntry> entries);
    }

    [ServiceContract]
    public interface IObtainAllIndividuals
    {
        [OperationContract]
        List<SemanticStructure> ObtainAllIndividuals();
    }

    [ServiceContract]
    public interface IObtainSSDetails
    {
        [OperationContract]
        SemanticDetails ObtainSD(string SsName);
    }

    [ServiceContract]
    public interface IObtainSSForInst
    {
        [OperationContract]
        SemanticStructure ObtainSSForInst(string InsName);
    }
      
    [ServiceContract]
    public interface ISubmitPVS
    {
        [OperationContract]
        bool SubmitPV(List<CircuitEntry> PVSystems);
    }

    [ServiceContract]
    public interface ISendPVInfo
    {
        [OperationContract]
        List<CircuitEntry> SendPVInfo();
    }

    [ServiceContract]
    public interface IObtainLoadIndividuals
    {
        [OperationContract]
        List<SemanticStructure> ObtainLoadIndividuals();
    }

    [ServiceContract]
    public interface IObtainSearchResults
    {
        [OperationContract]
        string GetSearchResults(string sTerm);
    }

    [ServiceContract(CallbackContract = typeof(ISendAddNewUserResult))]
    public interface IAddNewUserRole
    {
        [OperationContract(IsOneWay = true)]
        void AddNewUser(string UName);
    }

    public interface ISendAddNewUserResult
    {
        [OperationContract(IsOneWay = true)]
        void SendAddUserResult(bool res);
    }

    [ServiceContract(CallbackContract = typeof(ISendUserAddUpdate))]
    public interface IAddUserInstance
    {
        [OperationContract(IsOneWay = true)]
        void AddUserInstance(UInstEntry uiEntry);
    }

    public interface ISendUserAddUpdate
    {
        [OperationContract(IsOneWay = true)]
        void SendUAddResult(bool res);
    }
}
