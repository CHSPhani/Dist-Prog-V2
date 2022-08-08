using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSCreationApp
{
    public class DSModel : INotifyPropertyChanged
    {
        List<string> fTypes;
        public List<string> FTypes
        {
            get
            {
                return this.fTypes;
            }
            set
            {
                this.fTypes = value;
                OnPropertyChanged("FTypes");
            }
        }

        string sfType;
        public string SFType
        {
            get
            {
                return this.sfType;
            }
            set
            {
                this.sfType = value;
                OnPropertyChanged("SFType");
            }
        }

        public DSModel()
        {
            List<string> tps = new List<string>();
            tps.Add("XML");
            tps.Add("JSON");
            this.FTypes = tps;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
