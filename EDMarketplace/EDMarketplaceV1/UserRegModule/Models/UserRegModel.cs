using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegModule.DAtaOps;
using UserRegModule.Utilities;

namespace UserRegModule.Models
{
    public class UserStats :INotifyPropertyChanged
    {
        int noPRos;
        public int NOOfProsumers
        {
            get
            {
                return this.noPRos;
            }
            set
            {
                this.noPRos = value;
                OnPropertyChanged("NOOfProsumers");
            }
        }

        int noCons;
        public int NOOfConsumers
        {
            get
            {
                return this.noCons;
            }
            set
            {
                this.noCons = value;
                OnPropertyChanged("NOOfConsumers");
            }
        }

        int noSU;
        public int NOSU
        {
            get
            {
                return this.noSU;
            }
            set
            {
                this.noSU = value;
                OnPropertyChanged("NOSU");
            }
        }

        int noAD;
        public int NOAD
        {
            get
            {
                return this.noAD;
            }
            set
            {
                this.noAD = value;
                OnPropertyChanged("NOAD");
            }
        }

        int noNU;
        public int NONU
        {
            get
            {
                return this.noNU;
            }
            set
            {
                this.noNU = value;
                OnPropertyChanged("NONU");
            }
        }

        string cuName;
        public string CUName
        {
            get
            {
                return this.cuName;
            }
            set
            {
                this.cuName = value;
                OnPropertyChanged("CUName");
            }
        }

        List<string> eRoles;
        public List<string> ERoles
        {
            get
            {
                return this.eRoles;
            }
            set
            {
                this.eRoles = value;
                OnPropertyChanged("ERoles");
            }
        }

        string sRole;
        public string SRole
        {
            get
            {
                return this.sRole;
            }
            set
            {
                this.sRole = value;
                OnPropertyChanged("SRole");
            }
        }

        string pwd;
        public string PWD
        {
            get
            {
                return this.pwd;
            }
            set
            {
                this.pwd = value;
                OnPropertyChanged("PWD");
            }
        }

