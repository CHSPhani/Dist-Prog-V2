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
}
