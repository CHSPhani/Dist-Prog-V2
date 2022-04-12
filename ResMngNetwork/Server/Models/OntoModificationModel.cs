using DataSerailizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class OntoModificationModel : INotifyPropertyChanged
    {
        string ncName;
        public string NewClsName
        {
            get
            {
                return this.ncName;
            }
            set
            {
                this.ncName = value;
                OnPropertyChanged("NewClsName");
            }
        }

        string eqcName;
        public string EquivalentclsName
        {
            get
            {
                return this.eqcName;
            }
            set
            {
                this.eqcName = value;
                OnPropertyChanged("EquivalentclsName");
            }
        }

        List<string> pName;
        public List<string> PropertiesNames
        {
            get
            {
                return this.pName;
            }
            set
            {
                this.pName = value;
                OnPropertyChanged("PropertiesNames");
            }
        }

        List<string> bClasses;
        public List<string> BaseClsNames
        {
            get
            {
                return this.bClasses;
            }
            set
            {
                this.bClasses = value;
                OnPropertyChanged("BaseClsNames");
            }
        }

        string sbName;
        public string SelectedBCName
        {
            get
            {
                return this.sbName;
            }
            set
            {
                this.sbName = value;
                OnPropertyChanged("SelectedBCName");
            }
        }

        List<string> dTyps;
        public List<string> DTyps
        {
            get
            {
                return this.dTyps;
            }
            set
            {
                this.dTyps = value;
                OnPropertyChanged("DTyps");
            }
        }

        DBData dbData;
        public OntoModificationModel(DBData dbData)
        {
            this.NewClsName = string.Empty;
            this.EquivalentclsName = string.Empty;
            this.SelectedBCName = string.Empty;
            this.PropertiesNames = new List<string>();
            this.DTyps = new List<string>();
            this.BaseClsNames = new List<string>();
            this.DPCats = new List<string>();

            this.dbData = dbData;
            List<string> bc = new List<string>();
            foreach (KeyValuePair<string, SemanticStructure> kvp in dbData.OwlData.RDFG.NODetails)
            {
                if (kvp.Value.SSType == SStrType.Class)
                    bc.Add(kvp.Value.SSName);
            }
            this.BaseClsNames = bc;
            
            //Adding these classes to categorise the Object Properties
            DPCats.Add("EngineeringUnit");
            DPCats.Add("Units");
            //fill details
            FillSuitableBaseClass();
        }
        List<string> seClasses = new List<string>();
        public void FillSuitableBaseClass()
        {
            seClasses.Add("None");
            foreach (string s in this.DPCats)
            {
                //#region Good Recursive Code for getting up hierarchies
                string relation = string.Empty;
                string eName = this.dbData.OwlData.RDFG.GetExactNodeName(s);
                List<string> icmg = this.dbData.OwlData.RDFG.GetIncomingEdgesForNode(eName);
                List<string> ogn = this.dbData.OwlData.RDFG.GetEdgesForNode(eName);
                foreach (string isg in icmg)
                {
                    seClasses.Add(isg);
                }
            }
        }
        List<string> dpCats;
        public List<string> DPCats
        {
            get
            {
                return this.dpCats;
            }
            set
            {
                this.dpCats = value;
                OnPropertyChanged("DPCats");
            }
        }

        List<DPropperties> dPrps;

        public List<DPropperties> DPrps
        {
            get
            {
                return this.dPrps;
            }
            set
            {
                this.dPrps = value;
                OnPropertyChanged("DPrps");
            }
        }

        public void CreateDPSets(string pName)
        {
            DPropperties dp = new DPropperties()
            {
                DPName = pName,
                DPIName = pName,
                DPDt = this.DTyps,
                DPCat = this.DPCats,
                SelCatName = string.Empty,
                SelDpName = string.Empty,
                EqNames = seClasses,
                DpExp = string.Empty,
                SelEqName = string.Empty
            };
            this.DPrps.Add(dp);
        }
        
        public void CreateDPSets()
        {
            List<DPropperties> dprs = new List<DPropperties>();
            foreach(string s in this.PropertiesNames)
            {
                DPropperties dp = new DPropperties() { DPName = s, DPIName=s, DPDt = this.DTyps, DPCat = this.DPCats, SelCatName = string.Empty,
                                                            SelDpName = string.Empty, EqNames = seClasses, DpExp = string.Empty, SelEqName = string.Empty  };
                dprs.Add(dp);
            }
            this.DPrps = dprs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DPropperties : INotifyPropertyChanged
    {
        string dpName;
        public string DPName
        {
            get
            {
                return this.dpName;
            }
            set
            {
                this.dpName = value;
                OnPropertyChanged("DPName");
            }
        }

        string dpIName;
        public string DPIName
        {
            get
            {
                return this.dpIName;
            }
            set
            {
                this.dpIName = value;
                OnPropertyChanged("DPIName");
            }
        }

        string selDpName;

        public string SelDpName
        {
            get
            {
                return this.selDpName;
            }
            set
            {
                this.selDpName = value;
                OnPropertyChanged("SelDpName");
            }
        }

        List<string> dpDt;
        public List<string> DPDt
        {
            get
            {
                return this.dpDt;
            }
            set
            {
                this.dpDt = value;
                OnPropertyChanged("DPDt");
            }
        }

        List<string> dpCat;
        public List<string> DPCat
        {
            get
            {
                return this.dpCat;
            }
            set
            {
                this.dpCat = value;
                OnPropertyChanged("DPCat");
            }
        }

        string selCatName;
        public string SelCatName
        {
            get
            {
                return this.selCatName;
            }
            set
            {
                this.selCatName = value;
                OnPropertyChanged("SelCatName");
            }
        }

        string dpExp;
        public string DpExp
        {
            get
            {
                return this.dpExp;
            }
            set
            {
                this.dpExp = value;
                OnPropertyChanged("DpExp");
            }
        }

        string selEqName;
        public string SelEqName
        {
            get
            {
                return this.selEqName;
            }
            set
            {
                this.selEqName = value;
                OnPropertyChanged("SelEqName");
            }
        }

        List<string> egNames;
        public List<string> EqNames
        {
            get
            {
                return this.egNames;
            }
            set
            {
                this.egNames = value;
                OnPropertyChanged("EqNames");
            }
        }

        public DPropperties()
        {
            this.DPName = string.Empty;
            this.DPIName = string.Empty;
            this.SelDpName = string.Empty;
            this.DPDt = new List<string>();
            this.DPCat = new List<string>();
            this.SelCatName = string.Empty;
            this.DpExp = string.Empty;
            this.EqNames = new List<string>();
            this.SelEqName = string.Empty;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
