using DataSerailizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataFileProcess
{
    public enum AllowedFileTypes
    {
        txt,
        csv,
        xlsx,
        xml,
        json,
        invalid
    }

    public class DataFilesProcess
    {
        string fileName;

        public DataFilesProcess()
        {
            fileName = string.Empty;
        }

        public DataFilesProcess(string fName) : this()
        {
            this.fileName = fName;
        }

        //TODO: Here we know for sure the extension of file. We need to pass this further
        public IFileProcessResult ProcessFile()
        {
            switch (IsProcessingAllowed())
            {
                case AllowedFileTypes.csv:
                    {
                        return ProcessCSVFile();
                    }
                case AllowedFileTypes.txt:
                    {
                        return ProcessTextFile();
                    }
                case AllowedFileTypes.xlsx:
                    {
                        return ProcessXLSX();
                    }
                case AllowedFileTypes.xml:
                    {
                        return ProcessXML();
                    }
                case AllowedFileTypes.json:
                    {
                        return ProcessJSON();
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        CSVFileProcessResult ProcessCSVFile()
        {
            CSVFileProcessResult csvPFResult = new CSVFileProcessResult();
            csvPFResult.FileType = AllowedFileTypes.csv;
            try
            {
                int counter = 1;
                string[] lines = System.IO.File.ReadAllLines(this.fileName);

                foreach (string line in lines)
                {
                    csvPFResult.FileContent.Add(line);
                    if (counter == 1)
                    {
                        csvPFResult.ColNames = line.Split(',').ToList<string>();
                        csvPFResult.NoOfCols = csvPFResult.ColNames.Count;
                    }
                    counter++;
                }
                csvPFResult.NoOfRows = counter;
            }
            catch (Exception ex)
            {
                csvPFResult.ProcesState = ex.Message;
                csvPFResult.ProcessResult = false;
                return csvPFResult;
            }
            csvPFResult.ProcesState = "process successful";
            csvPFResult.ProcessResult = true;
            return csvPFResult;
        }

        IFileProcessResult ProcessTextFile()
        {
            return null;
        }

        IFileProcessResult ProcessXLSX()
        {
            return null;
        }

        IFileProcessResult ProcessXML()
        {
            return null;
        }

        IFileProcessResult ProcessJSON()
        {
            return null;
        }

        AllowedFileTypes IsProcessingAllowed()
        {
            FileInfo fi = new FileInfo(fileName);
            switch (fi.Extension)
            {
                case ".csv":
                    {
                        return AllowedFileTypes.csv;
                    }
                case ".txt":
                    {
                        return AllowedFileTypes.txt;
                    }
                case ".xlsx":
                    {
                        return AllowedFileTypes.xlsx;
                    }
                case ".xml":
                    {
                        return AllowedFileTypes.xml;
                    }
                case ".json":
                    {
                        return AllowedFileTypes.json;
                    }
                default:
                    {
                        return AllowedFileTypes.invalid;
                    }
            }
        }
    }

    public interface IFileProcessResult
    {
        bool ProcessResult { get; set; }

        string ProcesState { get; set; }

        AllowedFileTypes FileType { get; set; }
    }

    public class CSVFileProcessResult : IFileProcessResult
    {
        public List<string> FileContent;

        public List<string> ColNames;
        public int NoOfCols { get; set; }

        public int NoOfRows { get; set; }

        bool processRes;
        public bool ProcessResult
        {
            get
            {
                return processRes;
            }
            set
            {
                processRes = value;
            }
        }

        string pState;

        public string ProcesState
        {
            get
            {
                return this.pState;
            }
            set
            {
                pState = value;
            }
        }

        AllowedFileTypes fType;

        public AllowedFileTypes FileType
        {
            get
            {
                return this.fType;
            }
            set
            {
                fType = value;
            }
        }

        public CSVFileProcessResult()
        {
            FileType = AllowedFileTypes.invalid;
            FileContent = new List<string>();
            ColNames = new List<string>();
            NoOfCols = 0;
            NoOfRows = 0;
            processRes = false;
        }
    }

    public class ValidationResult
    {
        public bool Validated;
        public string Reason { get; set; }
        public string OClassName { get; set; }
        public string Details { get; set; }

        public ValidationResult()
        {
            Validated = false;
            Reason = string.Empty;
            OClassName = string.Empty;
            Details = string.Empty;
        }
    }
}
