using ContractDataModels;
using DataSerailizer;
using MySql.Data.MySqlClient;
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
using UserRegModule.DAtaOps;
using UserRegModule.Models;
using UserRegModule.Utilities;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>

    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class RegistrationWindow : Window, ISendAddNewUserResult, ISendUserAddUpdate, INotifyPropertyChanged
    {
        DSSFileParser dssFileParser;
        UserRegModel urModel;
        bool canSaveDet;
        List<UserDataModel> selectedModels;
        MySqlConnection conn = null;
        public RegistrationWindow()
        {
            urModel = new UserRegModel();
            conn = ConnectToDb1.Instance.conn;
            InitializeComponent();
            canSaveDet = false;
            this.DataContext = urModel;
            this.SResult += "Service Status:";
            GetUserRoles();
            selectedModels = new List<UserDataModel>();
        }

        void GetUserRoles()
        {
            urModel.URoles.Clear();
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
                //I am adding new Roles
                uRoles.Add("AlgorithmDeveloper*");
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
                GetUserRoles();
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
            if (urModel.SRoles.Count >= 1)//!string.IsNullOrEmpty(urModel.SURole))
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
            foreach(string sStr in urModel.SRoles)
            {
                //Existing Role is selected
                //Ideally i wanted to call ReturnParseResult and then use the results to
                //show the screen that is taking time and hence I am hardcoding 
                if (sStr.ToLower().Equals("prosumer"))
                {
                    //show Prosumer screen
                    ProsumerDet pDet = new ProsumerDet(this.dssFileParser);
                    pDet.Closing += PDet_Closing;
                    pDet.ShowDialog();
                }
                else if (sStr.ToLower().Equals("consumer"))
                {
                    //show consumer screen
                    ConsumerDet cDet = new ConsumerDet(this.dssFileParser);
                    cDet.Closing += CDet_Closing;
                    cDet.ShowDialog();
                }
                else if (sStr.ToLower().Equals("smartuser"))
                {
                    //show Smart User screen
                    SmartUser su = new SmartUser(this.dssFileParser);
                    su.Closing += Su_Closing;
                    su.ShowDialog();
                }
                else if (sStr.ToLower().Contains("algorithmdeveloper"))
                {
                    ADDet adDet = new ADDet();
                    adDet.Closing += AdDet_Closing;
                    adDet.ShowDialog();
                }
                else
                {
                    //Here we have a user that is not standard and hence we need to see how to get data.
                    if (!SaveToDB.NewUserExists(sStr, conn))
                    {
                        NewUserDataModel nuD = SaveToDB.GetExistingUSer(sStr, conn);
                        //Need to fill details here !!
                        NewUserDet nuDet = new NewUserDet(nuD);
                        nuDet.Closing += NuDet_Closing;
                        nuDet.ShowDialog();
                    }
                }
            }
            if(!string.IsNullOrEmpty(urModel.NRName))
            {
                //New thing is created
                NewUserDet nuDet = new NewUserDet(urModel.NRName);
                nuDet.Closing += NuDet_Closing;
                nuDet.ShowDialog();
            }
        }

        private void NuDet_Closing(object sender, CancelEventArgs e)
        {
            NewUserDet nudet = sender as NewUserDet;
            if(nudet != null)
            {
                selectedModels.Add(nudet.NUDM);
            }
        }

        private void AdDet_Closing(object sender, CancelEventArgs e)
        {
            ADDet ad = sender as ADDet;
            if(ad != null)
            {
                selectedModels.Add(ad.ADModel);
            }
        }

        private void Su_Closing(object sender, CancelEventArgs e)
        {
            SmartUser su = sender as SmartUser;
            if(su != null)
            {
                selectedModels.Add(su.SUModel);
            }
        }

        private void CDet_Closing(object sender, CancelEventArgs e)
        {
            ConsumerDet cd = sender as ConsumerDet;
            if(cd != null)
            {
                selectedModels.Add(cd.CModel);
            }
        }

        private void PDet_Closing(object sender, CancelEventArgs e)
        {
            ProsumerDet pd = sender as ProsumerDet;
            if(pd != null)
            {
                selectedModels.Add(pd.PDModel);
            }
        }

        //Add slected items
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if(cb.IsChecked == true)
            {
                if (!urModel.SRoles.Contains(cb.Content.ToString()))
                    urModel.SRoles.Add(cb.Content.ToString());
            }
            else if (cb.IsChecked == false)
            {
                if (urModel.SRoles.Contains(cb.Content.ToString()))
                    urModel.SRoles.Remove(cb.Content.ToString());
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == false)
            {
                if (urModel.SRoles.Contains(cb.Content.ToString()))
                    urModel.SRoles.Remove(cb.Content.ToString());
            }
        }
        /// <summary>
        /// Need to Save selected models to a temp data
        /// Later I copy these to KG.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnstep3_Click(object sender, RoutedEventArgs e)
        {
            foreach (UserDataModel udM in selectedModels)
            {
                if (udM is ConsumerDataModel)
                {
                    ConsumerDataModel cdm = udM as ConsumerDataModel;
                    if (cdm != null)
                    {
                        if (SaveToDB.SaveConsumerDet(cdm, conn))
                        {
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Consumer {0} details are saved", cdm.UId); }));
                            //Can I call a WCF Service that adds this as a instance ? 
                            UInstEntry uie = new UInstEntry();
                            uie.UId = cdm.UId;
                            uie.UClsName = "consumer";
                            AddUI(uie);
                        }
                        else
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Consumer {0} details are NOT saved", cdm.UId); }));
                    }
                }
                else if (udM is ProsumerDataModel)
                {
                    ProsumerDataModel pdm = udM as ProsumerDataModel;
                    if (pdm != null)
                    {
                        if (SaveToDB.SaveProsumerDet(pdm, conn))
                        {
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Prosumer {0} details are saved", pdm.UId); }));
                            UInstEntry uie = new UInstEntry();
                            uie.UId = pdm.UId;
                            uie.UClsName = "prosumer";
                            AddUI(uie);
                        }
                        else
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Prosumer {0} details are NOT saved", pdm.UId); }));
                    }
                }
                else if (udM is SmartUserDataModel)
                {
                    SmartUserDataModel sdm = udM as SmartUserDataModel;
                    if (sdm != null)
                    {
                        if (SaveToDB.SaveSUDetails(sdm, conn))
                        {
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Smart User {0} details are saved", sdm.UId); }));
                            UInstEntry uie = new UInstEntry();
                            uie.UId = sdm.UId;
                            uie.UClsName = "smartuser";
                            AddUI(uie);
                        }
                        else
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Smart User {0} details are NOT saved", sdm.UId); }));
                    }
                }
                else if (udM is ADevDataModel)
                {
                    ADevDataModel adm = udM as ADevDataModel;
                    if(adm != null)
                    {
                        if(SaveToDB.SaveALDDetails(adm,conn))
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Algo Dev {0} details are saved", adm.UId); }));
                        else
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Algo Dev {0} details are NOT saved", adm.UId); }));
                    }
                }
                else if (udM is NewUserDataModel)
                {
                    NewUserDataModel ndm = udM as NewUserDataModel;
                    if (ndm != null)
                    {
                        if (SaveToDB.SaveNewUserDetails(ndm, conn))
                        {
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("New User {0} details are saved", ndm.UId); }));
                            UInstEntry uie = new UInstEntry();
                            uie.UId = ndm.UId;
                            uie.UClsName = ndm.AUName; //class name is in urModel.NRName
                            AddUI(uie);
                        }
                        else
                            this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("New User {0} details are NOT saved", ndm.UId); }));
                    }
                }
                else
                {
                    //should not be here
                    this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("This object type is NOT known"); }));
                }
            }
            this.selectedModels.Clear();
        }

        void AddUI(UInstEntry uie)
        {
            try
            {
                string uri = "net.tcp://localhost:6565/AddNewUserInstance";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                DuplexChannelFactory<IAddUserInstance> channel = new DuplexChannelFactory<IAddUserInstance>(new InstanceContext(this), binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(new InstanceContext(this), endPoint);
                proxy.AddUserInstance(uie);
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Exception happend when adding new user to KG.\n Details {0}", ex.Message); }));
            }
        }
          

        public void SendUAddResult(bool res)
        {
            //Process call back for adding user instance
            if(res)
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Added instance to user in KG."); }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Text += Environment.NewLine + string.Format("Failed to add instance to user in KG."); }));
            }
        }
    }
}

/*
 *
 * */
