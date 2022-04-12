using Server.Models;
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
using Server.DataFileProcess;

namespace Server
{
    /// <summary>
    /// Interaction logic for ValidationFeedback.xaml
    /// </summary>
    public partial class ValidationFeedback : Window
    {
        public ValidationFeedback()
        {
            InitializeComponent();
        }

        public ValidationFeedback(ValidationFeedbackModel vfModel)
        {
            this.DataContext = vfModel;
            InitializeComponent();
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
