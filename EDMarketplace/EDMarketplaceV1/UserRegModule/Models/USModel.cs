using DataSerailizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegModule.Models
{
    public class USModel : INotifyPropertyChanged
    {
        string fPath;
        public string FPath
        {
            get
            {
                return this.fPath;
            }
            set
            {
                this.fPath = value;
                OnPropertyChanged("FPath");
            }
        }

        List<DSLayoutModel> dslModels;
        public List<DSLayoutModel> DslModels
        {
            get
            {
                return this.dslModels;
            }
            set
            {
                this.dslModels = value;
                OnPropertyChanged("DslModels");
            }
        }

        bool ffName;
        public bool FFName
        {
            get
            {
                return this.ffName;
            }
            set
            {
                this.ffName = value;
                OnPropertyChanged("FFName");
            }
        }

        int fNo;
        public int FNo
        {
            get
            {
                return this.fNo;
            }
            set
            {
                this.fNo = value;
                OnPropertyChanged("FNo");
            }
        }

        string luName;

        public string LUName
        {
            get
            {
                return this.luName;
            }
            set
            {
                this.luName = value;
                OnPropertyChanged("LUName");
            }
        }

        public USModel()
        {
            this.DslModels = new List<DSLayoutModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AlDevModel :INotifyPropertyChanged
    {
        string luName;
        public string LuName
        {
            get
            {
                return this.luName;
            }
            set
            {
                this.luName = value;
                OnPropertyChanged("LuName");
            }
        }
        List<AlDev> alDev;
        public List<AlDev> AlDev
        {
            get
            {
                return this.alDev;
            }
            set
            {
                this.alDev = value;
                OnPropertyChanged("AlDev");
            }
        }
        List<string> sInst;
        public List<string> SelInst
        {
            get { return this.sInst; }
            set { this.sInst = value; OnPropertyChanged("SelInst"); }
        }

        List<string> sDS;
        public List<string> SelDS
        {
            get
            {
                return this.sDS;
            }
            set
            {
                this.sDS = value;
                OnPropertyChanged("SelDS");
            }
                
        }
        List<string> sDP;
        public List<string> SelDP
        {
            get { return this.sDP; }
            set { this.sDP = value; OnPropertyChanged("SelDP"); }
        }

        public AlDevModel()
        {
            this.AlDev = new List<AlDev>();
            SelInst = new List<string>();
            SelDS = new List<string>();
            SelDP = new List<string>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    public class AlDev : INotifyPropertyChanged
    {
        string adRole;

        public string ADRole
        {
            get { return this.adRole; }
            set { this.adRole = value; OnPropertyChanged("ADRole"); }
        }

        List<AlDevInst> adInst;
        public List<AlDevInst> AdInst
        {
            get
            {
                return this.adInst;
            }
            set
            {
                this.adInst = value;
                OnPropertyChanged("AdInst");
            }
        }
        public AlDev()
        {
            this.AdInst = new List<AlDevInst>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AlDevInst: INotifyPropertyChanged
    {
        string adInstName;
        public string AdInstName
        {
            get
            {
                return adInstName;
            }
            set
            {
                adInstName = value;
                OnPropertyChanged("AdInstName");
            }
        }

        List<AlDevDS> alDsDs;
        public List<AlDevDS> AlDsDs
        {
            get
            {
                return this.alDsDs;
            }
            set
            {
                this.alDsDs = value;
                OnPropertyChanged("AlDsDs");
            }
        }  
        public AlDevInst() { this.AlDsDs = new List<AlDevDS>(); }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AlDevDS: INotifyPropertyChanged
    {
        string aldsName;
        public string AlDsName
        {
            get
            {
                return this.aldsName;
            }
            set
            {
                this.aldsName = value;
                OnPropertyChanged("AlDsName");
            }
        }
        List<DSLayoutModel> alDsLY;
        public List<DSLayoutModel> AlDsLY
        {
            get
            {
                return this.alDsLY;
            }
            set
            {
                this.alDsLY = value;
                OnPropertyChanged("AlDsLY");
            }
        }
        public AlDevDS() { this.AlDsLY = new List<DSLayoutModel>(); }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
