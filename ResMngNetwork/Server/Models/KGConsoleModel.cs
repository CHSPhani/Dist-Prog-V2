using ContractDataModels;
using DataSerailizer;
using Microsoft.Glee.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class KGConsoleModel : INotifyPropertyChanged, IObtainSearchResults
    {
        public static DBData dbData = null;

        string searchTerm;
        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }
            set
            {
                this.searchTerm = value;
                OnPropertyChanged("SearchTerm");
            }
        }

        bool includeSS;
        public bool IncludeSS
        {
            get
            {
                return this.includeSS;
            }
            set
            {
                this.includeSS = value;
                OnPropertyChanged("IncludeSS");
            }
        }

        bool includeII;
        public bool IncludeII
        {
            get
            {
                return this.includeII;
            }
            set
            {
                this.includeII = value;
                OnPropertyChanged("IncludeII");
            }
        }

        bool includeDS;
        public bool IncludeDS
        {
            get
            {
                return this.includeDS;
            }
            set
            {
                this.includeDS = value;
                OnPropertyChanged("IncludeDS");
            }
        }

        List<string> kgStats;
        public List<string> KGStats
        {
            get
            {
                return this.kgStats;
            }
            set
            {
                this.kgStats = value;
                OnPropertyChanged("KGStats");
            }
        }

        string searchRes;
        public string SearchRes
        {
            get
            {
                return this.searchRes;
            }
            set
            {
                this.searchRes = value;
                OnPropertyChanged("SearchRes");
            }
        }

        public KGConsoleModel()
        {
            if (KGConsoleModel.dbData != null)
            {
                this.IncludeSS = true;
                this.IncludeII = true;
                this.IncludeDS = true;

                List<string> ss = new List<string>();
                var GroupBySS = KGConsoleModel.dbData.OwlData.RDFG.NODetails.Values.GroupBy(s => s.SSType);
                foreach (var group in GroupBySS)
                {
                    string s1 = group.Key.ToString();
                    int c = group.Count();
                    ss.Add(string.Format("KG contains {1} number of {0} \n", s1, c));
                }
                this.KGStats = ss;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetSearchResults(string sTerm)
        {
            this.SearchTerm = sTerm;
            return this.GetSearchResults();
        }

        /// <summary>
        ///
        /// </summary>
        public string GetSearchResults()
        {
            List<SemanticStructure> classHierarchy = new List<SemanticStructure>();
            SemanticStructure curObj = KGConsoleModel.dbData.OwlData.RDFG.GetNodeDetails(this.searchTerm);

            if (curObj == null)
                curObj = KGConsoleModel.dbData.OwlData.RDFG.GetNodeDetailsW(this.searchTerm);
            
            if(curObj == null)
                return "Not able to find the requested item in knowledge Graph";
            
            classHierarchy.Add(curObj);

            string nName = KGConsoleModel.dbData.OwlData.RDFG.GetExactNodeName(curObj.SSName);
            if(string.IsNullOrEmpty(nName))
            {
                //Hopefully Individual.
                StringBuilder sb1 = new StringBuilder();
                //Construct result output
                sb1.Append("Search Results \n");
                sb1.Append(string.Format("SSName ={0}", curObj.SSName));
                sb1.Append("\n");
                sb1.Append(string.Format("SSType ={0}", curObj.SSType));
                sb1.Append("\n");
                sb1.Append(string.Format("XML URI ={0}", curObj.XMLURI));
                sb1.Append("\n");
                return sb1.ToString();
            }
            List<string> outgoing = KGConsoleModel.dbData.OwlData.RDFG.GetEdgesForNode(nName);
            List<string> incoming = KGConsoleModel.dbData.OwlData.RDFG.GetIncomingEdgesForNode(nName);
            List<string> nOutgoing = new List<string>();
            StringBuilder sb = new StringBuilder();

            if (outgoing.Count != 0 & incoming.Count != 0)
            {
                outgoing.Sort();

                SortedList<int, string> nsList = new SortedList<int, string>();
                foreach (string s in outgoing)
                {
                    nsList[Int32.Parse(s.Split('-')[0])] = s.Split('-')[1];
                }

                foreach (KeyValuePair<int, string> kvp in nsList)
                {
                    nOutgoing.Add(string.Format("{0}-{1}", kvp.Key, kvp.Value));
                }
                //Step3: Calculating data properties up in hierarchies 
                //Seems like not required to get all properties
                string reqParts = nOutgoing[0].Split('-')[1].Split(':')[0];
                string relation = string.Empty;
                if (KGConsoleModel.dbData.OwlData.RDFG.EdgeData.ContainsKey(string.Format("{0}-{1}", nName.Split(':')[0], reqParts)))
                    relation = KGConsoleModel.dbData.OwlData.RDFG.EdgeData[string.Format("{0}-{1}", nName.Split(':')[0], reqParts)];

                //if sub class I want to add all data properties gathered from all base classes till owl:Thing
                if (relation.ToLower().Equals("subclassof"))
                {
                    string source = nOutgoing[0].Split('-')[1];
                    SemanticStructure ss = null;
                    while (!source.ToLower().Equals("owl:thing"))
                    {
                        ss = KGConsoleModel.dbData.OwlData.RDFG.GetNodeDetails(source.Split(':')[0]);
                        if (ss != null)
                            classHierarchy.Add(ss);

                        List<string> og = KGConsoleModel.dbData.OwlData.RDFG.GetEdgesForNode(source);
                        List<string> ic = KGConsoleModel.dbData.OwlData.RDFG.GetIncomingEdgesForNode(source);
                        foreach (string ics in ic)
                        {
                            if (ics.Split(':')[1].Equals("DatatypeProperty"))
                                incoming.Add(ics);
                        }
                        foreach (string sst in og) //must be one only
                            source = sst.Split('-')[1];
                    }
                    ss = KGConsoleModel.dbData.OwlData.RDFG.GetNodeDetails(source.Split(':')[0]);
                    if (ss != null)
                        classHierarchy.Add(ss);
                }
                int c = 1;
                if (classHierarchy.Count != 0)
                {
                    //Construct result output
                    sb.Append("Search Results \n");
                    sb.Append("Primary structure\n");
                    sb.Append(string.Format("\t {0}", classHierarchy[0].SSName));
                    sb.Append("\n");
                    sb.Append(string.Format("Class hierarchy\n"));
                    c = 1;
                    while (c <= classHierarchy.Count - 1)
                    {
                        sb.Append(string.Format("\t{0}", classHierarchy[c].SSName));
                        c++;
                    }
                    sb.Append("\n");
                }
                sb.Append(string.Format("Data Properties\n"));
                List<string> incomingCls = new List<string>();
                foreach (string s in incoming)
                {
                    if (s.Split(':')[1].ToLower().Contains("instance"))
                        continue;
                    if (s.Split(':')[1].ToLower().Contains("class"))
                    {
                        incomingCls.Add(s.Split(':')[0]);
                    }
                    else
                    {
                        sb.Append(string.Format("\t {0}", s.Split(':')[0]));
                        sb.Append("\n");
                    }
                }
                sb.Append(string.Format("Sub Classes"));
                sb.Append("\n");
                foreach (string icStr in incomingCls)
                {
                    sb.Append(string.Format("\t {0}", icStr));
                    sb.Append("\n");
                }
                sb.Append(string.Format("Object Restrictions"));
                sb.Append("\n");
                c = 1;
                while (c <= nOutgoing.Count - 1)
                {
                    if (nOutgoing[c].Split('-')[1].ToLower().Contains("instance"))
                    {
                        c++;
                        continue;
                    }
                    if (nOutgoing[c].Split('-')[1].ToLower().Contains("objectproperty"))
                        sb.Append("\n");
                    sb.Append(string.Format("\t{0}", nOutgoing[c].Split('-')[1].Split(':')[0]));
                    sb.Append("\n");
                    c++;
                }
                sb.Append("\n");
            }

            //Now process Edge data
            List<string> eds = new List<string>();
            foreach (string sedg in KGConsoleModel.dbData.OwlData.RDFG.EdgeData.Keys)
            {
                if (sedg.Split('-')[0].ToLower().Equals(nName.Split(':')[0].ToLower()))
                {
                    if (KGConsoleModel.dbData.OwlData.RDFG.EdgeData[sedg].Equals("Instance"))
                        eds.Add(sedg);
                }
            }
            sb.Append(string.Format("Intsnaces"));
            sb.Append("\n");
            
            foreach (string es in eds)
            {
                SemanticStructure insObj = KGConsoleModel.dbData.OwlData.RDFG.GetNodeDetails(es.Split('-')[1]);
                if (insObj != null)
                {
                    string inName = KGConsoleModel.dbData.OwlData.RDFG.GetExactNodeName(insObj.SSName);
                    sb.Append(string.Format("\t{0}", inName.Split(':')[0]));
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
    }
}


#region Graph construction
/*
 * 
            Graph gp = new Graph("Search Graph");
            foreach(SemanticStructure st in classHierarchy)
            {
                gp.AddNode(st.SSName);
            }

            int c = 1;
            while (c <= classHierarchy.Count - 1)
            {
                gp.AddEdge(classHierarchy[0].SSName, classHierarchy[c].SSName);
                c++;
            }

            foreach(string s in incoming)
            {
                gp.AddNode(s);
                gp.AddEdge(classHierarchy[0].SSName, s);
            }

            c = 1;
            while (c <= outgoing.Count - 1)
            {
                gp.AddNode(outgoing[c].Split('-')[1]);
                gp.AddEdge(classHierarchy[0].SSName, outgoing[c].Split('-')[1]);
                c++;
            }

            Server.KnowledgeGraph.KnowledgeGraph kgr = new KnowledgeGraph.KnowledgeGraph(gp);
            kgr.ShowDialog();
 * 
 * */
#endregion