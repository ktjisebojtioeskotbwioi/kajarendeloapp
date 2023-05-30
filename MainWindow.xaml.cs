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
        public MainWindow()
        {
            InitializeComponent();
            User.GetUsers();
            if (User.currUser != null ) 
            {

            }
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            User.Login(unBox.Text, pwBox.Password);
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            User.OnExit();
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
