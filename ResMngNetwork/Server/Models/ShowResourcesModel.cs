using DataSerailizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ShowResourcesModel : INotifyPropertyChanged
    {
        string curUserName;
        public string CurUserName
        {
            get
            {
                return this.curUserName;
            }
        }

        DBData curCbData;

        public DBData CurDbData
        {
            get
            {
                return this.curCbData;
            }
        }
        public List<ResourceItem> Items { get; private set; }
        
        List<string> clsDetails;
        public List<string> ClsDetails
        {
            get
            {
                return this.clsDetails;
            }
            set
            {
                this.clsDetails = value;
                OnPropertyChanged("ClsDetails");
            }
        }

        List<string> pDetails;
        public List<string> PrptyDetails
        {
            get
            {
                return this.pDetails;
            }
            set
            {
                this.pDetails = value;
                OnPropertyChanged("PrptyDetails");
            }
        }

        public ShowResourcesModel()
        {
            clsDetails = new List<string>();
            pDetails = new List<string>();
        }

        public  ShowResourcesModel(string uName, DBData dbData)
        {
            this.curUserName = uName;
            this.curCbData = dbData;
            clsDetails = new List<string>();
            pDetails = new List<string>();

            List<string> packages = new List<string>();
            foreach (PackageData pData in curCbData.PkgData)
            {
                packages.Add(pData.PkgName);
            }

            Items = new List<ResourceItem>();
            ResourceItem rItem1 = new ResourceItem("Entity");
            this.Items.Add(rItem1);

            foreach (string s in packages)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                string[] cItems = s.Split(new char[] { '.' });

                ResourceItem rItem = rItem1;
                ResourceItem nrItem = null;
                bool found = false;
                foreach (string si in cItems)
                {
                    if (rItem1.HasNode(si, rItem, ref nrItem))
                    {
                        rItem = nrItem;
                        found = true;
                        continue;
                    }
                    if (found)
                        rItem.Childs.Add(new ResourceItem(si));
                    else
                        rItem1.Childs.Add(new ResourceItem(si));
                }
            }
        }
        public void FillPropertyDetails(string pkgName)
        {
            this.pDetails.Clear();
            List<string> pNames = new List<string>();

            List<string> pd = new List<string>();
            
            List<string> clde = GetPropertyDetails(pkgName);
            
            foreach (string cd in clde)//string s in pData.Keys)
                pNames.Add(string.Format("{0}", cd));// s);

            this.PrptyDetails = pNames;
        }

        List<string> GetPropertyDetails(string pkgName)
        {
            List<string> propertyDetails = new List<string>();
            foreach(EntityData eData in curCbData.PropertyData)
            {
                if (eData.ETURIName.Contains(pkgName))
                    propertyDetails.Add(eData.FullEntityName);
            }
            return propertyDetails;
        }

        public void FillClassDetails(string pkgName)
        {
            this.clsDetails.Clear();
            
            List<string> cNames = new List<string>();

            
            List<string> clde = GetClassDetails(pkgName);
            
            foreach (string cd in clde)
                cNames.Add(string.Format("{0}", cd));

            this.ClsDetails = cNames;
        }
        List<string> GetClassDetails(string pkgName)
        {
            List<string> propertyDetails = new List<string>();
            foreach (EntityData eData in curCbData.ClassData)
            {
                if (eData.ETURIName.Contains(pkgName))
                    propertyDetails.Add(eData.FullEntityName);
            }
            return propertyDetails;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ResourceItem
    {
        public ResourceItem(string name)
        {
            this.Childs = new List<ResourceItem>();
            this.Name = name;
            this.IsRootNode = false;
        }

        public List<ResourceItem> Childs { get; private set; }
        public string Name { get; private set; }

        public bool IsRootNode { get; set; }

        public bool HasNode(string si, ResourceItem rootNode, ref ResourceItem newRootNode)
        {
            newRootNode = null;
            foreach (ResourceItem ri in rootNode.Childs)
            {
                if (ri.Name.Equals(si))
                {
                    newRootNode = ri;
                    return true;
                }
            }
            return false;
        }
    }
}
