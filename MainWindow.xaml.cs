using System;
using System.Windows;
using System.Windows.Controls;

namespace kajarendeloapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int a = 0;
        public MainWindow()
        {
            InitializeComponent();
            User.GetUsers();
            if (User.currUser == null ) 
            {
                new User.LoginWindow();
                a = 1;
                Close();
            }
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            User.Login(unBox.Text, pwBox.Password);
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            User.OnExit();
            User.LogOut(true);
            if (a != 1)
            {
                Environment.Exit(0);
            }
        }

        private void checkBox1_Changed(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                usernamelabel.Visibility = Visibility.Hidden;
                emailcimlabel.Visibility = Visibility.Visible;
            }
            else
            {
                usernamelabel.Visibility = Visibility.Visible;
                emailcimlabel.Visibility = Visibility.Hidden;
            }
        }
    }
}
