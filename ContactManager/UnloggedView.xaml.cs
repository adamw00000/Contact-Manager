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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for UnloggedView.xaml
    /// </summary>
    public partial class UnloggedView : UserControl
    {
        public MainWindow mainWindow { get; set; }

        public UnloggedView()
        {
            InitializeComponent();
        }
        
        private void Register(object sender, RoutedEventArgs e)
        {
            mainWindow.Register(sender, e);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            mainWindow.Login(login.Text, password.Password);
        }
    }
}
