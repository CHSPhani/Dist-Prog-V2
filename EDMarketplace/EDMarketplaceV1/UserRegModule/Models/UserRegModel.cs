using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegModule.Models
{
    public class ProsumerDataModel: INotifyPropertyChanged
    {
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
}
