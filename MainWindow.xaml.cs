using System;
using System.Windows;

namespace kajarendeloapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //static bool isFirstRun = true;
        public MainWindow()
        {
            InitializeComponent();
            User.GetUsers();
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            User.Login(unBox.Text, pwBox.Password);
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            new User(unBox.Text, pwBox.Password, emailBox.Text);
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            User.LogOut();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            User.EditUser();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            User.OnExit();
            Environment.Exit(0);
        }
    }
}
