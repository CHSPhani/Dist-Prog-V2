using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSerailizer;
using Server.DataFileProcess;
using Server.DSystem;

namespace Server.ChangeRules
{
    public class ValidateRules
    {
        public static VoteType Validate(DBData nData, NodeMesaage nMsg)
        {
            bool condResult = true;
            ReadConditionRules rcRules = new ReadConditionRules();// @"C:\WorkRelated-Offline\Dist Prog V1\ResMngNetwork\Server\ConfigData\Rules.xml");
            if (nMsg.PCause == ProposalCause.ModifyDataProperty)
            {
                //Need to write modify property
                //    DMDataProperty oldDMProp = nMsg.OldDmDP;
                string newRange = nMsg.DataItems[0];
                string newExpr = nMsg.DataItems[1];
                string oldClsName = nMsg.DataItems[2];
                bool igCols = true;
                if (nMsg.DataItems[3].ToLower().Equals("true"))
                    igCols = true;
                else
                    igCols = false;
                ODataProperty oldDP = nMsg.OldDP;

                string oldExpr = string.Empty;
                string colName = string.Empty;

                bool relevantDataFound = false;
                if (nData.NodeData == null || nData.NodeData.Count == 0)
                {
                    //No data in the Node So this node can not evaluate the new expr and hence it says no-objection to change 
                    return VoteType.NoObjection;
                }
                else
                {
                    foreach (NodeData ndData in nData.NodeData)
                    {
                        string dmc = ndData.VerifiedDataSet;
                        if (!dmc.Trim().ToLower().Equals(oldClsName.Trim().ToLower()))
                            continue;
                        List<ODataProperty> selectedDPs = new List<ODataProperty>();
                        foreach (ODataProperty dp in nData.OwlData.OWLDataProperties)
                        {
                            var res = dp.DPChildNodes.FindAll((p) => { if ((p.CNType.Equals("rdfs:domain")) && (p.CNName.Equals(dmc))) { return true; } else { return false; } });
                            foreach (OChildNode ocNode in res)
                            {
                                selectedDPs.Add(dp);
                            }
                        }
                        var reqData = selectedDPs.Find((s) => { if (s.DProperty == oldDP.DProperty) return true; else return false; });

                        if (reqData != null)
                        {
                            relevantDataFound = true;
                            colName = reqData.DProperty;
                            
                            //Let us start
                            //req data : ndData, colName, oldDMProp, nData, newExpr
                            
                            //0. Validation result let me hope that i get a result
                            ValidationResult vResult = new ValidationResult();
                            
                            //1. Load CSV and prepare a table as do
                            string fileLocation = ndData.AbsLocation;
                            DataFilesProcess dfProcess = new DataFilesProcess(fileLocation);
                            IFileProcessResult pRes = dfProcess.ProcessFile();
                            
                            //2.Load CSV data
                            CSVFileProcessResult csvFPResult = pRes as CSVFileProcessResult;
                            List<String> colValues = new List<string>();
                            int propCount = 0;
                            bool propFound = false;
                            foreach (string cV in csvFPResult.FileContent[0].Split(',')) //kind of primitive code to get com number of desired col
                            {
                                if (cV.Trim().ToLower().Equals(colName.Trim().ToLower()))
                                {
                                    propFound = true;
                                    break;
                                }
                                propCount++;
                            }
                            if (!propFound)
                            {
                                if (!igCols)
                                {
                                    vResult.Reason = string.Format("Validation Failed. Reason: Column Name {0} is not found in File {1}.", colName, ndData.AbsLocation);
                                    //Validattion failed because col name is not present in the dataset being validated Ex: Ontology contains Second as name and Dataset contains t(Sec)
                                    //Need to abort as this is a structural difference.
                                    return VoteType.Abort;
                                }
                                else
                                {
                                    //return no objection
                                    return VoteType.NoObjection;
                                }
                            }

                            //3.Fill col values
                            foreach (string s1 in csvFPResult.FileContent) // Loading col name and values
                            {
                                colValues.Add(s1.Split(',')[propCount]);
                            }

                            //4. now we have the col name and values. let us get proerty type.
                            string propType = string.Empty;
                            foreach (OChildNode ocN in reqData.DPChildNodes)
                            {
                                if (ocN.CNType.Equals("owl:onDatatype") || ocN.CNType.Equals("rdfs:range"))
                                    propType = ocN.CNName;
                            }
                            
                            //5. Validate ColValues 
                            vResult.Validated = ValidateDataSets.ValidatedExpr(colValues, newExpr, propType, true); //adValue provided as true this is to accept default value.
                            if (!vResult.Validated)
                            {
                                vResult.Reason = string.Format("Validation Failed. Reason: Column Name {0} is not as per the expression specified in Ontology {1}.", colValues[0], newExpr);
                                //Validattion falied
                                return VoteType.NotAccepted;
                            }
                            else
                            {
                                vResult.Reason = string.Format("Validation Success. Column Name {0} is validated as per the expression specified in Change {1}.", colValues[0], newExpr);
                                return VoteType.Accepted;
                            }
                        }
                        if (!relevantDataFound)
                        {
                            //Node has local data but no relevant data set found in the context of modification. hence say no objection
                            return VoteType.NoObjection;
                        }
                    }
                }
            }
            else if (nMsg.PCause == ProposalCause.NewOntology)
            {
                return VoteType.Accepted;//What needs to be validated when some one wants to upload new ontology? Thats why just say yes.
            }
            else if (nMsg.PCause == ProposalCause.NewDataProperty)
            {
                return VoteType.Accepted;//What needs to be validated when some one wants to add a data property for an class in an ontology? Thats why just say yes.
            }
            else if (nMsg.PCause == ProposalCause.NewObjectProperty)
            {
                return VoteType.Accepted;//What needs to be validated when some one wants to add an equivalent object property for an class in an ontology? Thats why just say yes.
            }
            else if (nMsg.PCause == ProposalCause.NewOClass)
            {
                string newCls = nMsg.DataItems[0];
                OClass oCl = new OClass(newCls);
                var oc = nData.OwlData.OWLClasses.Find((o) => { if (o.CName.Equals(newCls)) { return true; } else { return false; } });

                if (oc != null)
                {
                    return VoteType.NotAccepted;
                }
                return VoteType.Accepted;//What needs to be validated when some one wants to add an new Ontology class for an class in an ontology? Thats why just say yes.
            }
            else
            {
                Rule reqRule = rcRules.Rules.Find((s) => { if (ValidateRules.GetCause(s.Cause) == nMsg.PCause & ValidateRules.GetType(s.Type) == nMsg.PTYpe) { return true; } else { return false; } });
                if (reqRule == null)
                    return VoteType.NotAccepted;
                else
                {
                    if (nMsg.PCause == ProposalCause.NewPackage)
                    {
                        object res = null;
                        foreach (string s in nMsg.DataItems)
                        {
                            res = AdaptRules.AdaptRulesForCSharp(nData, nMsg.PCause, nMsg.PTYpe, "nonempty", s);
                            if (res != null)
                                condResult = condResult & (bool)(res);
                        }
                        res = AdaptRules.AdaptRulesForCSharp(nData, nMsg.PCause, nMsg.PTYpe, "unique", nMsg.DataItems[1]);
                        if (res != null)
                            condResult = condResult & (bool)(res);

                    }
                    if (nMsg.PCause == ProposalCause.NewClass)
                    {
                        object res = null;
                        foreach (string s in nMsg.DataItems)
                        {
                            res = AdaptRules.AdaptRulesForCSharp(nData, nMsg.PCause, nMsg.PTYpe, "nonempty", s);
                            if (res != null)
                                condResult = condResult & (bool)(res);
                        }
                        res = AdaptRules.AdaptRulesForCSharp(nData, nMsg.PCause, nMsg.PTYpe, "unique", nMsg.DataItems[0]);
                        if (res != null)
                            condResult = condResult & (bool)(res);
                    }
                    if (nMsg.PCause == ProposalCause.NewProperty)
                    {
                        object res = null;
                        foreach (string s in nMsg.DataItems)
                        {
                            res = AdaptRules.AdaptRulesForCSharp(nData, nMsg.PCause, nMsg.PTYpe, "nonempty", s);
                            if (res != null)
                                condResult = condResult & (bool)(res);
                        }
                        res = AdaptRules.AdaptRulesForCSharp(nData, nMsg.PCause, nMsg.PTYpe, "unique", nMsg.DataItems[0]);
                        if (res != null)
                            condResult = condResult & (bool)(res);
                    }
                }
            }
            if (condResult == true)
                return VoteType.Accepted;
            else if (condResult == false)
                return VoteType.NotAccepted;
            return VoteType.Accepted; //I am sending accepted as default value because i wanted to write optimistic programs.
        }

