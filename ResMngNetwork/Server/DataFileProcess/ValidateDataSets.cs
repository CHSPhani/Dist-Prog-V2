using DataSerailizer;
using Server.ChangeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Reasoner;

namespace Server.DataFileProcess
{
    public class DPropDetails
    {
        public string PName { get; set; }
        public string PType { get; set; }
        public string PExpression { get; set; }

        public DPropDetails()
        {
            PName = PType = PExpression = string.Empty;
        }
    }
    public class ValidateDataSets
    {
        public static ValidationResult ValidateCSVDataSet(DBData dbData, IFileProcessResult pResult, string selectedDSName, bool adValue)
        {
            ValidationResult vResult = new ValidationResult();
            CSVFileProcessResult csvFPResult = pResult as CSVFileProcessResult;
            Dictionary<string, bool> foundNames = new Dictionary<string, bool>();
            List<DPropDetails> colNamesExprs = new List<DPropDetails>();
            List<ODataProperty> dmDPs = dbData.OwlData.OWLDataProperties;
            List<ODataProperty> selectedDPs = new List<ODataProperty>();

            //Step1:-Select the corrent data set from all datasets obtained from ontologies
            foreach (ODataProperty dp in dmDPs)
            {
                var res = dp.DPChildNodes.FindAll((p) => { if ((p.CNType.Equals("rdfs:domain")) && (p.CNName.Equals(selectedDSName))) { return true; } else { return false; } });
                foreach(OChildNode ocNode in res)
                {
                    selectedDPs.Add(dp);
                }
            }
            //Step2:- This is seemingly not required but need to get all col names by trimming
            foreach(string s in csvFPResult.ColNames)
            {
                foundNames[s.Trim()] = false;
            }
             
            //Step3:- Process each DP for finding the subset of proerties and proceed further
            //subset of properties is possible because ontologu can have more properties thatn specified in dataset.
            foreach (ODataProperty oDPR in selectedDPs)
            {
                string processedName = ValidateDataSets.RepalceChars(oDPR.DProperty);
                if (foundNames.Keys.Contains(processedName))
                {
                    DPropDetails dpDetails = new DPropDetails();

                    StringBuilder sBuilder = new StringBuilder();
                    sBuilder.Append("[");
                    sBuilder.Append(" ");
                    foundNames[processedName] = true; //oDPR.DProperty

                    foreach (OChildNode ocNode in oDPR.DPChildNodes)
                    {
                        if(ValidateRules.IsOperator(ocNode.CNType))
                        {
                            //now we need to construct exprssion
                            sBuilder.Append(" ");
                            sBuilder.Append(ValidateRules.GetOperator(ocNode.CNType));
                            sBuilder.Append(" ");
                            sBuilder.Append(ocNode.CNSpecialValue);
                        }
                        if(ocNode.CNType.Equals("rdfs:range") || ocNode.CNType.Equals("owl:onDatatype"))
                        {
                            dpDetails.PType = ocNode.CNName;
                        }
                    }
                    sBuilder.Append("]");
                    dpDetails.PName = processedName;//oDPR.DProperty;
                    dpDetails.PExpression = sBuilder.ToString();
                    colNamesExprs.Add(dpDetails);
                }
            }

            //Step4: - If any property in CSV file not found, then return saying that CSV file is not matched with selected DS

            //Here We need to check PBReasoner
            //Reasoner based on property name
            //PBReasoner pbReasoner = new PBReasoner();
            //pbReasoner.ReasonWithProperties(dbData, csvFPResult);

            if (foundNames.Values.Contains(false))
            {
                vResult.Reason = string.Format("Validation Failed. Reason: Number of Cols in data set specification from Ontology --> {0}, and in Data set -->{1} are not equal.", selectedDPs.Count, csvFPResult.NoOfCols);
                return vResult;
            }
            if (csvFPResult.NoOfCols != foundNames.Count)
            {
                vResult.Reason = string.Format("Validation Failed. Reason: Number of Cols in data set specification from Ontology --> {0}, and in Data set -->{1} are not equal.", selectedDPs.Count, csvFPResult.NoOfCols);
                return vResult;
            }
            //Step 5: Next check --> Check value of each col with ontology specification
            //1. getting name and value of each col
            //2. check with Range to see whether the calues are of correct type
            //3. Check with PRangeExpr to see whether the col values are in proper range.
            //4. Checking Unit? 
            //5. checking proper conversions and see adaptability? Ex: of the CSV contains seconds and Onotology spec expects minutes then we can conver seconds to min? 

            int propCount = 0;
            vResult.Validated = true;
            while(propCount < csvFPResult.NoOfCols)
            {
                List<String> colValues = new List<string>();
                foreach (string s in csvFPResult.FileContent)
                {
                    colValues.Add(s.Split(',')[propCount]);
                }
                DPropDetails dpDet = colNamesExprs.Find((cp) => { if (cp.PName.Trim().ToLower().Equals(colValues[0].Trim().ToLower())) { return true; } else { return false; } });
              
                if (!string.IsNullOrEmpty(dpDet.PExpression))
                    vResult.Validated = vResult.Validated & ValidateDataSets.ValidatedExpr(colValues, dpDet.PExpression, dpDet.PType, adValue);
                if (!vResult.Validated)
                {
                    vResult.Reason = string.Format("Validation Failed. Reason: Column Name {0} is not as per the expression specified in Ontology {1}.", colValues[0], dpDet.PExpression);
                    break;
                }
                propCount++;
            }
            if (string.IsNullOrEmpty(vResult.Reason))
                if (vResult.Validated)
                {
                    vResult.Reason = "Processed Successfully";
                    vResult.OClassName = selectedDSName;
                }
            return vResult;
        }

