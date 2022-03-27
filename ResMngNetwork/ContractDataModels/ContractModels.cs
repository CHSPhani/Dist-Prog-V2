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
}
