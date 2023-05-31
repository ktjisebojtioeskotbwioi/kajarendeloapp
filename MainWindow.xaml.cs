using System.Windows;

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
        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.LogOut())
            {
                new User.LoginWindow();
                a = 1;
                Close();
            }
        }
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            User.EditUser();
        }
        private void ordersButton_Click(object sender, RoutedEventArgs e)
        {
            new Food.OrderGridWindow();
        }
        private void termekgrid1_Loaded(object sender, RoutedEventArgs e)
        {
            termekgrid1.ItemsSource = Food.foods;
        }
        private void basket_Click(object sender, RoutedEventArgs e)
        {
            string amount = new User.InputWindow("Mennyit szeretnél rendelni?", true).ShowDialog();
            if (amount != null && int.TryParse(amount, out _))
            {
                if (MessageBox.Show("Biztos vagy benne, hogy meg akarsz rendelni " + amount + "db " + Food.foods[termekgrid1.SelectedIndex].Name + "-t/-át/-ét?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    new Food.Order(Food.foods[termekgrid1.SelectedIndex].ID, amount);
                }
            }
            else
            {
                MessageBox.Show("Adj meg egy számot!");
                return;
            }
        }
    }
}
