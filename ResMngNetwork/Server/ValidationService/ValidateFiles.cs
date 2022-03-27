using ContractDataModels;
using DataSerailizer;
using Server.DataFileProcess;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.ValidationService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ValidateFiles:IValidateService
    {
        public static DBData dbData =null; 

        public ValidateFiles()
        {
        }
        
        public void ValidateFiless()
        {
            string folderPath = @"C:\WorkRelated-Offline\Simulation Data";// C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3\MatlabAnlaysis_15-03-2022";
            OWLDataG oDatag = ValidateFiles.dbData.OwlData;
            string[] fileEntries = Directory.GetFiles(folderPath);
            foreach (string fileName in fileEntries)
            {
                DataFilesProcess dfProcess = new DataFilesProcess(fileName);
                IFileProcessResult pRes = dfProcess.ProcessFile();

                List<string> colNames = ((CSVFileProcessResult)pRes).ColNames;

                List<string> suitableCN = new List<string>();
                List<string> unmatchedColNames = new List<string>();

                List<string> clNames = new List<string>();

                //Step-1:Get Class names, which are equivalent to or part of property names 
                foreach (KeyValuePair<string, SemanticStructure> sp in oDatag.RDFG.NODetails)
                {
                    if (sp.Value.SSType == SStrType.Class)
                    {
                        clNames.Add(sp.Key.Split(':')[0]);
                    }
                }
                
                foreach (string cn in colNames)
                {
                    var oC = clNames.Find((a) => { if (a.ToLower().Contains(cn.ToLower()) || cn.ToLower().Contains(a.ToLower())) { return true; } else { return false; } });
                    if (oC != null)
                        suitableCN.Add(oC);
                    else
                        unmatchedColNames.Add(cn);
                }
            }
        }

        List<string> IValidateService.ValidateFiles(string folderPath)
        {
            List<string> results = new List<string>();
            if (!Directory.Exists(folderPath))
            {
                return null;
            }
            else
            {
                
            }
            return results;
        }
    }
}
