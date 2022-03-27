using DataSerailizer;
using Server.ChangeRules;
using Server.DataFileProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Reasoner
{
    /// <summary>
    /// This class reasons to possibly select a ontology class which could semantically 
    /// represent a data set. There are the below assumptions
    /// propert name (replace several chars)
    /// data type
    /// only select if all properties are part of one class
    /// if all properties are found in multiple classes then..? 
    /// </summary>
    public class PBReasoner
    {
        public PBReasoner() { }

        public OClass ReasonWithProperties(DBData dbData, CSVFileProcessResult csvFPResult)
        {
            bool considerPName = true;
            bool considerPType = true;

            List<OClass> suitableClass = new List<OClass>();

            //Get
            int propCount = 0;
            List<String> colValues = new List<string>();
            foreach (string s in csvFPResult.FileContent)
            {
                string coName = colValues[0].Trim();
                colValues.Add(s.Split(',')[propCount]);
                //ODataProperty oDP =  dbData.OwlData.OWLDataProperties.Find((dp) => { if (dp.DProperty.Trim().ToLower().Equals(coName.ToLower())) { return true; } else { return false; } });

                //foreach (OChildNode ocNode in oDP.DPChildNodes)
                //{
                //    if (ocNode.CNType.Equals("rdfs:domain"))
                //    {
                //        //class name
                //    }
                //    if (ocNode.CNType.Equals("rdfs:range") || ocNode.CNType.Equals("owl:onDatatype"))
                //    {
                //        //Type

                //    }
                //}
            }

            return null;
        }
    }
}