        public static void CreateChildNodesForExpr(string expr, string selectedDT, ref ODataProperty oDp)
        {
            
            int stIndex = expr.IndexOf('[');
            StringBuilder rawExprBuilder = new StringBuilder();
            foreach (char c in expr.Substring(stIndex + 1))
            {
                if (c == ']')
                    break;
                rawExprBuilder.Append(c);
            }
            string rawExpr = rawExprBuilder.ToString().Trim();
            if (!string.IsNullOrEmpty(rawExpr))
            { 
                string[] exprs = rawExpr.Split(',');
                foreach (string expression in exprs)
                {
                    string op = string.Empty;
                    string opVal = string.Empty;
                    if (!expression.Trim().Contains(' '))
                    {
                        string[] exprParts = ValidateDataSets.ProcessExpression(expression.Trim());
                        if (exprParts.Length < 2)
                            return;
                        op = exprParts[0];
                        opVal = exprParts[1];
                    }
                    else
                    {
                        op = expression.Trim().Split(' ')[0];
                        opVal = expression.Trim().Split(' ')[1];
                    }
                    OChildNode ocNode3 = new OChildNode(); //Range
                    ocNode3.CNBaseURI = "http://www.w3.org/2001/XMLSchema";
                    ocNode3.CNName = selectedDT;
                    if (op.Equals("<"))
                    {
                        ocNode3.CNType = "xsd:minExclusive";
                        ocNode3.CNSpecialValue = opVal;

                    }
                    else if (op.Equals(">"))
                    {
                        ocNode3.CNType = "xsd:maxExclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    else if (op.Equals("<="))
                    {
                        ocNode3.CNType = "xsd:minInclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    else if (op.Equals(">="))
                    {
                        ocNode3.CNType = "xsd:maxInclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    else if (op.Equals("minLength"))
                    {
                        ocNode3.CNType = "xsd:minInclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    oDp.DPChildNodes.Add(ocNode3);
                }
            }
        }

        public static void UpdateChildNodesForExpr(string expr, string selectedDT, ref ODataProperty oDp)
        {
            int stIndex = expr.IndexOf('[');
            StringBuilder rawExprBuilder = new StringBuilder();
            foreach (char c in expr.Substring(stIndex + 1))
            {
                if (c == ']')
                    break;
                rawExprBuilder.Append(c);
            }
            string rawExpr = rawExprBuilder.ToString().Trim();
            List<OChildNode> ocNodes = new List<OChildNode>();
            
            if (!string.IsNullOrEmpty(rawExpr))
            {
                string[] exprs = rawExpr.Split(',');
                foreach (string expression in exprs)
                {
                    string op = string.Empty;
                    string opVal = string.Empty;
                    if (!expression.Trim().Contains(' '))
                    {
                        string[] exprParts = ValidateDataSets.ProcessExpression(expression.Trim());
                        if (exprParts.Length < 2)
                            return;
                        op = exprParts[0];
                        opVal = exprParts[1];
                    }
                    else
                    {
                        op = expression.Trim().Split(' ')[0];
                        opVal = expression.Trim().Split(' ')[1];
                    }

                    OChildNode ocNode3 = new OChildNode(); //Range
                    ocNode3.CNBaseURI = "http://www.w3.org/2001/XMLSchema";
                    ocNode3.CNName = selectedDT;
                    if (op.Equals("<"))
                    {
                        ocNode3.CNType = "xsd:minExclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    else if (op.Equals(">"))
                    {
                        ocNode3.CNType = "xsd:maxExclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    else if (op.Equals("<="))
                    {
                        ocNode3.CNType = "xsd:minInclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    else if (op.Equals(">="))
                    {
                        ocNode3.CNType = "xsd:maxInclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    else if (op.Equals("minLength"))
                    {
                        ocNode3.CNType = "xsd:minInclusive";
                        ocNode3.CNSpecialValue = opVal;
                    }
                    ocNodes.Add(ocNode3);
                }
            }

            oDp.DPChildNodes.RemoveAll((cn) => { if (ValidateRules.IsOperator(cn.CNType)) { return true; } else { return false; } });
            foreach(OChildNode ocNode in ocNodes)
            {
                oDp.DPChildNodes.Add(ocNode);
            }
        }

        public static bool IsOperator(string cType)
        {
            if (cType.Equals("xsd:minExclusive") || cType.Equals("xsd:maxExclusive") || cType.Equals("xsd:minInclusive") || cType.Equals("xsd:maxInclusive"))
                return true;
            return false;
        }

        public static string GetOperator(string opName)
        {
            if (opName.Equals("xsd:minExclusive"))
                return "<";
            if (opName.Equals("xsd:maxExclusive"))
                return ">";
            if (opName.Equals("xsd:minInclusive"))
                return "<=";
            if (opName.Equals("xsd:maxInclusive"))
                return ">=";
            return string.Empty;
        }

        public static string GetOperatorName(string optr)
        {
            if (optr.Equals("<"))
                return "xsd:minExclusive";
            if (optr.Equals(">"))
                return "xsd:maxExclusive";
            if (optr.Equals("<="))
                return "xsd:minInclusive";
            if (optr.Equals(">="))
                return "xsd:maxInclusive";
            return string.Empty;
        }

        public static bool IsDataTypeExists(string dType)
        {
            List<string> dT = new List<string>();
            dT.Add("xsd:byte");
            dT.Add("xsd:boolean");
            dT.Add("xsd:int");
            dT.Add("xsd:integer");
            dT.Add("xsd:long");
            dT.Add("xsd:short");
            dT.Add("xsd:string");
            dT.Add("xsd:double");
            dT.Add("xsd:decimal");
            dT.Add("xsd:float");
            dT.Add("xsd:unsignedByte");
            dT.Add("xsd:unsignedInt");
            dT.Add("xsd:unsignedLong");
            dT.Add("xsd:unsignedShort");
            if (dT.Contains(dType))
                return true;
            return false;
        }

        public static ProposalCause GetCause(string strCause)
        {
            switch (strCause.ToLower())
            {
                case "newpackage":
                    {
                        return ProposalCause.NewPackage;
                    }
                case "newclass":
                    {
                        return ProposalCause.NewClass;
                    }
                case "newproperty":
                    {
                        return ProposalCause.NewProperty;
                    }
                case "newontology":
                    {
                        return ProposalCause.NewOntology;
                    }
                case "modifydataproperty":
                    {
                        return ProposalCause.ModifyDataProperty;
                    }
                case "modifyobjectproperty":
                    {
                        return ProposalCause.ModifyObjectProperty;
                    }
                default:
                    {
                        return ProposalCause.None;
                    }
            }
        }

        public static ProposalType GetType(string strType)
        {
            switch(strType.ToLower())
            {
                case "voting":
                    {
                        return ProposalType.Voting;
                    }
                case "transition":
                    {
                        return ProposalType.Transition;
                    }
                default:
                    {
                        return ProposalType.None;
                    }
            }
        }
    }
}
