using EmailContactsExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LoggedView.xaml
    /// </summary>
    public partial class LoggedView : UserControl
    {
        public string Username
        {
            get
            {
                return usernameLabel.Content as string;
            }
            set
            {
                usernameLabel.Content = value;
            }
        }
        public MainWindow MainWindow { get; set; }

        public LoggedView()
        {
            InitializeComponent();
        }

        private void SaveContacts(object sender, RoutedEventArgs e)
        {
            MainWindow.SaveContacts(sender, e);
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            MainWindow.Logout(sender, e);
        }

        private void contacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            contactInfo.Visibility = Visibility.Visible;
        }
        
        private void Remove(object sender, RoutedEventArgs e)
        {
            int oldIndex = contacts.SelectedIndex;

            MainWindow.Remove(contacts.SelectedItem as Contact);

            contacts.SelectedIndex = oldIndex > contacts.Items.Count - 1 ?
                contacts.Items.Count - 1 : oldIndex;
            if (contacts.Items.Count == 0)
                contactInfo.Visibility = Visibility.Collapsed;
        }
    }
}
