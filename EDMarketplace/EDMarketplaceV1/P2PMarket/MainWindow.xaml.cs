using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P2PMarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MWModel mwModel; 
        public MainWindow()
        {
            mwModel = new MWModel();
            InitializeComponent();
            this.DataContext = mwModel;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class MWModel : INotifyPropertyChanged
    {
        int tradingPeriodInterval = 30;
        string tradingPeriodUnit = "min";
        string tradingPeriod = string.Empty;
        public string TPeriod
        {
            get { return this.tradingPeriod; }
            set { this.tradingPeriod = value; OnPropertyChanged("TPeriod"); }
        }
            
        public MWModel()
        {
            string time = DateTime.Now.ToString("h:mm:ss tt"); //Output 9:29:47 AM
            string[] times = time.Split(':');

            if (Int32.Parse(times[1]) >= 30)
            {
                this.TPeriod = string.Format("{0}:{1}-{2}{3}", times[0], "30", (Int32.Parse(times[0]) + 1), "00");
            }
            else
            {
                this.TPeriod = string.Format("{0}:{1}-{2}{3}", times[0], "00", times[0], "30");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
