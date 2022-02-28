using DataSerailizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class VDViewModel : INotifyPropertyChanged
    {
        string curUserName;
        public string CurrentUserName
        {
            get
            {
                return this.curUserName;
            }
            set
            {
                this.curUserName = value;
                OnPropertyChanged("CurrentUserName");
            }
        }

        DBData curDbInstance;

        public DBData CurrentDbInstance
        {
            get
            {
                return curDbInstance;
            }
        }

        List<string> vDs;

        public List<string> VDs
        {
            get
            {
                return this.vDs;
            }
            set
            {
                this.vDs = value;
                OnPropertyChanged("VDs");
            }
        }

        string selectedDS;

        public string SelectedDS
        {
            get
            {
                return this.selectedDS;
            }
            set
            {
                this.selectedDS = value;
                OnPropertyChanged("SelectedDS");
            }
        }

        public VDViewModel() { }

        public VDViewModel(string uName, DBData dbData)
        {
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
            if (this.CurrentDbInstance.NodeData == null || this.CurrentDbInstance.NodeData.Count == 0)
            {
                List<string> vs = new List<string>();
                vs.Add("No Datasets are validated in this Node");
                this.VDs = vs;
            }
            else
            {
                List<string> vs = new List<string>();
                foreach (NodeData nd in this.CurrentDbInstance.NodeData)
                {
                    vs.Add(string.Format("{0} File of type {1} with {2} Columns and {3} rows is validated against {4}", nd.FileName, nd.FileType, nd.NoOfCols, nd.NoOfRows, nd.VerifiedDataSet));// nd.VerifiedDataSet.CName));
                }
                this.VDs = vs;
            }
        }

        public void DeletedDS()
        {
            string s = this.SelectedDS.Split(' ')[0];
            this.curDbInstance.NodeData.RemoveAll((n) => { if (n.FileName.Equals(s)) { return true; } else { return false; } });
            this.vDs.Clear();
            List<string> vs = new List<string>();
            foreach (NodeData nd in this.CurrentDbInstance.NodeData)
            {
                vs.Add(string.Format("{0} File of type {1} with {2} Columns and {3} rows is validated against {4}", nd.FileName, nd.FileType, nd.NoOfCols, nd.NoOfRows, nd.VerifiedDataSet));// nd.VerifiedDataSet.CName));
            }
            this.VDs = vs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
