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
            if (User.currUser == null) 
            {
                new User.LoginWindow();
                a = 1;
                Close();
            }
            else
            {
                Title = "Rendelés (Felhasználó: " + User.currUser.UserName + ")";
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (a != 1)
            {
                User.OnExit();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
