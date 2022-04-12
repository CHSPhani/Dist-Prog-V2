using DataSerailizer;
using Server.ChangeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Reasoner;
using Server.Models;

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
        public static List<string> dT;
        static ValidateDataSets()
        {
            dT = new List<string>();
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
        }
        
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
            ValidationResult vResult = new ValidationResult(); //return this

            //This is where the classification algorithm kicks in
            if (string.IsNullOrEmpty(indName) && string.IsNullOrEmpty(selectedDSName))
            {
                //Step1: use iCols, dbData, pResult and Classification algorithm to get a classified class name
                vResult = ValidateDataSets.ClassifyCSVBasedDataset(dbData, pResult, iCols);
            }
            else
            {
                Dictionary<string, bool> foundNames = new Dictionary<string, bool>();
                CSVFileProcessResult csvFPResult = pResult as CSVFileProcessResult;
                List<DPropDetails> colNamesExprs = new List<DPropDetails>();
                foundNames = TryMatchwithRestrictions(selectedDSName, dbData, csvFPResult, ref colNamesExprs);
                
                //Step4: - If any property in CSV file not found, then return saying that CSV file is not matched with selected DS
                
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
                
                //Obtain vResult
                vResult = ValidateCSVSets(csvFPResult, colNamesExprs, adValue);

                //Return Result
                if (string.IsNullOrEmpty(vResult.Reason) || vResult.Reason.Equals("Processed Successfully"))
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
        public static ValidationResult ClassifyCSVBasedDataset(DBData dbData, IFileProcessResult pResult, bool iCols)
        {
            //settings
            List<char> ParseChars = new List<char>();
            ParseChars.Add('/');
            //Return value
            ValidationResult vRes = new ValidationResult();
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
                        List<string> matchedCls = new List<string>();
                        foreach(NewMatchDataCls s1 in res)
                        {
                            string s2 = s1.CName;
                            string s3 = s1.MatchedName;
                            if (s3.Contains(':'))
                            {
                                if (!matchedCls.Contains(s3.Split(':')[0]))
                                {
                                    matchedCls.Add(s3.Split(':')[0]);
                                }
                            }
                            else
                            {
                                if (!matchedCls.Contains(s3))
                                {
                                    matchedCls.Add(s3);
                                }
                            }
                        }
                        ValidationFeedbackModel vfModel = new ValidationFeedbackModel(matchedCls);
                        ValidationFeedback vF = new ValidationFeedback(vfModel);
                        vF.ShowDialog();
                        string selectedCls = vfModel.SelectedItem;
                        //Call Match function that tries with Restrictions defined in selectedCls.
                        List<DPropDetails> colNamesExprs = new List<DPropDetails>();
                        foundNames = TryMatchwithRestrictions(selectedCls, dbData, csvFPResult, ref colNamesExprs); //Why I am doing that
                        if (foundNames.Values.Contains(false))
                        {
                            //Need to create a new screen that helps in creating Ontology equivalent class.
                            OntoModificationModel oModel = new OntoModificationModel(dbData);
                            oModel.NewClsName = string.Format("{0}_{1}", selectedCls, "inst");
                            oModel.EquivalentclsName = selectedCls;
                            foreach(string es in csvFPResult.ColNames)
                            {
                                oModel.PropertiesNames.Add(es);
                            }
                            foreach(string ds in dT)
                            {
                                oModel.DTyps.Add(ds);
                            }
                          
                            oModel.CreateDPSets();
                            //ow show the screen and get modified
                            OntoModificationWindow omWindow = new OntoModificationWindow(oModel);
                            omWindow.ShowDialog();
                            //Now we can have class and data properties 
                            //we can add an equivalent to selectedCls.
                            //CreateOntEgClassV1(dbData, selectedCls, oModel, true); //I used this for testing. 
                            //create colExpres
                            foreach(DPropperties dp in oModel.DPrps)
                            {
                                colNamesExprs.Add(new DPropDetails() { PType = dp.SelDpName, PExpression = dp.DpExp, PName = dp.DPName });
                            }
                            //Validate
                            vRes = ValidateCSVSets(csvFPResult, colNamesExprs, true, oModel.NewClsName);
                            if (vRes.Reason.ToLower().Contains("processed successfully"))
                            {
                                vRes.OMModel = oModel;
                                vRes.IsODataLinked = true;
                            }
                        }
                    }
                }
                else
                {
                    //Ask user to create semantic structure manually and come back to validate thier datasets
                    //Need to create a new screen that helps in creating Ontology equivalent class.
                    List<DPropDetails> colNamesExprs = new List<DPropDetails>();
                    OntoModificationModel oModel = new OntoModificationModel(dbData);
                    oModel.NewClsName = string.Empty;// string.Format("{0}_{1}", selectedCls, "inst");
                    oModel.EquivalentclsName = string.Empty;  //selectedCls;
                    foreach (string es in csvFPResult.ColNames)
                    {
                        oModel.PropertiesNames.Add(es);
                    }
                    foreach (string ds in dT)
                    {
                        oModel.DTyps.Add(ds);
                    }
                    oModel.CreateDPSets();
                    //ow show the screen and get modified
                    GenOntoModificationWindow omWindow = new GenOntoModificationWindow(oModel);
                    omWindow.ShowDialog();
                    
                    vRes = ValidateCSVSets(csvFPResult, colNamesExprs, true, oModel.NewClsName);
                    if (vRes.Reason.ToLower().Contains("processed successfully"))
                    {
                        vRes.OMModel = oModel;
                        if (string.IsNullOrEmpty(oModel.EquivalentclsName))
                            vRes.IsODataLinked = false;
                        else
                            vRes.IsODataLinked = true;
                    }
                }
            }
            else if (unmatchedColNames.Count == 0)
            {
                //All properties are matched. 
                //Usecase1: Similar datasets would have been validated and corresponding semantic structure created OR
                //Usecase2: Incidentally (Accidentally) the dataset is matched with one of existing semantic structure 
                Dictionary<string, string> selectedClasses = new Dictionary<string, string>();
                if(suitableCN.Count != 0)
                {
                    foreach(string s in suitableCN)
                    {
                        string nName = dbData.OwlData.RDFG.GetExactNodeName(s);
                        List<string> routgoing = dbData.OwlData.RDFG.GetEdgesForNode(s);
                        List<string> rincoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(s);
                        if (rincoming.Count >= 1)
                        {
                            selectedClasses[s] = rincoming[0];
                        }
                        else
                        {
                            //we dont have any selectedClass. what to do
                        }
                    }
                }
                List<string> sCls = new List<string>();
                foreach (string scls in selectedClasses.Values)
                {
                    if (!sCls.Contains(scls))
                        sCls.Add(scls);
                }
                if (sCls.Count >= 1) //Sanity Check
                {
                    //Ideal case we found that all run time characteristics are matched with only one class
                    //Step1: Treat as known case 
                    List<DPropDetails> colNamesExprs = new List<DPropDetails>();
                    foundNames = TryMatchwithRestrictions(sCls[0], dbData, csvFPResult, ref colNamesExprs);
                    vRes = ValidateCSVSets(csvFPResult, colNamesExprs, true, sCls[0]);
                }
                else
                {
                    //We dont have any selected classes..
                }
            }
            else
            {
                //Hybrid case..Some found some not found
                //Do we have to create new derived??
                //But must return VRes from here
                string selectedCls = string.Empty;
                Dictionary<string, string> selectedClasses = new Dictionary<string, string>();
                if (suitableCN.Count != 0)
                {
                    foreach (string s in suitableCN)
                    {
                        string nName = dbData.OwlData.RDFG.GetExactNodeName(s);
                        List<string> routgoing = dbData.OwlData.RDFG.GetEdgesForNode(s);
                        List<string> rincoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(s);
                        if (rincoming.Count >= 1)
                        {
                            selectedClasses[s] = rincoming[0];
                        }
                        else
                        {
                            //we dont have any selectClass Hence we cant do this.
                        }
                    }
                }
                List<string> sCls = new List<string>();
                foreach (string scls in selectedClasses.Values)
                {
                    if (!sCls.Contains(scls))
                        sCls.Add(scls);
                }
                List<DPropDetails> colNamesExprs = new List<DPropDetails>();
                foundNames = TryMatchwithRestrictions(sCls[0], dbData, csvFPResult, ref colNamesExprs); 

                List<string> exisNames = new List<string>();
                OntoModificationModel oModel = new OntoModificationModel(dbData);
                oModel.NewClsName = string.Empty;
                oModel.EquivalentclsName = string.Empty;
                foreach (string es in csvFPResult.ColNames)
                {
                    if (!foundNames[es])
                        oModel.PropertiesNames.Add(es);
                    else
                        exisNames.Add(es);
                }
                foreach (string ds in dT)
                {
                    oModel.DTyps.Add(ds);
                }
                oModel.CreateDPSets();
                //ow show the screen and get modified
                GenOntoModificationWindow omWindow = new GenOntoModificationWindow(oModel);
                omWindow.ShowDialog();

                // Validate
                vRes = ValidateCSVSets(csvFPResult, colNamesExprs, true, oModel.NewClsName);
                if (vRes.Reason.ToLower().Contains("processed successfully"))
                {
                    vRes.OMModel = oModel;
                    if (string.IsNullOrEmpty(oModel.EquivalentclsName))
                        vRes.IsODataLinked = false;
                    else
                        vRes.IsODataLinked = true;
                }
                //add existing properties
                foreach (string exis in exisNames)
                {
                    oModel.CreateDPSets(exis);
                }
            }
            return vRes;
        }
        public static bool CreateOntEgClassV1(DBData dbData, string selectedCls, OntoModificationModel oModel, bool isIndCls)
        {
            try
            {
                //Process For each entry in oModel
                //Step1.1: See whether the Class is already there using ExactNodeName and GetNodeDetails. If there get that (Cls1)
                //Step1.2: if not Create Semantic Structure for Class (Cls1)
                //Step2.1 if newly created then 
                //Step2.2: Get the Base class for Cat (Units/EngineeringUnit) using ExactNodeName and GetNodeDetails  (BaseCls)
                //Step2.3: Link Cls1-BaseCls using SStrType.SubClassOf 
                //Step4: Check SelEqName is empty or not to see whether we need to create Equivalent Class
                //Step5: Get the class to use Equivalent class using ExactNodeName and GetNodeDetails (EquiCls)
                //Step6: Link Cls1 -EquiCls using SStrType.EquivalentClass
                //Step7.1 Check DataProperty DP1
                //Step7.2: if not then Create DataProperty DP1
                //Step7.3: Link Cls1 - DP1 using sd.SSType (Use the logic in Forloop in old 
                //Step7.4: Add Exp (Use logic in old)

                //Pre-DS for maintenance
                Dictionary<string, string> createdCls = new Dictionary<string, string>();
                List<SemanticStructure> ssExp = new List<SemanticStructure>();
                //Step-0: Create Class Which is root all the work in this function
                SemanticStructure src = new SemanticStructure() { SSName = string.Format("{0}", oModel.NewClsName), SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2#" };
                //Step2.1 add details
                dbData.OwlData.RDFG.AddNode(src.ToString());
                dbData.OwlData.RDFG.AddEntryToNODetail(src.ToString(), src);
                if (isIndCls)
                {
                    //Step2:- get exact name and corresponding semantic structure
                    string nName = dbData.OwlData.RDFG.GetExactNodeName(oModel.EquivalentclsName);
                    SemanticStructure ss = dbData.OwlData.RDFG.GetNodeDetails(oModel.EquivalentclsName);
                    //Step2.2 connect parent node and new child(equivalent) node
                    string edKey1 = string.Format("{0}-{1}", ss.SSName, src.SSName);
                    if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                        dbData.OwlData.RDFG.EdgeData[edKey1] = SStrType.EquivalentClass.ToString();
                    //adding edge
                    dbData.OwlData.RDFG.AddEdge(ss.ToString(), src.ToString());
                }
                //Class Done Now let us do rest of thing..
                foreach (DPropperties dP in oModel.DPrps)
                {
                    //Step1.1
                    //Creating sub class name
                    string mcName = string.Format("C{0}", dP.DPName);
                    string catName = string.Empty;
                    string cls1 = dbData.OwlData.RDFG.GetExactNodeName(mcName);
                    SemanticStructure sc = null;
                    sc = dbData.OwlData.RDFG.GetNodeDetails(mcName); //cls1
                    if (sc == null)
                    {
                        //Step1.2
                        sc = new SemanticStructure() { SSName = string.Format("{0}", mcName), SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2#" };
                        //Add to Graph
                        dbData.OwlData.RDFG.AddNode(sc.ToString());
                        dbData.OwlData.RDFG.AddEntryToNODetail(sc.ToString(), sc);
                        //Step 2.1 and Step2.2
                        string cat1 = dbData.OwlData.RDFG.GetExactNodeName(dP.SelCatName); //EngineeringUnit and Units are already there. 
                        SemanticStructure scat = dbData.OwlData.RDFG.GetNodeDetails(dP.SelCatName);
                        //Step2.3 Linking
                        string scat1 = string.Format("{0}-{1}", sc.SSName, scat.SSName);
                        if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(scat1))
                            dbData.OwlData.RDFG.EdgeData[scat1] = SStrType.SubClassOf.ToString();
                        dbData.OwlData.RDFG.AddEdge(sc.ToString(), scat.ToString()); //interchange2
                        catName = scat.SSName;
                        //Step4
                        if (!string.IsNullOrEmpty(dP.SelEqName) && !dP.SelEqName.ToLower().Equals("none"))
                        {
                            //Step5
                            string ceqn = string.Empty;
                            if (dP.SelEqName.Contains(':'))
                                ceqn = dbData.OwlData.RDFG.GetExactNodeName(dP.SelEqName.Split(':')[0]); //Equivalent class must be already there. 
                            SemanticStructure sceqn = dbData.OwlData.RDFG.GetNodeDetails(dP.SelEqName.Split(':')[0]);
                            //Step6 Linking
                            string seqn1 = string.Format("{0}-{1}", sceqn.SSName, sc.SSName);
                            if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(seqn1))
                                dbData.OwlData.RDFG.EdgeData[seqn1] = SStrType.EquivalentClass.ToString();
                            dbData.OwlData.RDFG.AddEdge(sceqn.ToString(), sc.ToString()); //interchange1
                        }
                    }
                    else
                    {
                        string actualName = dbData.OwlData.RDFG.GetExactNodeName(sc.SSName);
                        SemanticStructure sds = dbData.OwlData.RDFG.GetNodeDetails(sc.SSName);
                        List<string> sdsic = dbData.OwlData.RDFG.GetIncomingEdgesForNode(sc.SSName); //
                        List<string> sdsog = dbData.OwlData.RDFG.GetEdgesForNode(actualName); //
                        foreach(string sog in sdsog)
                        {
                            int indOfSep = sog.IndexOf('-');
                            string strKey = sog.Substring(0, indOfSep);
                            string strVal = sog.Substring(indOfSep + 1);
                            if (strVal.Split(':')[1].Equals("Class"))
                                catName = strVal.Split(':')[0];
                        }
                    }
                    //Step7.1
                    string sdp1 = dbData.OwlData.RDFG.GetExactNodeName(dP.DPIName);
                    SemanticStructure sd = null;
                    sd = dbData.OwlData.RDFG.GetNodeDetails(dP.DPIName);// sdp1);

                    if (sd == null || (sd != null && sd.SSType != SStrType.DatatypeProperty))
                    {
                        //Step7.2
                        sd = new SemanticStructure() { SSName = dP.DPIName, SSType = SStrType.DatatypeProperty, XMLURI = "http://www.w3.org/2001/XMLSchema#" };
                        //Step3.1 add data property details
                        dbData.OwlData.RDFG.AddNode(sd.ToString());
                        dbData.OwlData.RDFG.AddEntryToNODetail(sd.ToString(), sd);
                        //Step7.3 connect parent node and new chikd(equivalent) node
                        string edKey2 = string.Format("{0}-{1}", sc.SSName, sd.SSName);
                        if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                            dbData.OwlData.RDFG.EdgeData[edKey2] = sc.SSType.ToString();
                        //adding edge
                        dbData.OwlData.RDFG.AddEdge(sc.ToString(), sd.ToString());  //interchange3
                        //first check data type exists or not. Usually XSD datatypes exists
                        string nName2 = dbData.OwlData.RDFG.GetExactNodeName(dP.SelDpName);
                        SemanticStructure ss22 = dbData.OwlData.RDFG.GetNodeDetails(dP.SelDpName);// nName2.Split(':')[0]);
                        if (ss22 != null)
                        {
                            //Step3.2 connect parent node and new chikd(equivalent) node
                            string edKey3 = string.Format("{0}-{1}", sd.SSName, ss22.SSName);
                            if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey3))
                                dbData.OwlData.RDFG.EdgeData[edKey3] = ss22.SSType.ToString();
                            //adding edge
                            dbData.OwlData.RDFG.AddEdge(sd.ToString(), ss22.SSName); //interchange4
                        }
                        else
                        {
                            //adding new XSD data type strange this datatype is not there
                            SemanticStructure sr = new SemanticStructure() { SSName = dP.SelDpName, SSType = SStrType.Datatype, XMLURI = "http://www.w3.org/2001/XMLSchema#" };
                            //Step3.1.1 add data property details
                            dbData.OwlData.RDFG.AddNode(sr.ToString());
                            dbData.OwlData.RDFG.AddEntryToNODetail(sc.ToString(), sr);
                            //Step3.2.2 connect parent node and new chikd(equivalent) node
                            string edKey3 = string.Format("{0}-{1}", sc.SSName, sr.SSName);
                            if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey3))
                                dbData.OwlData.RDFG.EdgeData[edKey3] = sc.SSType.ToString();
                            //adding edge
                            dbData.OwlData.RDFG.AddEdge(sc.ToString(), sr.SSName); //interchange5
                        }
                        //Step7.4 
                        if (!string.IsNullOrEmpty(dP.DpExp))
                        {
                            SemanticStructure sex = new SemanticStructure() { SSName = string.Format("Expr>{0}", mcName), SSType = SStrType.RangeExpression, XMLURI = dP.DpExp };
                            ssExp.Add(sex);
                        }
                    }
                    createdCls[mcName] = catName;//adding class name after sucessfull Iteration.
                }
                //Here We need to add Object Properties
                //has_eng_unit 
                //has_measurement_unit
                //createdCls
                //src
                List<string> unitProps = new List<string>();
                List<string> eUnitProps = new List<string>();
                foreach (KeyValuePair<string, string> ccl in createdCls)
                {
                    if (ccl.Value.ToLower().Equals("units"))
                    {
                        unitProps.Add(ccl.Key);
                    }
                    else if (ccl.Value.ToLower().Equals("engineeringunit"))
                    {
                        eUnitProps.Add(ccl.Key);
                    }
                }
                //start adding unit
                if (unitProps.Count != 0)
                {
                    SemanticStructure ssu = dbData.OwlData.RDFG.GetNodeDetails("has_measurement_unit");
                    string edKey4 = string.Format("{0}-{1}", src.SSName, ssu.SSName);
                    if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey4))
                        dbData.OwlData.RDFG.EdgeData[edKey4] = src.SSType.ToString();
                    //add edge
                    dbData.OwlData.RDFG.AddEdge(src.ToString(), ssu.ToString());

                    foreach (string s in unitProps)
                    {
                        SemanticStructure su = dbData.OwlData.RDFG.GetNodeDetails(s);
                        string edKey5 = string.Format("{0}-{1}", src.SSName, su.SSName);
                        if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey5))
                            dbData.OwlData.RDFG.EdgeData[edKey5] = src.SSType.ToString();
                        //add edge
                        dbData.OwlData.RDFG.AddEdge(src.ToString(), su.ToString());
                    }
                }
                if (eUnitProps.Count != 0)
                {
                    SemanticStructure seu = dbData.OwlData.RDFG.GetNodeDetails("has_eng_unit");
                    string edKey41 = string.Format("{0}-{1}", src.SSName, seu.SSName);
                    if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey41))
                        dbData.OwlData.RDFG.EdgeData[edKey41] = src.SSType.ToString();
                    //add edge
                    dbData.OwlData.RDFG.AddEdge(src.ToString(), seu.ToString());

                    foreach (string s in eUnitProps)
                    {
                        SemanticStructure su = dbData.OwlData.RDFG.GetNodeDetails(s);
                        string edKey51 = string.Format("{0}-{1}", src.SSName, su.SSName);
                        if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey51))
                            dbData.OwlData.RDFG.EdgeData[edKey51] = src.SSType.ToString();
                        //add edge
                        dbData.OwlData.RDFG.AddEdge(src.ToString(), su.ToString());
                    }
                }
                //Now add Expressions Adding in the last because I have counter and while validating expressions
                //should be last.
                foreach(SemanticStructure sex in ssExp)
                {
                    //Step4.1 add Expression to data property details
                    dbData.OwlData.RDFG.AddNode(sex.ToString());
                    dbData.OwlData.RDFG.AddEntryToNODetail(sex.ToString(), sex);
                    string edKey4 = string.Format("{0}-{1}", src.SSName, sex.SSName);
                    if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey4))
                        dbData.OwlData.RDFG.EdgeData[edKey4] = src.SSType.ToString();
                    //add edge
                    dbData.OwlData.RDFG.AddEdge(src.ToString(), sex.ToString()); //changed to sc to sd then to src
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static Dictionary<string, bool> TryMatchwithRestrictions(string selectedDSName, DBData dbData, CSVFileProcessResult csvFPResult, ref List<DPropDetails> colNamesExprs)
        {
            //CSVFileProcessResult csvFPResult = pResult as CSVFileProcessResult;
            Dictionary<string, bool> foundNames = new Dictionary<string, bool>();

            //Step1:- To get all properties.
            string newName = string.Empty;
            if (selectedDSName.Contains(":"))
            {
                newName = selectedDSName.Split(':')[0];
            }
            else
            {
                newName = selectedDSName;
            }
            string nName = dbData.OwlData.RDFG.GetExactNodeName(newName);
            List<string> routgoing = dbData.OwlData.RDFG.GetEdgesForNode(nName);
            List<string> rincoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(nName);
            SortedList<int, string> nsList = new SortedList<int, string>();
            foreach (string s in routgoing)
            {
                int indOfSep = s.IndexOf('-');
                string strKey = s.Substring(0, indOfSep);
                string strVal = s.Substring(indOfSep + 1);
                nsList[Int32.Parse(strKey)] = strVal;
                //nsList[Int32.Parse(s.Split('-')[0])] = s.Split('-')[1];
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
            List<string> usedOnes = new List<string>();
            foreach (string sno in nOutgoing)
            {
                int indOfSep = sno.IndexOf('-');
                string strKey = sno.Substring(0, indOfSep);
                string nosr = sno.Substring(indOfSep + 1);
                if (nosr.Split(':')[1].ToLower().Equals("instance"))//removed .Split('-')[1] because I processed - already
                {
                    continue;
                }
                if (nosr.Split(':')[1].ToLower().Equals("rangeexpression")) //removed 
                {
                    string cStr = nosr.Split(':')[0]; //.Split('-')[1]
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
                        else if (pName.ToLower().Equals(dp.PName.ToLower()) || pName.ToLower().Contains(dp.PName.ToLower()))
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
                        SemanticStructure ssN = dbData.OwlData.RDFG.NODetails[nName2];
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
                else if (nosr.Split(':')[1].ToLower().Equals("class")) //removed Split('-')[1].
                {
                    string cNm = nosr;//.Split('-')[1];
                    string sNm2 = nosr.Split(':')[0];
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
                                else if(s.ToLower().Contains(ks.ToLower())) //This would typically the case when I add new semantic str using software
                                {
                                    DPropDetails dpDetails = new DPropDetails();
                                    string snName = dbData.OwlData.RDFG.GetExactNodeName(ks);
                                    SemanticStructure sNm = dbData.OwlData.RDFG.NODetails[snName];
                                    List<string> ogn = dbData.OwlData.RDFG.GetEdgesForNode(snName);
                                    List<string> ign = dbData.OwlData.RDFG.GetIncomingEdgesForNode(snName);
                                    //Here we process ogn instead of ign as in the above case
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
                                    else
                                    {
                                        //No need to process ign if thats the case let us see
                                        //if (ign.Count > 0)
                                        //{
                                        //    foreach (string igns in ign)
                                        //    {
                                        //        SemanticStructure signs = dbData.OwlData.RDFG.NODetails[igns];
                                        //        List<string> ogns = dbData.OwlData.RDFG.GetEdgesForNode(igns);
                                        //        List<string> ignss = dbData.OwlData.RDFG.GetIncomingEdgesForNode(igns);
                                        //        var secResult = ogns.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
                                        //        if (secResult.Count == 1)
                                        //        {
                                        //            dpDetails.PType = secResult[0].Split('-')[1].Split(':')[0];
                                        //        }
                                        //        else
                                        //        { }
                                        //    }
                                        //}
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
            return foundNames;
        }
        public static ValidationResult ValidateCSVSets(CSVFileProcessResult csvFPResult, List<DPropDetails> colNamesExprs, bool adValue)
        {
            return ValidateCSVSets(csvFPResult, colNamesExprs, adValue, string.Empty);
        }
        public static ValidationResult ValidateCSVSets(CSVFileProcessResult csvFPResult, List<DPropDetails> colNamesExprs, bool adValue, string selectedDSName)
        {
            ValidationResult vResult = new ValidationResult();
            int propCount = 0;
            vResult.Validated = true;
            while (propCount < csvFPResult.NoOfCols)
            {
                List<String> colValues = new List<string>();
                foreach (string s in csvFPResult.FileContent)
                {
                    colValues.Add(s.Split(',')[propCount]);
                }
                if (colNamesExprs.Count > 0)
                {
                    DPropDetails dpDet = null;
                    // previousnly i was using lambda exp. but i changed to loop.
                    // colNamesExprs.Find((cp) => { if (cp.PName.Trim().ToLower().Equals(colValues[0].Trim().ToLower()) || (colValues[0].Trim().ToLower().Contains(cp.PName.Trim().ToLower()))) { return true; } else { return false; } });
                    foreach(DPropDetails cne in colNamesExprs)
                    {
                        string cn = colValues[0];
                        
                        if( cn.Equals(cne.PName)) //cn.Contains(cne.PName) ||
                        {
                            dpDet = cne;
                            break;
                        }
                    }
                    if (dpDet != null)
                    {
                        if (!string.IsNullOrEmpty(dpDet.PExpression))
                            vResult.Validated = vResult.Validated & ValidateDataSets.ValidatedExpr(colValues, dpDet.PExpression, dpDet.PType, adValue);
                    }
                    if (!vResult.Validated)
                    {
                        vResult.Reason = string.Format("Validation Failed. Reason: Column Name {0} is not as per the expression specified in Ontology {1}.", colValues[0], dpDet.PExpression);
                        break;
                    }
                }
                propCount++;
            }
            if (string.IsNullOrEmpty(vResult.Reason))
                if (vResult.Validated)
                {
                    vResult.Reason = "Processed Successfully";
                    vResult.OClassName = selectedDSName;
                    vResult.InstName = string.Empty; //No instance as we add a new equiv class.
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
            if (string.IsNullOrEmpty(pType))
                return validatedRes;
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


#region OldCode
#region Converted as function hence commented 
//CSVFileProcessResult csvFPResult = pResult as CSVFileProcessResult;
//Dictionary<string, bool> foundNames = new Dictionary<string, bool>();

////Step1:- To get all properties.
//string nName = dbData.OwlData.RDFG.GetExactNodeName(selectedDSName);
//List<string> routgoing = dbData.OwlData.RDFG.GetEdgesForNode(nName);
//List<string> rincoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(nName);
//SortedList<int, string> nsList = new SortedList<int, string>();
//foreach (string s in routgoing)
//{
//    nsList[Int32.Parse(s.Split('-')[0])] = s.Split('-')[1];
//}
//List<string> nOutgoing = new List<string>();
//foreach (KeyValuePair<int, string> kvp in nsList)
//{
//    nOutgoing.Add(string.Format("{0}-{1}", kvp.Key, kvp.Value));
//}


////Step2:- This is seemingly not required but need to get all col names by trimming
//foreach (string s in csvFPResult.ColNames)
//{
//    foundNames[s.Trim()] = false;
//}

////Step3: Get the equivalent classes before-hand
//List<string> equivalentCls = new List<string>();
//foreach (KeyValuePair<string, string> ks in dbData.OwlData.RDFG.EdgeData)
//{
//    if (ks.Value.ToLower().Contains("equivalent"))
//    {
//        equivalentCls.Add(ks.Key);
//    }
//}

////Step4:- Process each DP for finding the subset of proerties and proceed further
////subset of properties is possible because ontologu can have more properties thatn specified in dataset.
//List<DPropDetails> colNamesExprs = new List<DPropDetails>();
//List<string> usedOnes = new List<string>();
//foreach (string nosr in nOutgoing)
//{
//    if (nosr.Split('-')[1].Split(':')[1].ToLower().Equals("instance"))
//    {
//        continue;
//    }
//    if (nosr.Split('-')[1].Split(':')[1].ToLower().Equals("rangeexpression"))
//    {
//        string cStr = nosr.Split('-')[1].Split(':')[0];
//        string pName = cStr.Split('>')[1];
//        string rnName = dbData.OwlData.RDFG.GetExactNodeName(cStr);
//        SemanticStructure rss = dbData.OwlData.RDFG.NODetails[rnName];
//        bool alreadyVisited = false;
//        foreach (DPropDetails dp in colNamesExprs)
//        {
//            if (dp.PName.ToLower().Equals(pName.ToLower()) || dp.PName.ToLower().Contains(pName.ToLower()))
//            {
//                dp.PExpression = rss.XMLURI;
//                alreadyVisited = true;
//                break;
//            }
//        }
//        if (!alreadyVisited)
//        {
//            DPropDetails dpDet = new DPropDetails();
//            dpDet.PName = pName;
//            dpDet.PExpression = rss.XMLURI;
//            string nName2 = dbData.OwlData.RDFG.GetExactNodeName(pName);// nosr.Split('-')[1].Split(':')[0]);
//            List<string> outgoing = dbData.OwlData.RDFG.GetEdgesForNode(nName2);
//            List<string> incoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(nName2);
//            var secResult = outgoing.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
//            if (secResult.Count == 1)
//            {
//                dpDet.PType = secResult[0].Split('-')[1].Split(':')[0];
//            }
//            else
//            { }
//            colNamesExprs.Add(dpDet);
//            List<string> visd = new List<string>();
//            foreach (string ss in foundNames.Keys)
//            {
//                if (ss.ToLower().Equals(pName.ToLower()))
//                    visd.Add(ss);
//            }
//            foreach (string sv in visd)
//                foundNames[sv] = true;
//        }
//    }
//    else if (nosr.Split('-')[1].Split(':')[1].ToLower().Equals("class"))
//    {
//        string cNm = nosr.Split('-')[1];
//        string sNm2 = nosr.Split('-')[1].Split(':')[0];
//        List<string> equis = new List<string>();
//        foreach (string s in equivalentCls)
//        {
//            if (s.Split('-').ToList<string>().Contains(sNm2))
//            {
//                equis.AddRange(s.Split('-').ToList<string>());
//            }
//        }
//        if (equis.Count == 0)
//        {
//            equis.Add(sNm2);
//        }
//        foreach (string s in equis)
//        {
//            foreach (string ks in foundNames.Keys)
//            {
//                if (!foundNames[ks])
//                {
//                    if (ks.ToLower().Contains(s.ToLower()))
//                    {
//                        DPropDetails dpDetails = new DPropDetails();
//                        string snName = dbData.OwlData.RDFG.GetExactNodeName(s);
//                        SemanticStructure sNm = dbData.OwlData.RDFG.NODetails[snName];
//                        List<string> ogn = dbData.OwlData.RDFG.GetEdgesForNode(snName);
//                        List<string> ign = dbData.OwlData.RDFG.GetIncomingEdgesForNode(snName);
//                        if (ign.Count > 0)
//                        {
//                            foreach (string igns in ign)
//                            {
//                                SemanticStructure signs = dbData.OwlData.RDFG.NODetails[igns];
//                                List<string> ogns = dbData.OwlData.RDFG.GetEdgesForNode(igns);
//                                List<string> ignss = dbData.OwlData.RDFG.GetIncomingEdgesForNode(igns);
//                                var secResult = ogns.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
//                                if (secResult.Count == 1)
//                                {
//                                    dpDetails.PType = secResult[0].Split('-')[1].Split(':')[0];
//                                }
//                                else
//                                { }
//                            }
//                        }
//                        else
//                        {
//                            if (ogn.Count > 0)
//                            {
//                                var secResult = ogn.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
//                                if (secResult.Count == 1)
//                                {
//                                    dpDetails.PType = secResult[0].Split('-')[1].Split(':')[0];
//                                }
//                                else
//                                { }
//                            }
//                        }
//                        dpDetails.PName = ks;
//                        colNamesExprs.Add(dpDetails);
//                        usedOnes.Add(ks);
//                    }
//                }
//            }
//            foreach (string kss in usedOnes)
//            {
//                foundNames[kss] = true;
//            }
//            usedOnes.Clear();
//        }
//    }
//}
#endregion
#region Commented because I created a new function
//int propCount = 0;
//vResult.Validated = true;
//while (propCount < csvFPResult.NoOfCols)
//{
//    List<String> colValues = new List<string>();
//    foreach (string s in csvFPResult.FileContent)
//    {
//        colValues.Add(s.Split(',')[propCount]);
//    }
//    if (colNamesExprs.Count > 0)
//    {
//        DPropDetails dpDet = colNamesExprs.Find((cp) => { if (cp.PName.Trim().ToLower().Equals(colValues[0].Trim().ToLower())) { return true; } else { return false; } });

//        if (!string.IsNullOrEmpty(dpDet.PExpression))
//            vResult.Validated = vResult.Validated & ValidateDataSets.ValidatedExpr(colValues, dpDet.PExpression, dpDet.PType, adValue);
//        if (!vResult.Validated)
//        {
//            vResult.Reason = string.Format("Validation Failed. Reason: Column Name {0} is not as per the expression specified in Ontology {1}.", colValues[0], dpDet.PExpression);
//            break;
//        }
//        propCount++;
//    }
//}
#endregion
#region Version 1 code for Creating Ont Changes
/*
 * public static bool CreateOntEqClass(DBData dbData, string selectedCls, OntoModificationModel oModel, bool isIndCls)
    {
        try
        {
            //Step1: Create semantic structure for new class name
            SemanticStructure sc = new SemanticStructure() { SSName = string.Format("{0}", oModel.NewClsName), SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2#" };
            //Step2.1 add details
            dbData.OwlData.RDFG.AddNode(sc.ToString());
            dbData.OwlData.RDFG.AddEntryToNODetail(sc.ToString(), sc);
            if (isIndCls)
            {
                //Step2:- get exact name and corresponding semantic structure
                string nName = dbData.OwlData.RDFG.GetExactNodeName(selectedCls);
                SemanticStructure ss = dbData.OwlData.RDFG.GetNodeDetails(selectedCls);
                //Step2.2 connect parent node and new chikd(equivalent) node
                string edKey1 = string.Format("{0}-{1}", ss.SSName, sc.SSName);
                if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                    dbData.OwlData.RDFG.EdgeData[edKey1] = SStrType.EquivalentClass.ToString();// sc.SSType.ToString();
                //adding edge
                dbData.OwlData.RDFG.AddEdge(ss.ToString(), sc.ToString());
            }
            //Step3: Creating new data Property for child node
            foreach (DPropperties dP in oModel.DPrps)
            {
                SemanticStructure sd = new SemanticStructure() { SSName = dP.DPName, SSType = SStrType.DatatypeProperty, XMLURI = "http://www.w3.org/2002/07/owl#" };
                //Step3.1 add data property details
                dbData.OwlData.RDFG.AddNode(sd.ToString());
                dbData.OwlData.RDFG.AddEntryToNODetail(sd.ToString(), sd);
                //Step3.2 connect parent node and new chikd(equivalent) node
                string edKey2 = string.Format("{0}-{1}", sc.SSName, sd.SSName);
                if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                    dbData.OwlData.RDFG.EdgeData[edKey2] = sd.SSType.ToString();
                //adding edge
                dbData.OwlData.RDFG.AddEdge(sc.ToString(), sd.ToString());
                //first check data type exists or not. Usually XSD datatypes exists
                string nName2 = dbData.OwlData.RDFG.GetExactNodeName(dP.SelDpName);
                SemanticStructure ss22 = dbData.OwlData.RDFG.GetNodeDetails(nName2.Split(':')[0]);
                if (ss22 != null)
                {
                    //Step3.2 connect parent node and new chikd(equivalent) node
                    string edKey3 = string.Format("{0}-{1}", sd.SSName, ss22.SSName);
                    if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey3))
                        dbData.OwlData.RDFG.EdgeData[edKey3] = ss22.SSType.ToString();
                    //adding edge
                    dbData.OwlData.RDFG.AddEdge(sd.ToString(), ss22.SSName);
                }
                else
                {
                    //adding new XSD data type strange this datatype is not there
                    SemanticStructure sr = new SemanticStructure() { SSName = dP.SelDpName, SSType = SStrType.Datatype, XMLURI = "http://www.w3.org/2001/XMLSchema#" };
                    //Step3.1.1 add data property details
                    dbData.OwlData.RDFG.AddNode(sr.ToString());
                    dbData.OwlData.RDFG.AddEntryToNODetail(sr.ToString(), sr);
                    //Step3.2.2 connect parent node and new chikd(equivalent) node
                    string edKey3 = string.Format("{0}-{1}", sc.SSName, sr.SSName);
                    if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey3))
                        dbData.OwlData.RDFG.EdgeData[edKey3] = sr.SSType.ToString();
                    //adding edge
                    dbData.OwlData.RDFG.AddEdge(sc.ToString(), sr.SSName);
                }
                //Expression Step -4
                if (!string.IsNullOrEmpty(dP.DpExp))
                {
                    SemanticStructure sex = new SemanticStructure() { SSName = string.Format("Expr>{0}", dP.DPName), SSType = SStrType.RangeExpression, XMLURI = dP.DpExp };
                    //Step4.1 add data property details
                    dbData.OwlData.RDFG.AddNode(sex.ToString());
                    dbData.OwlData.RDFG.AddEntryToNODetail(sex.ToString(), sex);
                    string edKey4 = string.Format("{0}-{1}", sc.SSName, sex.SSName);
                    if (!dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey4))
                        dbData.OwlData.RDFG.EdgeData[edKey4] = sc.SSType.ToString();
                    //add edge
                    dbData.OwlData.RDFG.AddEdge(sc.ToString(), sex.ToString()); 
                }
            }
        }
        catch(Exception ex)
        {
            return false;
        }
        return true;
    }
 * 
 * */
#endregion
#endregion