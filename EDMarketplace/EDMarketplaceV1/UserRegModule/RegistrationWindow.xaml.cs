using ContractDataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
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
using UoB.ToolUtilities.OpenDSSParser;
using UserRegModule.Models;
using UserRegModule.Utilities;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>

    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class RegistrationWindow : Window, ISendAddNewUserResult, INotifyPropertyChanged
    {
        DSSFileParser dssFileParser;
        UserRegModel urModel;
        bool canSaveDet;
        public RegistrationWindow()
        {
            urModel = new UserRegModel();
            InitializeComponent();
            canSaveDet = false;
            this.DataContext = urModel;
            this.SResult += "Service Status:";
            GetUserRoles();
        }

        void GetUserRoles()
        {
            List<string> uRoles = new List<string>();
            string sResult = string.Empty;
            try
            {
                string uri = "net.tcp://localhost:6565/KGConsoleModel";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IObtainSearchResults>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                sResult = proxy.GetSearchResults("users");
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Exception happend when calling Service for getting Serach details from KG. Details {0}", ex.Message); }));
                urModel.URoles = null;
            }
            if(string.IsNullOrEmpty(sResult))
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("NO Search Results are obtained for Users from KG."); })); 
                urModel.URoles = null;
            }
            else
            {
                //Update status
                this.SResult += Environment.NewLine + string.Format("Search Results obtained for Users from KG.");
                //Parse string and construct an object.
                Dictionary<string, List<string>> sResults = ParseUtils.ParseSearchString(sResult);
                uRoles = sResults[ParseUtils.SCls];
            }
            urModel.URoles = uRoles;
        }

        public RegistrationWindow(DSSFileParser tDssFileParser) : this()
        {
            this.dssFileParser = tDssFileParser;
        }

        private void RegExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Vuser_Click(object sender, RoutedEventArgs e)
        {
            //Here we call Validate user with the class/
            //Step1: Convert urModel to JSON
            //Call WCF service for Validation
            //In case true then store in Database
            //Else say that network rejected.
        }

        private void Suser_Click(object sender, RoutedEventArgs e)
        {
            //Here we save the urModel to database.
            if(canSaveDet)
            {

            }
        }

        //Creating new user role..by calling service
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(urModel.NRName))
                return;

            try
            {
                string uri = "net.tcp://localhost:6565/AddNewUserRole";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                DuplexChannelFactory<IAddNewUserRole> channel = new DuplexChannelFactory<IAddNewUserRole>(new InstanceContext(this), binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(new InstanceContext(this), endPoint);
                proxy.AddNewUser(urModel.NRName);
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Exception happend when adding new user to KG.\n Details {0}", ex.Message); }));
            }

            
        }
        string sResult = string.Empty;
        public string SResult
        {
            get
            {
                return this.sResult;
            }
            set
            {
                this.sResult = value;
                OnPropertyChanged("SResult");
            }
        }
        public void SendAddUserResult(bool addResult)
        {
            if (addResult)
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Added new user to KG."); btnstep2.IsEnabled = true; btnstep3.IsEnabled = true; }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Cannot add new user to KG."); }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CmbRoles_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(urModel.SURole))
            {
                btnstep2.IsEnabled = true;
                btnstep3.IsEnabled = true;
            }
        }
        /// <summary>
        ///  In this step2 we need to create screen for entering details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnstep2_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(urModel.SURole))
            {
                //Existing Role is selected

            }
            else if(!string.IsNullOrEmpty(urModel.NRName))
            {
                //New thing is created
            }
        }
    }
}
