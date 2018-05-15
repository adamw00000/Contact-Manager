using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        EmailContactsExtension.ContactManager contactManager = new EmailContactsExtension.ContactManager();

        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = new UserData();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            if (login.Text.Length == 0)
            {
                MessageBox.Show("Login cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (email.Text.Length == 0)
            {
                MessageBox.Show("Email cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsValid(this as DependencyObject))
            {
                MessageBox.Show("Validation errors!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (password.Password.Length == 0)
            {
                MessageBox.Show("Password cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (password.Password != confirmPassword.Password)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            contactManager.RegisterUser(login.Text, password.Password);

            Close();
        }

        private bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(IsValid);
        }
    }

    public class UserData
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    
    public class LoginValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null || value as string == "")
                return new ValidationResult(false, "This field cannot be empty!");
            return ValidationResult.ValidResult;
        }
    }

    public class EmailValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == "")
                return new ValidationResult(false, "This field cannot be empty!");

            string regexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Match matches = Regex.Match(value as string, regexPattern);
            return matches.Success ? ValidationResult.ValidResult : 
                new ValidationResult(false, "Incorrect email format!");
        }
    }
}
