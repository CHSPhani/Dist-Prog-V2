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
        
        List<string> IValidateService.ValidateFiles(string folderPath)
        {
            List<string> results = new List<string>();
            if (Directory.Exists(folderPath))
            {
                string[] fileEntries = Directory.GetFiles(folderPath);
                foreach (string fileName in fileEntries)
                {
                    
                    string actualFileName = fileName.Split('\\')[fileName.Split('\\').Length - 1];
                    if (actualFileName.EndsWith(".m"))
                        continue;
                    string instName = string.Empty;
                    if (actualFileName.Split('_')[2].Equals("subxfmr")) //Exceptional condition nonsense
                        instName = actualFileName.Split('_')[2];
                    else
                        instName = string.Format("{0}_{1}", actualFileName.Split('_')[2], actualFileName.Split('_')[3]);

                    bool processed = false;

                    foreach (NodeData nData in ValidateFiles.dbData.NodeData)
                    {
                        if (nData.FileName.Equals(actualFileName))
                        {
                            results.Add(string.Format("The File {0} already processed Successfully", actualFileName));
                            processed = true;
                            break;
                        }
                    }
                    if (processed)
                    {
                        processed = false;
                        continue;
                    }
                    else
                    {
                        DataFilesProcess dfProcess = new DataFilesProcess(fileName);
                        IFileProcessResult pRes = dfProcess.ProcessFile();

                        ValidationResult vRes = ValidateDataSets.ValidateCSVDataSet(ValidateFiles.dbData, instName, pRes, true);
                        results.Add(string.Format("Processing of {0} resulted in {1}", actualFileName, vRes.Reason));

                        //Save successful data to AU
                        if (vRes.Validated)
                        {
                            NodeData nData = new NodeData();

                            nData.FileName = actualFileName;
                            nData.AbsLocation = fileName;
                            nData.FileType = ((CSVFileProcessResult)pRes).FileType.ToString();
                            nData.ColNames = ((CSVFileProcessResult)pRes).ColNames;
                            nData.NoOfCols = ((CSVFileProcessResult)pRes).NoOfCols;
                            nData.NoOfRows = ((CSVFileProcessResult)pRes).NoOfRows;
                            nData.ProcessResult = ((CSVFileProcessResult)pRes).ProcessResult;
                            nData.VerifiedDataSet = vRes.OClassName;
                            nData.AttachedInstance = vRes.InstName;
                            if (ValidateFiles.dbData.NodeData == null)
                            {
                                ValidateFiles.dbData.NodeData = new List<NodeData>();
                                ValidateFiles.dbData.NodeData.Add(nData);
                            }
                            else
                            {
                                ValidateFiles.dbData.NodeData.Add(nData);
                            }
                        }
                        //added successfully resolved thing to AU
                    }
                }
            }
            else
            {

            }
            return results;
        }
    }
}


/*
 * 
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
 * 
 * */