        /// <summary>
        /// This function replaces some chars in names
        /// _ is replaced by Space. because Protege replaces Space with _ Ex: S1 kva ==? S1_kva
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static string RepalceChars(string sName)
        {
            if (sName.Contains("_"))
                sName = sName.Replace("_", " ");
            return sName;
        }
        public static bool ValidatedExpr(List<string> colValues, string expr, string pType, bool adValue)
        {
            bool validatedRes = false;
            int stIndex = expr.IndexOf('[');
            StringBuilder rawExprBuilder = new StringBuilder();
            foreach (char c in expr.Substring(stIndex + 1))
            {
                if (c == ']')
                    break;
                rawExprBuilder.Append(c);
            }
            string rawExpr = rawExprBuilder.ToString().Trim();
            if (string.IsNullOrEmpty(rawExpr))
                validatedRes = true;
            else
            {
                string[] exprs = rawExpr.Split(',');
                foreach (string expression in exprs)
                {
                    string op = string.Empty;
                    string opVal = string.Empty;
                    if(!expression.Contains(' '))
                    {
                        string[] exprParts = ProcessExpression(expression.Trim());
                        if (exprParts.Length < 2)
                            return false;
                        op = exprParts[0];
                        opVal = exprParts[1];
                    }
                    else
                    {
                        op = expression.Trim().Split(' ')[0];
                        opVal = expression.Trim().Split(' ')[1];
                    }

                    if (op.Equals("<"))
                        validatedRes = ValidateLT(colValues, colValues[0], opVal, pType, adValue);
                    else if (op.Equals(">"))
                        validatedRes = ValidateGT(colValues, colValues[0], opVal, pType, adValue);
                    else if (op.Equals("<="))
                        validatedRes = ValidateLE(colValues, colValues[0], opVal, pType, adValue);
                    else if (op.Equals(">="))
                        validatedRes = ValidateGE(colValues, colValues[0], opVal, pType, adValue);
                    else if (op.Equals("!="))
                        validatedRes = ValidateNT(colValues, colValues[0], opVal, pType, adValue);
                    else if (op.Equals("=="))
                        validatedRes = ValidateEQ(colValues, colValues[0], opVal, pType, adValue);
                    else if (op.Equals("!"))
                        validatedRes = ValidateNotET(colValues, colValues[0], opVal, pType, adValue);
                }
            }
            return validatedRes;
        }
        public static string[] ProcessExpression(string expression)
        {
            List<string> exParts = new List<string>();
            StringBuilder sB = new StringBuilder();
            
            foreach (char c in expression)
            {
                if (!Char.IsDigit(c))
                    sB.Append(c);
                else
                {
                    if (sB.ToString().Equals(">"))
                    {
                        exParts.Add(sB.ToString());
                        sB.Clear();
                    }
                    else if (sB.ToString().Equals("<"))
                    {
                        exParts.Add(sB.ToString());
                        sB.Clear();
                    }
                    else if (sB.ToString().Equals("<="))
                    {
                        exParts.Add(sB.ToString());
                        sB.Clear();
                    }
                    else if (sB.ToString().Equals(">="))
                    {
                        exParts.Add(sB.ToString());
                        sB.Clear();
                    }
                    else if (sB.ToString().Equals("!="))
                    {
                        exParts.Add(sB.ToString());
                        sB.Clear();
                    }
                    else if (sB.ToString().Equals("=="))
                    {
                        exParts.Add(sB.ToString());
                        sB.Clear();
                    }
                    exParts.Add(c.ToString());
                }
            }
            
            return exParts.ToArray<string>();
        }
        public static string GetActualType(string uriType, DBData curDBData)
        {
            string pType = string.Empty;
            int pID = -1;
            string searchUri = string.Format("{0}/{1}", PrefixIRIs.GetIRIs(uriType.Split('#')[0]), uriType.Split('#')[1]);
            try
            {
                foreach (EntityData eData in curDBData.ClassData)
                {
                    if (eData.FullEntityName.Equals(searchUri))
                    {
                        pID = eData.ETID;
                        break;
                    }
                }
                foreach (EntityData eData in curDBData.PropertyData)
                {
                    if (eData.FullEntityName.Equals(searchUri))
                    {
                        pID = eData.ETID;
                        break;
                    }
                }
                foreach (EntityData eData in curDBData.EnumData)
                {
                    if (eData.FullEntityName.Equals(searchUri))
                    {
                        pID = eData.ETID;
                        break;
                    }
                }
                if(pID != -1)
                {
                    var rData = curDBData.RelationData.FindAll((r) => { if (r.SourceETID == pID) { return true; } else { return false; } });
                    foreach(RelationsData rD in rData)
                    {
                        string s = rD.RelNotes;
                        string[] relParts = s.Split('-')[1].Split('/');
                        pType = relParts[relParts.Length-1];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while getting Property ID details. Details are {0}", ex.Message));
            }
            return pType; 
        }
        public static bool ValidateGT(List<string> colValues, string colName, string opVal, string pType, bool adValue)
        {
            try
            {
                if (pType.ToLower().Equals("integer"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Int32.Parse(s) > compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("float"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(float.Parse(s) > compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("decimal"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Decimal.Parse(s) > compValue))
                            return false;
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateLT(List<string> colValues, string colName, string opVal, string pType, bool adValue)
        {
            try
            {
                if (pType.ToLower().Equals("integer"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Int32.Parse(s) < compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("float"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(float.Parse(s) < compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("decimal"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Decimal.Parse(s) < compValue))
                            return false;
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateGE(List<string> colValues, string colName, string opVal, string pType, bool adValue)
        {
            try
            {
                if (pType.ToLower().Equals("integer") || pType.ToLower().Contains("integer") || pType.ToLower().Equals("int") || pType.ToLower().Contains("int"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Int32.Parse(s) >= compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("float") || pType.ToLower().Contains("float"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(float.Parse(s) >= compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("decimal") || pType.ToLower().Contains("decimal") || pType.ToLower().Contains("double") || pType.ToLower().Equals("double"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Decimal.Parse(s) >= compValue))
                            return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateLE(List<string> colValues, string colName, string opVal, string pType, bool adValue)
        {
            try
            {
                if (pType.ToLower().Equals("integer"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Int32.Parse(s) <= compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("float"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(float.Parse(s) <= compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("decimal"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Decimal.Parse(s) <= compValue))
                            return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateNT(List<string> colValues, string colName, string opVal, string pType, bool adValue)
        {
            try
            {
                if (pType.ToLower().Equals("integer"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if ((Int32.Parse(s) == compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("float"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if ((float.Parse(s) == compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("decimal"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if ((Decimal.Parse(s) == compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("string"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if ((string.Equals(s, compValue)))
                            return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateEQ(List<string> colValues, string colName, string opVal, string pType, bool adValue)
        {
            try
            {
                if (pType.ToLower().Equals("integer"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Int32.Parse(s) == compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("float"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(float.Parse(s) == compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("decimal"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (adValue)
                            if (s.Trim().Contains("E") || s.Trim().Contains("e"))
                                continue;
                        if (!(Decimal.Parse(s) == compValue))
                            return false;
                    }
                    return true;
                }
                else if (pType.ToLower().Equals("string"))
                {
                    int compValue = Int32.Parse(opVal);
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (!(string.Equals(s, compValue)))
                            return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateNotET(List<string> colValues, string colName, string opVal, string pType, bool adValue)
        {
            try
            {
                if (pType.ToLower().Equals("string"))
                {
                    foreach (string s in colValues)
                    {
                        if (s.Trim().ToLower().Equals(colName.Trim().ToLower()))
                            continue;
                        if (string.IsNullOrEmpty(s))
                            return false;
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// hard coded but we need to get these from Onology OWL file.
    /// </summary>
    public static class PrefixIRIs
    {
        public static string GetIRIs(string uris)
        {
            if (uris.Trim().Equals("BaseIRI"))
            {
                return "http://bristol.ac.uk/sles/ontologies/User1/opendsssim/";
            }
            else if (uris.Trim().Equals("VersionIRI"))
            {
                return "http://bristol.ac.uk/sles/ontologies/User1/opendsssim/1.0."; ;
            }
            else if (uris.Trim().Equals("PrefixIRI1"))
            {
                return "http://bristol.ac.uk/sles/cim/v1.0/Operations/Generation/GenerationTrainingSimulation/OpenDSSData";
            }
            else if (uris.Trim().Equals("PrefixIRI2"))
            {
                return "http://bristol.ac.uk/sles/cim/v1.0/Operations/NamedValues/";
            }
            else if (uris.Trim().Equals("PrefixIRI3"))
            {
                return "http://bristol.ac.uk/sles/cim/v1.0/Operations/DataTypes/PrimitiveDataTypes";
            }
            else if (uris.Trim().Equals("PrefixIRI4"))
            {
                return "http://bristol.ac.uk/sles/cim/v1.0/Operations/DataTypes/ElectricParamsDataTypes";
            }
            else if (uris.Trim().Equals("PrefixIRI5"))
            {
                return "http://bristol.ac.uk/sles/cim/v1.1/Operations/Generation/GenerationTrainingSimulation/OpenDSSData";
            }
            else if (uris.Trim().Equals("PrefixIRI6"))
            {
                return "http://bristol.ac.uk/sles/cim/v1.1/Operations/Generation/GenerationTrainingSimulation/OpenLV";
            }
            return string.Empty;
        }
    }
}
