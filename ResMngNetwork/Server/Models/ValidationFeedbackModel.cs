using Server.DataFileProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ValidationFeedbackModel : INotifyPropertyChanged
    {
        List<string> proposedCls;
        public List<string> ProposedCls
        {
            get { return this.proposedCls; }
            set { this.proposedCls = value; OnPropertyChanged("ProposedCls"); }
        }

        string selectedItem;
        public string SelectedItem
        {
            get { return this.selectedItem; }
            set { this.selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value; OnPropertyChanged("IsChecked");
            }
        }

        public ValidationFeedbackModel()
        {
            this.SelectedItem = string.Empty;
            proposedCls = new List<string>();
        }

        public ValidationFeedbackModel(List<string> pC) : this()
        {
            this.ProposedCls = pC;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
