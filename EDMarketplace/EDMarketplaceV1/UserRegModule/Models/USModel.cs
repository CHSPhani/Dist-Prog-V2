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

        public USModel() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