        public UserStats()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class NUDataProperty: INotifyPropertyChanged
    {
        string nudpName;
        public string NUDPName
        {
            get
            {
                return this.nudpName;
            }
            set
            {
                this.nudpName = value;
                OnPropertyChanged("NUDPName");
            }
        }

        List<string> nudpTypes;
        public List<string> NUDPTypes
        {
            get
            {
                return this.nudpTypes;
            }
            set
            {
                this.nudpTypes = value;
                OnPropertyChanged("NUDPType");
            }
        }

        string sNudpType;
        public string SNudpType
        {
            get
            {
                return this.sNudpType;
            }
            set
            {
                this.sNudpType = value;
                OnPropertyChanged("SNudpType");
            }
        }

        string nudpExpr;
        public string NUDPExpr
        {
            get
            {
                return this.nudpExpr;
            }
            set
            {
                this.nudpExpr = value;
                OnPropertyChanged("NUDPExpr");
            }
        }

        public NUDataProperty()
        {
            List<string> dtys = new List<string>();
            dtys.Add("xsd:integer");
            dtys.Add("xsd:float");
            dtys.Add("xsd:double");
            dtys.Add("xsd:bool");
            dtys.Add("xsd:string");
            this.NUDPTypes = dtys;
        }

        public override string ToString()
        {
            return string.Format("{0}&{1}&{2}", this.NUDPName, this.SNudpType, this.NUDPExpr);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class NURestriction:INotifyPropertyChanged
    {
        string nurName;
        public string NURName
        {
            get
            {
                return this.nurName;
            }
            set
            {
                this.nurName = value;
                OnPropertyChanged("NURName");
            }
        }

        string toName;
        public string TargetOName
        {
            get
            {
                return this.toName;
            }
            set
            {
                this.toName = value;
                OnPropertyChanged("TargetOName");
            }
        }

        List<string> rs;
        public List<string> Rstrs
        {
            get
            {
                return this.rs;
            }
            set
            {
                this.rs = value;
                OnPropertyChanged("Rstrs");
            }
        }

        List<string> oTypes;
        public List<string> OTypes
        {
            get
            {
                return this.oTypes;
            }
            set
            {
                this.oTypes = value;
                OnPropertyChanged("OTypes");
            }
        }
        string srName;
        public string SRName
        {
            get
            {
                return this.srName;
            }
            set
            {
                this.srName = value;
                OnPropertyChanged("SRName");
            }
        }
        public NURestriction()
        {
            List<string> rss = new List<string>();
            rss.Add("some");
            rss.Add("only");
            this.Rstrs = rss;
            List<string> ots = new List<string>();
            ots.Add("Load");
            ots.Add("PVSystem");
            ots.Add("Transformer");
            this.OTypes = ots;
        }

        public override string ToString()
        {
            return string.Format("{0}&{1}&{2}", this.NURName, this.TargetOName, this.SRName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class NewUserDataModel : UserDataModel
    {
        int nID;
        public int NID
        {
            get
            {
                return this.nID;
            }
            set
            {
                this.nID = value;
                OnPropertyChanged("NID");
            }
        }
        string auName;
        public string AUName
        {
            get
            {
                return this.auName;
            }
            set
            {
                this.auName = value;
                OnPropertyChanged("AUName");
            }
        }
        List<NUDataProperty> nuDataProps;
        public List<NUDataProperty> NUDataProps
        {
            get
            {
                return this.nuDataProps;
            }
            set
            {
                this.nuDataProps = value;
                OnPropertyChanged("NUDataProps");
            }
        }

        List<NURestriction> nuRestrictions;
        public List<NURestriction> NURestrictions
        {
            get
            {
                return this.nuRestrictions;
            }
            set
            {
                this.nuRestrictions = value;
                OnPropertyChanged("NURestrictions");
            }
        }

        public NewUserDataModel()
        {
            NUDataProps = new List<NUDataProperty>();
            NURestrictions = new List<NURestriction>();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class UserDataModel : INotifyPropertyChanged
    {
        string uID;
        public string UId
        {
            get
            {
                return this.uID;
            }
            set
            {
                this.uID = value;
                OnPropertyChanged("UId");
            }
        }

        string uName;
        public string UName
        {
            get
            {
                return this.uName;
            }
            set
            {
                this.uName = value;
                OnPropertyChanged("UName");
            }
        }

        string uAdd;

        public string UAddress
        {
            get
            {
                return this.uAdd;
            }
            set
            {
                this.uAdd = value;
                OnPropertyChanged("UAddress");
            }
        }

        //General Properties
        string ufName;
        public string UFName
        {
            get
            {
                return this.ufName;
            }
            set
            {
                this.ufName = value;
                OnPropertyChanged("UFName");
            }
        }

        string ueMail;
        public string UEMail
        {
            get
            {
                return this.ueMail;
            }
            set
            {
                this.ueMail = value;
                OnPropertyChanged("UEMail");
            }
        }

        string uePhone;
        public string UEPhone
        {
            get
            {
                return this.uePhone;
            }
            set
            {
                this.uePhone = value;
                OnPropertyChanged("UEPhone");
            }
        }

        string uPwd;
        public string UPwd
        {
            get
            {
                return this.uPwd;
            }
            set
            {
                this.uPwd = value;
                OnPropertyChanged("UPwd");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ADevDataModel:UserDataModel
    {
        List<string> uRoles;
        public List<string> URoles
        {
            get
            {
                return this.uRoles;
            }
            set
            {
                this.uRoles = value;
                OnPropertyChanged("URoles");
            }
        }

        List<string> selDs;
        public List<string> SelDS
        {
            get
            {
                return this.selDs;
            }
            set
            {
                this.selDs = value;
                OnPropertyChanged("SelDS");
            }
        }

        public ADevDataModel()
        {
            this.URoles = ServiceUtils.GetUserRoles();
            this.SelDS = new List<string>();
        }
    }

    public class SmartUserDataModel : UserDataModel
    {
        List<string> loads;
        public List<string> Loads
        {
            get
            {
                return this.loads;
            }
            set
            {
                this.loads = value;
                OnPropertyChanged("Loads");
            }
        }

        string sLoad;
        public string SLoad
        {
            get
            {
                return this.sLoad;
            }
            set
            {
                this.sLoad = value;
                OnPropertyChanged("SLoad");
            }
        }

        List<string> fOptions;
        public List<string> FOptions
        {
            get
            {
                return this.fOptions;
            }
            set
            {
                this.fOptions = value;
                OnPropertyChanged("FOptions");
            }
        }
        string sfOption;
        public string SFOption
        {
            get
            {
                return this.sfOption;
            }
            set
            {
                this.sfOption = value;
                OnPropertyChanged("SFOption");
            }
        }

        public SmartUserDataModel()
        {
            this.Loads = new List<string>();
            List<string> tfOptions = new List<string>();
            tfOptions.Add("Bank Account");
            tfOptions.Add("Coins");
            this.FOptions = tfOptions;
            //Serice calls
            this.Loads = ServiceUtils.ReturnInstanceResult("load");
        }
    }

    public class ConsumerDataModel : UserDataModel
    {
        double cDuration;
        public double CDuration
        {
            get
            {
                return this.cDuration;
            }
            set
            {
                this.cDuration = value;
                OnPropertyChanged("CDuration");
            }
        }

        string intInRE;
        public string IntInRE
        {
            get
            {
                return this.intInRE;
            }

            set
            {
                this.intInRE = value;
                OnPropertyChanged("IntInRE");
            }
        }

        List<string> loads;
        public List<string> Loads
        {
            get
            {
                return this.loads;
            }
            set
            {
                this.loads = value;
                OnPropertyChanged("Loads");
            }
        }

        string sLoad;
        public string SLoad
        {
            get
            {
                return this.sLoad;
            }
            set
            {
                this.sLoad = value;
                OnPropertyChanged("SLoad");
            }
        }

        List<string> fOptions;
        public List<string> FOptions
        {
            get
            {
                return this.fOptions;
            }
            set
            {
                this.fOptions = value;
                OnPropertyChanged("FOptions");
            }
        }
        string sfOption;
        public string SFOption
        {
            get
            {
                return this.sfOption;
            }
            set
            {
                this.sfOption = value;
                OnPropertyChanged("SFOption");
            }
        }

        public ConsumerDataModel()
        {
            this.Loads = new List<string>();
            List<string> tfOptions = new List<string>();
            tfOptions.Add("Bank Account");
            tfOptions.Add("Coins");
            this.FOptions = tfOptions;
            //Serice calls
            this.Loads = ServiceUtils.ReturnInstanceResult("load");
        }
    }

    public class ProsumerDataModel : UserDataModel
    {
        List<string> pvPanels;
        public List<string> PVPanels
        {
            get
            {
                return this.pvPanels;
            }
            set
            {
                this.pvPanels = value;
                OnPropertyChanged("PVPanels");
            }
        }

        string spvPanel;
        public string SPvPanel
        {
            get
            {
                return this.spvPanel;
            }
            set
            {
                this.spvPanel = value;
                OnPropertyChanged("SPvPanel");
            }
        }

        List<string> loads;
        public List<string> Loads
        {
            get
            {
                return this.loads;
            }
            set
            {
                this.loads = value;
                OnPropertyChanged("Loads");
            }
        }

        string sLoad;
        public string SLoad
        {
            get
            {
                return this.sLoad;
            }
            set
            {
                this.sLoad = value;
                OnPropertyChanged("SLoad");
            }
        }

        List<string> fOptions;
        public List<string> FOptions
        {
            get
            {
                return this.fOptions;
            }
            set
            {
                this.fOptions = value;
                OnPropertyChanged("FOptions");
            }
        }
        string sfOption;
        public string SFOption
        {
            get
            {
                return this.sfOption;
            }
            set
            {
                this.sfOption = value;
                OnPropertyChanged("SFOption");
            }
        }

        public ProsumerDataModel()
        {
            this.PVPanels = new List<string>();
            this.Loads = new List<string>();
            List<string> tfOptions = new List<string>();
            tfOptions.Add("Bank Account");
            tfOptions.Add("Coins");
            this.FOptions = tfOptions;
            //Serice calls
            this.Loads = ServiceUtils.ReturnInstanceResult("load");
            this.PVPanels = ServiceUtils.GetPVDetails();
        }
    }

    public class UserRegModel : INotifyPropertyChanged
    {
        List<string> uRoles;
        public List<string> URoles
        {
            get
            {
                return this.uRoles;
            }
            set
            {
                this.uRoles = value;
                OnPropertyChanged("URoles");
            }
        }

        List<string> sRoles;
        public List<string> SRoles
        {
            get
            {
                return this.sRoles;
            }
            set
            {
                this.sRoles = value;
                OnPropertyChanged("SRoles");
            }
        }

       
        string nrName;
        public string NRName
        {
            get
            {
                return this.nrName;
            }
            set
            {
                this.nrName = value;
                OnPropertyChanged("NRName");
            }
        }
        public UserRegModel()
        {
            this.SRoles = new List<string>();
            this.URoles = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

#region OLD Code
/*
 *   public class ProsumerDataModel: INotifyPropertyChanged
    {

     string suRole;
        public string SURole
        {
            get
            {
                return this.suRole;
            }
            set
            {
                this.suRole = value;
                OnPropertyChanged("SURole");
            }
        }
        List<string> trans;
        public List<string> Trans
        {
            get
            {
                return this.trans;
            }
            set
            {
                this.trans = value;
                OnPropertyChanged("Trans");
            }
        }

        List<string> pvPanels;
        public List<string> PVPanels
        {
            get
            {
                return this.pvPanels;
            }
            set
            {
                this.pvPanels = value;
                OnPropertyChanged("PVPanels");
            }
        }

        string stName;
        public string STName
        {
            get
            {
                return this.stName;
            }
            set
            {
                this.stName = value;
                OnPropertyChanged("STName");
            }
        }

        string spvName;
        public string SPVName
        {
            get
            {
                return this.spvName;
            }
            set
            {
                this.spvName = value;
                OnPropertyChanged("SPVName");
            }
        }

        public ProsumerDataModel()
        {
            this.Trans = new List<string>();
            this.PVPanels = new List<string>();
            //Call WCF service to get PvPanels
            //Call WCF service to get Transformers
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ConsumerDataModel :INotifyPropertyChanged
    {
        List<string> aLoads;
        public List<string> ALoads
        {
            get
            {
                return this.aLoads;
            }
            set
            {
                this.aLoads = value;
                OnPropertyChanged("ALoads");
            }
        }

        string slName;
        public string SLName
        {
            get
            {
                return this.slName;
            }
            set
            {
                this.slName = value;
                OnPropertyChanged("SLName");
            }
        }

        public ConsumerDataModel()
        {
            this.ALoads = new List<string>();
            //Call a WCF service to get all Loads
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class UserRegModel : INotifyPropertyChanged
    {
        string ufName;
        public string UFName
        {
            get
            {
                return this.ufName;
            }
            set
            {
                this.ufName = value;
                OnPropertyChanged("UFName");
            }
        }

        string ueMail;
        public string UEMail
        {
            get
            {
                return this.ueMail;
            }
            set
            {
                this.ueMail = value;
                OnPropertyChanged("UEMail");
            }
        }

        string uePhone;
        public string UEPhone
        {
            get
            {
                return this.uePhone;
            }
            set
            {
                this.uePhone = value;
                OnPropertyChanged("UEPhone");
            }
        }

        List<string> fOptions;
        public List<string> FOptions
        {
            get
            {
                return this.fOptions;
            }
            set
            {
                this.fOptions = value;
                OnPropertyChanged("FOptions");
            }
        }

        string sfoption;
        public string SFOption
        {
            get
            {
                return this.sfoption;
            }
            set
            {
                this.sfoption = value;
                OnPropertyChanged("SFOption");
            }
        }
        List<string> uRoles;
        public List<string> URoles
        {
            get
            {
                return this.uRoles;
            }
            set
            {
                this.uRoles = value;
                OnPropertyChanged("URoles");
            }
        }
        string suRole;
        public string SURole
        {
            get
            {
                return this.suRole;
            }
            set
            {
                this.suRole = value;
                OnPropertyChanged("SURole");
            }
        }

        bool cnRol;
        public bool CNRole
        {
            get
            {
                return this.cnRol;
            }
            set
            {
                this.cnRol = value;
                OnPropertyChanged("CNRole");
            }
        }

        string uid;
        public string UId
        {
            get
            {
                return this.uid;
            }
            set
            {
                this.uid = value;
                OnPropertyChanged("UId");
            }
        }

        string uPwd;
        public string UPwd
        {
            get
            {
                return this.uPwd;
            }
            set
            {
                this.uPwd = value;
                OnPropertyChanged("UPwd");
            }
        }

        string selTrName;
        public string SelTrName
        {
            get
            {
                return this.selTrName;
            }
            set
            {
                this.selTrName = value;
                OnPropertyChanged("SelTrName");
            }
        }

        string selLName;
        public string SelLName
        {
            get
            {
                return this.selLName;
            }
            set
            {
                this.selLName = value;
                OnPropertyChanged("SelLName");
            }
        }

        string nrName;
        public string NRName
        {
            get
            {
                return this.nrName;
            }
            set
            {
                this.nrName = value;
                OnPropertyChanged("NRName");
            }
        }
        public UserRegModel()
        {
            List<string> tfOptions = new List<string>();
            tfOptions.Add("Bank Account");
            tfOptions.Add("Coins");
            this.FOptions = tfOptions;
            //I am hard coing these roles..
            List<string> urRoles = new List<string>();
            urRoles.Add("Prosumer");
            urRoles.Add("Consumer");
            urRoles.Add("Vehicle Owner");
            urRoles.Add("Charging Point Owner");
            this.URoles = urRoles;
            //set Value;
            this.CNRole = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
 * 
 * */
#endregion