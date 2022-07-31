using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserRegModule.Models;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for NewUserDet.xaml
    /// </summary>
    public partial class NewUserDet : Window
    {
        NewUserDataModel nuDataModel;
        public NewUserDataModel NUDM
        {
            get
            {
                return this.nuDataModel;
            }
        }
        public NewUserDet()
        {
            nuDataModel = new NewUserDataModel();
            InitializeComponent();
            this.DataContext = nuDataModel;
        }
        public NewUserDet(string nuName)
        {
            nuDataModel = new NewUserDataModel();
            nuDataModel.AUName = nuName;
            InitializeComponent();
            this.DataContext = nuDataModel;
        }
        public NewUserDet(NewUserDataModel nuDet)
        {
            InitializeComponent();
            this.nuDataModel = nuDet;
            this.DataContext = nuDataModel;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnNewUserDP_Click(object sender, RoutedEventArgs e)
        {
            DPDet dpdet = new DPDet();
            dpdet.Closing += Dpdet_Closing;
            dpdet.ShowDialog();
        }

        private void Dpdet_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DPDet dd = sender as DPDet;
            List<NUDataProperty> tnudp = new List<NUDataProperty>();
            foreach(NUDataProperty bb in this.nuDataModel.NUDataProps)
            {
                tnudp.Add(bb);
            }
            
            if (dd != null)
            {
                tnudp.Add(dd.NUDPP);
            }
            this.nuDataModel.NUDataProps.Clear();
            this.nuDataModel.NUDataProps = tnudp;
        }

        private void BtnNewUserRT_Click(object sender, RoutedEventArgs e)
        {
            NewRestrictionDet nrDet = new NewRestrictionDet();
            nrDet.Closing += NrDet_Closing;
            nrDet.ShowDialog();
        }

        private void NrDet_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NewRestrictionDet nrd = sender as NewRestrictionDet;
            List<NURestriction> tnrr = new List<NURestriction>();
            foreach(NURestriction cc in this.nuDataModel.NURestrictions)
            {
                tnrr.Add(cc);
            }
            if(nrd != null)
            {
                tnrr.Add(nrd.NuRes);
            }
            this.nuDataModel.NURestrictions.Clear();
            this.nuDataModel.NURestrictions = tnrr;
        }
    }
}
