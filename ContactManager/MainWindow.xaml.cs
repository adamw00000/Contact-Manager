using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using EmailContactsExtension;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Windows.Media.Effects;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        EmailContactsExtension.ContactManager contactManager = new EmailContactsExtension.ContactManager();
        System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
        System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

        User user;
        ObservableCollection<Contact> userContacts;

        public MainWindow()
        {
            InitializeComponent();
            

            accountPanel.Content = new UnloggedView()
            {
                mainWindow = this
            };
        }

        private void Import(object sender, RoutedEventArgs e)
        {
            openFileDialog.AddExtension = true;
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.FileName = "Users.xml";
            openFileDialog.Filter = "XML filter | *.xml";

            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            using (System.IO.FileStream stream =
                new System.IO.FileStream(openFileDialog.FileName, System.IO.FileMode.Open))
            {
                try
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(List<User>));
                    List<User> users = serializer.ReadObject(stream) as List<User>;
                    contactManager.SetUsers(users);
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid XML contents", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "Users.xml";
            saveFileDialog.Filter = "XML filter | *.xml";

            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            using (System.IO.FileStream stream =
                new System.IO.FileStream(saveFileDialog.FileName, System.IO.FileMode.Create))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<User>));
                serializer.WriteObject(stream, contactManager.GetUsers());
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Login(string login, string password)
        {
            user = contactManager.GetUser(login, password);
            if (user == null)
            {
                MessageBox.Show("Wrong username/password", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            userContacts = new ObservableCollection<Contact>();
            foreach(var contact in user.GetContacts())
            {
                userContacts.Add(new Contact()
                {
                    Email = contact.Email,
                    Gender = contact.Gender,
                    Name = contact.Name,
                    Surname = contact.Surname,
                    Phone = contact.Phone
                });
            }

            DataContext = userContacts;
            accountPanel.Content = new LoggedView()
            {
                Username = login,
                MainWindow = this
            };
        }

        public void Register(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow
            {
                Owner = this
            };

            registerWindow.ShowInTaskbar = false;
            this.Opacity = 0.5;
            registerWindow.ShowDialog();
            this.Opacity = 1;
        }
        
        public void Remove(Contact contact)
        {
            userContacts.Remove(contact);
        }

        public void SaveContacts(object sender, RoutedEventArgs e)
        {
            user.SaveContacts(userContacts.ToList());
        }

        public void Logout(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            accountPanel.Content = new UnloggedView()
            {
                mainWindow = this
            };
            (accountPanel.Content as UnloggedView).login.Text = user.Login;
            (accountPanel.Content as UnloggedView).password.Password = user.Password;
        }
    }
}
