using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace kajarendeloapp
{
    public class Food
    {
        public static List<Food> foods { get; private protected set; } = new List<Food>();
        private protected string id;
        public string ID { get => id; private protected set => id = value; }
        private protected string name;
        public string Name { get => name; private protected set => name = value; }
        private protected string cost;
        public string Cost { get => cost; private protected set => cost = value; }
        private protected string desc;
        public string Desc { get => desc; private protected set => desc = value; }
        public Food(string name, string cost, string desc)
        {
            try
            {
                if (name.Contains(';') || cost.Contains(';') || !int.TryParse(cost, out int g) || desc.Contains(';') || name == null || cost == null || desc == null)
                {
                    throw new Exception("Sikertelen hozzáadás");
                }
                this.name = name;
                this.cost = cost;
                this.desc = desc;
                File.SetAttributes("foods.txt", FileAttributes.Normal);
                id = File.ReadAllLines("foods.txt").Length.ToString();
                foods.Add(this);
                if (File.ReadAllLines("foods.txt").Length == 0)
                {
                    File.AppendAllText("foods.txt", ID + ";" + Name + ";" + Cost + ";" + Desc + ";");
                }
                else
                {
                    File.AppendAllText("foods.txt", "\r\n" + ID + ";" + Name + ";" + Cost + ";" + Desc + ";");
                }
                File.SetAttributes("foods.txt", FileAttributes.Hidden);
            }
            catch (Exception ex)
            {
                File.SetAttributes("foods.txt", FileAttributes.Hidden);
                MessageBox.Show(ex.Message);
            }

        }
        private protected Food(string id, string name, string cost, string desc)
        {
            this.id = id;
            this.name = name;
            this.cost = cost;
            this.desc = desc;
        }
        public static void GetFoods()
        {
            try
            {
                if (File.Exists("foods.txt"))
                {
                    File.SetAttributes("foods.txt", FileAttributes.Normal);
                    string[] x = File.ReadAllLines("foods.txt");
                    if (x.Length > 0)
                    {
                        foods.Clear();
                        for (int i = 0; i < x.Length; i++)
                        {
                            foods.Add(new Food(x[i].Split(';')[0], x[i].Split(';')[1], x[i].Split(';')[2], x[i].Split(';')[3]));
                        }
                    }
                    File.SetAttributes("foods.txt", FileAttributes.Hidden);
                }
                else
                {
                    FileStream f1 = File.Open("foods.txt", FileMode.OpenOrCreate);
                    File.SetAttributes(f1.Name, FileAttributes.Hidden);
                    f1.Close();
                    new Food("Margherita pizza", "1600", "paradicsom, sajt");
                    new Food("Salamé pizza", "1900", "paradicsom, sajt, szalámi");
                    new Food("Bolognese pizza", "2100", "bolognai ragu, parmezán, sajt");
                    new Food("Magyaros pizza", "2300", "paradicsom, csípős kolbász, ff.tarja, hagyma, hegyes erős, sajt");
                    new Food("Gyrosos pizza", "2700", "tejfölös alap, gyros hús, paradicsom, lilahagyma, kukorica, kígyóuborka, sajt, fokhagyma");
                    new Food("Coca Cola", "300", "0,5l");
                }
                Order.GetOrders();
            }
            catch
            {
                File.SetAttributes("foods.txt", FileAttributes.Hidden);
            }
        }
        public class Order
        {
            public static List<Order> orders { get; private protected set; } = new List<Order>();
            private protected string transactionid;
            public string TransactionID { get => transactionid; private protected set => transactionid = value; }
            private protected string userid;
            public string userID { get => userid; private protected set => userid = value; }
            private protected string itemid;
            public string itemID { get => itemid; private protected set => itemid = value; }
            private protected string amount;
            public string Amount { get => amount; private protected set => amount = value; }
            private protected string cost;
            public string Cost { get => cost; private protected set => cost = value; }
            private protected string time;
            public string Time { get => time; private protected set => time = value; }
            private protected string itemname;
            public string ItemName { get => itemname; private protected set => itemname = value; }
            public Order(string itemid, string amount)
            {
                try
                {
                    if (!int.TryParse(amount, out int x))
                    {
                        throw new Exception("Sikertelen rendelés!");
                    }
                    userid = User.currUser.ID;
                    this.itemid = itemid;
                    this.amount = amount;
                    cost = (Convert.ToInt32(foods[Convert.ToInt32(itemid)].cost) * Convert.ToInt32(amount)).ToString();
                    File.SetAttributes("orders.txt", FileAttributes.Normal);
                    transactionid = File.ReadAllLines("orders.txt").Length.ToString();
                    time = DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss");
                    itemname = foods[Convert.ToInt32(itemID)].Name;
                    orders.Add(this);
                    if (File.ReadAllLines("orders.txt").Length == 0)
                    {
                        File.AppendAllText("orders.txt", TransactionID + ";" + userID + ";" + itemID + ";" + Amount + ";" + Cost + ";" + Time + ";" + ItemName + ";");
                    }
                    else
                    {
                        File.AppendAllText("orders.txt", "\r\n" + TransactionID + ";" + userID + ";" + itemID + ";" + Amount + ";" + Cost + ";" + Time + ";" + ItemName + ";");
                    }
                    File.SetAttributes("orders.txt", FileAttributes.Hidden);
                    MessageBox.Show("Sikeres rendelés!");
                }
                catch (Exception ex)
                {
                    File.SetAttributes("orders.txt", FileAttributes.Hidden);
                    MessageBox.Show(ex.Message);
                }
            }
            private protected Order(string tid, string userid, string itemid, string amount, string cost, string time, string name)
            {
                transactionid = tid;
                this.userid = userid;
                this.itemid = itemid;
                this.amount = amount;
                this.cost = cost;
                this.time = time;
                itemname = name;
            }
            internal protected static void GetOrders()
            {
                try
                {
                    if (File.Exists("orders.txt"))
                    {
                        File.SetAttributes("orders.txt", FileAttributes.Normal);
                        string[] x = File.ReadAllLines("orders.txt");
                        if (x.Length > 0)
                        {
                            orders.Clear();
                            for (int i = 0; i < x.Length; i++)
                            {
                                orders.Add(new Order(x[i].Split(';')[0], x[i].Split(';')[1], x[i].Split(';')[2], x[i].Split(';')[3], x[i].Split(';')[4], x[i].Split(';')[5], x[i].Split(';')[6]));
                            }
                        }
                        File.SetAttributes("orders.txt", FileAttributes.Hidden);
                    }
                    else
                    {
                        FileStream f1 = File.Open("orders.txt", FileMode.OpenOrCreate);
                        File.SetAttributes(f1.Name, FileAttributes.Hidden);
                        f1.Close();
                    }
                }
                catch
                {
                    File.SetAttributes("orders.txt", FileAttributes.Hidden);
                }
            }
        }
        public class OrderGridWindow : Window
        {
            Window window = new Window();
            DataGrid grid = new DataGrid();
            StackPanel panel = new StackPanel();
            private DataGridTextColumn TIDColumn = new DataGridTextColumn() { Header = "Tranzakciós ID", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTextColumn userIDColumn = new DataGridTextColumn() { Header = "Felhasználó ID", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTextColumn itemIDColumn = new DataGridTextColumn() { Header = "Étel ID", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTextColumn itemNameColumn = new DataGridTextColumn() { Header = "Étel neve", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTextColumn AmountColumn = new DataGridTextColumn() { Header = "Mennyiség", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTextColumn CostColumn = new DataGridTextColumn() { Header = "Ár (ft)", Width = DataGridLength.Auto, IsReadOnly = true };
            private DataGridTextColumn TimeColumn = new DataGridTextColumn() { Header = "Időpont", Width = DataGridLength.Auto, IsReadOnly = true };
            public OrderGridWindow()
            {
                window.Width = 800;
                window.Height = 350;
                window.Background = Brushes.Wheat;
                grid.Background = Brushes.Cornsilk;
                grid.RowBackground = Brushes.Beige;
                grid.AutoGenerateColumns = false;
                grid.Width = 800;
                grid.Height = 350;
                grid.Margin = new Thickness(0, 0, 0, 0);
                grid.AlternatingRowBackground = Brushes.Beige;
                grid.DataContext = Order.orders;
                Binding b = new Binding();
                b.Mode = BindingMode.OneWay;
                b = new Binding("TransactionID");
                TIDColumn.Binding = b;
                b = new Binding("userID");
                userIDColumn.Binding = b;
                b = new Binding("itemID");
                itemIDColumn.Binding = b;
                b = new Binding("ItemName");
                itemNameColumn.Binding = b;
                b = new Binding("Amount");
                AmountColumn.Binding = b;
                b = new Binding("Cost");
                CostColumn.Binding = b;
                b = new Binding("Time");
                TimeColumn.Binding = b;
                grid.Columns.Add(TIDColumn);
                if (User.currUser.Perms == "a")
                {
                    grid.ItemsSource = Order.orders;
                    grid.Columns.Add(userIDColumn);
                    window.Title = "Összes rendelés (Felhasználó: " + User.currUser.UserName + ")";
                }
                else
                {
                    grid.ItemsSource = Order.orders.Where(n => n.userID == User.currUser.ID);
                    window.Title = "Rendeléseim (Felhasználó: " + User.currUser.UserName + ")";
                }
                grid.Columns.Add(itemIDColumn);
                grid.Columns.Add(itemNameColumn);
                grid.Columns.Add(AmountColumn);
                grid.Columns.Add(CostColumn);
                grid.Columns.Add(TimeColumn);
                grid.HorizontalAlignment = HorizontalAlignment.Left;
                panel.Children.Add(grid);
                window.Content = panel;
                window.Show();
            }
        }
    }
}
