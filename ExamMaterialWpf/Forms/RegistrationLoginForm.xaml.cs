using ExamMaterialWpf.ServiceReference1;
using System;
using System.Collections.Generic;
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

namespace ExamMaterialWpf
{

    /// <summary>
    /// Логика взаимодействия для RegistrationLoginForm.xaml
    /// </summary>
    public partial class RegistrationLoginForm : Window, IServiceCallback
    {
        static ServiceClient _proxy;
        public RegistrationLoginForm()
        {
            InitializeComponent();
            _proxy = new ServiceClient(new InstanceContext(this));
            
        }

        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private  void bSignIn_Click(object sender, RoutedEventArgs e)
        {
            User currentUser =  _proxy.SignIn(tbSignInEmail.Text, tbSignInPassword.Password);

            if (currentUser!=null)
            {
                MainWindow mainWindow = new MainWindow(currentUser);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong data!");
            }
        }

        private void bSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (tbSignUpPassword.Text != "" && tbSignUpLN.Text != "" && tbSignUpEmail.Text != "" && tbSignUpFN.Text != "")
            {
                if (_proxy.RegisterUser(tbSignUpFN.Text, tbSignUpLN.Text, tbSignUpEmail.Text, tbSignUpPassword.Text))
                {
                    MessageBox.Show("Регистрация прошла успешно!");
                }
                else
                {
                    MessageBox.Show("Email занят");
                }
            }
            else
            {
                MessageBox.Show("Проверьте правильность ввода!");
            }
        }

        public void UpdateListClients(Client[] clients)
        {
            throw new NotImplementedException();
        }

        public void UpdateChat(string nameFrom, string message)
        {
            throw new NotImplementedException();
        }

        public void UpdateChat(string nameFrom, string message, string image)
        {
            throw new NotImplementedException();
        }
    }
}

