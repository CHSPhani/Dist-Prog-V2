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

    public class NewMatchDataCls
    {
        public string CName { get; set; }
        public string MatchedName { get; set; }

        public NewMatchDataCls()
        {
            CName = string.Empty;
            MatchedName = string.Empty;
        }
    }
    public class ValidateDataSets
    {
        public static ValidationResult ValidateCSVDataSet(DBData dbData, string indName, IFileProcessResult pResult, bool adValue)
        {
            //Step1: First find string selectedDSName using indName
            string actInstName = dbData.OwlData.RDFG.GetExactNodeName(indName);
            SemanticStructure acss = dbData.OwlData.RDFG.NODetails[actInstName];
            List<string> acoutgoing = dbData.OwlData.RDFG.GetEdgesForNode(actInstName);
            List<string> acincoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(actInstName);
            if (acincoming.Count > 1)
                return new ValidationResult() { Reason = "Error .. There must be two classes to which this instance belongs" }; 
            string selectedDSName = acincoming[0].Split(':')[0]; //There should be only one incoming to instance
            //Step2 Then call ValidateCSVDataSet() below to get remaining function. 
            ValidationResult vRes = ValidateDataSets.ValidateCSVDataSet(dbData, indName, pResult, selectedDSName, true, false);
            return vRes;
        }
        
        public static ValidationResult ValidateCSVDataSet(DBData dbData, string indName, IFileProcessResult pResult, string selectedDSName, bool adValue, bool iCols)
        {
            List<string> dT = new List<string>();
            dT.Add("byte");
            dT.Add("boolean");
            dT.Add("int");
            dT.Add("integer");
            dT.Add("long");
            dT.Add("short");
            dT.Add("string");
            dT.Add("double");
            dT.Add("decimal");
            dT.Add("float");
            dT.Add("unsignedByte");
            dT.Add("unsignedInt");
            dT.Add("unsignedLong");
            dT.Add("unsignedShort");
            ValidationResult vResult = new ValidationResult(); //return this

            //This is where the classification algorithm kicks in
            if (string.IsNullOrEmpty(indName) && string.IsNullOrEmpty(selectedDSName))
            {
                //use iCols, dbData, pResult, 
                ValidateDataSets.ClassifyCSVBasedDataset(dbData, pResult, iCols);
            }
            else//Other wise
            {
                CSVFileProcessResult csvFPResult = pResult as CSVFileProcessResult;
                Dictionary<string, bool> foundNames = new Dictionary<string, bool>();

                //Step1:- To get all properties.
                string nName = dbData.OwlData.RDFG.GetExactNodeName(selectedDSName);
                List<string> routgoing = dbData.OwlData.RDFG.GetEdgesForNode(nName);
                List<string> rincoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(nName);
                SortedList<int, string> nsList = new SortedList<int, string>();
                foreach (string s in routgoing)
                {
                    nsList[Int32.Parse(s.Split('-')[0])] = s.Split('-')[1];
                }
                List<string> nOutgoing = new List<string>();
                foreach (KeyValuePair<int, string> kvp in nsList)
                {
                    nOutgoing.Add(string.Format("{0}-{1}", kvp.Key, kvp.Value));
                }


                //Step2:- This is seemingly not required but need to get all col names by trimming
                foreach (string s in csvFPResult.ColNames)
                {
                    foundNames[s.Trim()] = false;
                }

                //Step3: Get the equivalent classes before-hand
                List<string> equivalentCls = new List<string>();
                foreach (KeyValuePair<string, string> ks in dbData.OwlData.RDFG.EdgeData)
                {
                    if (ks.Value.ToLower().Contains("equivalent"))
                    {
                        equivalentCls.Add(ks.Key);
                    }
                }

                //Step4:- Process each DP for finding the subset of proerties and proceed further
                //subset of properties is possible because ontologu can have more properties thatn specified in dataset.
                List<DPropDetails> colNamesExprs = new List<DPropDetails>();
                List<string> usedOnes = new List<string>();
                foreach (string nosr in nOutgoing)
                {
                    if (nosr.Split('-')[1].Split(':')[1].ToLower().Equals("instance"))
                    {
                        continue;
                    }
                    if (nosr.Split('-')[1].Split(':')[1].ToLower().Equals("rangeexpression"))
                    {
                        string cStr = nosr.Split('-')[1].Split(':')[0];
                        string pName = cStr.Split('>')[1];
                        string rnName = dbData.OwlData.RDFG.GetExactNodeName(cStr);
                        SemanticStructure rss = dbData.OwlData.RDFG.NODetails[rnName];
                        bool alreadyVisited = false;
                        foreach (DPropDetails dp in colNamesExprs)
                        {
                            if (dp.PName.ToLower().Equals(pName.ToLower()) || dp.PName.ToLower().Contains(pName.ToLower()))
                            {
                                dp.PExpression = rss.XMLURI;
                                alreadyVisited = true;
                                break;
                            }
                        }
                        if (!alreadyVisited)
                        {
                            DPropDetails dpDet = new DPropDetails();
                            dpDet.PName = pName;
                            dpDet.PExpression = rss.XMLURI;
                            string nName2 = dbData.OwlData.RDFG.GetExactNodeName(pName);// nosr.Split('-')[1].Split(':')[0]);
                            List<string> outgoing = dbData.OwlData.RDFG.GetEdgesForNode(nName2);
                            List<string> incoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(nName2);
                            var secResult = outgoing.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
                            if (secResult.Count == 1)
                            {
                                dpDet.PType = secResult[0].Split('-')[1].Split(':')[0];
                            }
                            else
                            { }
                            colNamesExprs.Add(dpDet);
                            List<string> visd = new List<string>();
                            foreach (string ss in foundNames.Keys)
                            {
                                if (ss.ToLower().Equals(pName.ToLower()))
                                    visd.Add(ss);
                            }
                            foreach (string sv in visd)
                                foundNames[sv] = true;
                        }
                    }
                    else if (nosr.Split('-')[1].Split(':')[1].ToLower().Equals("class"))
                    {
                        string cNm = nosr.Split('-')[1];
                        string sNm2 = nosr.Split('-')[1].Split(':')[0];
                        List<string> equis = new List<string>();
                        foreach (string s in equivalentCls)
                        {
                            if (s.Split('-').ToList<string>().Contains(sNm2))
                            {
                                equis.AddRange(s.Split('-').ToList<string>());
                            }
                        }
                        if (equis.Count == 0)
                        {
                            equis.Add(sNm2);
                        }
                        foreach (string s in equis)
                        {
                            foreach (string ks in foundNames.Keys)
                            {
                                if (!foundNames[ks])
                                {
                                    if (ks.ToLower().Contains(s.ToLower()))
                                    {
                                        DPropDetails dpDetails = new DPropDetails();
                                        string snName = dbData.OwlData.RDFG.GetExactNodeName(s);
                                        SemanticStructure sNm = dbData.OwlData.RDFG.NODetails[snName];
                                        List<string> ogn = dbData.OwlData.RDFG.GetEdgesForNode(snName);
                                        List<string> ign = dbData.OwlData.RDFG.GetIncomingEdgesForNode(snName);
                                        if (ign.Count > 0)
                                        {
                                            foreach (string igns in ign)
                                            {
                                                SemanticStructure signs = dbData.OwlData.RDFG.NODetails[igns];
                                                List<string> ogns = dbData.OwlData.RDFG.GetEdgesForNode(igns);
                                                List<string> ignss = dbData.OwlData.RDFG.GetIncomingEdgesForNode(igns);
                                                var secResult = ogns.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
                                                if (secResult.Count == 1)
                                                {
                                                    dpDetails.PType = secResult[0].Split('-')[1].Split(':')[0];
                                                }
                                                else
                                                { }
                                            }
                                        }
                                        else
                                        {
                                            if (ogn.Count > 0)
                                            {
                                                var secResult = ogn.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
                                                if (secResult.Count == 1)
                                                {
                                                    dpDetails.PType = secResult[0].Split('-')[1].Split(':')[0];
                                                }
                                                else
                                                { }
                                            }
                                        }
                                        dpDetails.PName = ks;
                                        colNamesExprs.Add(dpDetails);
                                        usedOnes.Add(ks);
                                    }
                                }
                            }
                            foreach (string kss in usedOnes)
                            {
                                foundNames[kss] = true;
                            }
                            usedOnes.Clear();
                        }
                    }
                }

                //Step4: - If any property in CSV file not found, then return saying that CSV file is not matched with selected DS

                //Here We need to check PBReasoner
                //Reasoner based on property name
                //PBReasoner pbReasoner = new PBReasoner();
                //pbReasoner.ReasonWithProperties(dbData, csvFPResult);

                if (foundNames.Values.Contains(false))
                {
                    vResult.Reason = string.Format("Validation Failed. Reason: Number of Cols in data set specification from Ontology --> {0}, and in Data set -->{1} are not equal.", foundNames.Count, csvFPResult.NoOfCols);
                    return vResult;
                }
                if (csvFPResult.NoOfCols != foundNames.Count)
                {
                    vResult.Reason = string.Format("Validation Failed. Reason: Number of Cols in data set specification from Ontology --> {0}, and in Data set -->{1} are not equal.", foundNames.Count, csvFPResult.NoOfCols);
                    return vResult;
                }
                ////Step 5: Next check --> Check value of each col with ontology specification
                ////1. getting name and value of each col
                ////2. check with Range to see whether the calues are of correct type
                ////3. Check with PRangeExpr to see whether the col values are in proper range.
                ////4. Checking Unit? 
                ////5. checking proper conversions and see adaptability? Ex: of the CSV contains seconds and Onotology spec expects minutes then we can conver seconds to min? 

                int propCount = 0;
                vResult.Validated = true;
                while (propCount < csvFPResult.NoOfCols)
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
                        vResult.InstName = indName;
                    }
            }
            return vResult;
        }

        /// <summary>
        /// Implementing classification algorithm(??)
        /// </summary>
        /// <param name="dbData"></param>
        /// <param name="pResult"></param>
        /// <param name="iCols"></param>
        public static void ClassifyCSVBasedDataset(DBData dbData, IFileProcessResult pResult, bool iCols)
        {
            //settings
            List<char> ParseChars = new List<char>();
            ParseChars.Add('/');

            //Read CSV Files and get Cols
            CSVFileProcessResult csvFPResult = pResult as CSVFileProcessResult;
            Dictionary<string, bool> foundNames = new Dictionary<string, bool>();
            foreach (string s in csvFPResult.ColNames)
            {
                foundNames[s.Trim()] = false;
            }

            //Step1: Check whether colnames match with any of graph nodes.
            List<string> suitableCN = new List<string>();
            List<string> unmatchedColNames = new List<string>();
            foreach (string scName in foundNames.Keys)
            {
                var oC = dbData.OwlData.RDFG.NODetails.Keys.ToList<string>().Find((a) => { if (a.ToLower().Contains(scName.ToLower()) || scName.ToLower().Contains(a.ToLower())) { return true; } else { return false; } });
                if (oC != null)
                    suitableCN.Add(oC);
                else
                    unmatchedColNames.Add(scName);
            }
            //Step2: Process
            if (suitableCN.Count == 0)
            {
                //Absolutely no matches found..
                //We should create new semantic structures if iCols
                if (iCols)
                {
                    //Step2.1: Process all column names and see for any matches with existing semantic structure
                    foreach (char pc in ParseChars)
                    {
                        List<string> newColNames = new List<string>();
                        foreach (string scName in foundNames.Keys)
                        {
                            if (scName.Contains(pc))
                            {
                                foreach (string scn in scName.Split(pc))
                                {
                                    newColNames.Add(scn);
                                }
                            }
                            else
                            {
                                newColNames.Add(scName);
                            }
                        }
                        List<NewMatchDataCls> newMatches = new List<NewMatchDataCls>();
                        //we have new colNames. Lets check what are in Graph
                        foreach (string snm in newColNames)
                        {
                            var oC = dbData.OwlData.RDFG.NODetails.Keys.ToList<string>().FindAll((a) => { if (a.ToLower().Contains(snm.ToLower()) || snm.ToLower().Contains(a.ToLower())) { return true; } else { return false; } });
                            if (oC != null)
                            {
                                foreach (string soC in oC)
                                {
                                    newMatches.Add(new NewMatchDataCls() { CName = snm, MatchedName = soC });
                                }
                            }
                        }
                        var res = newMatches.FindAll((m) => { if (m.MatchedName.Split(':')[1].ToLower().Equals("class")) { return true; } else { return false; } });
                        foreach(NewMatchDataCls s1 in res)
                        {
                            string s2 = s1.CName;
                            string s3 = s1.MatchedName;
                        }

                        var res1 = newMatches.FindAll((m) => { if (m.CName.ToLower().Contains("vol")) { return true; } else { return false; } });
                        foreach (NewMatchDataCls s1 in res1)
                        {
                            string s2 = s1.CName;
                            string s3 = s1.MatchedName;
                        }
                    }
                }
                else
                {
                    //Ask user to create semantic structure manually and come back to validate thier datasets
                }
            }
            else if (unmatchedColNames.Count == 0)
            {
                //All properties are matched. so process
            }
            else
            {
                //Hybrid case..Some found some not found
                // Do we have to create new derived??
            }

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
