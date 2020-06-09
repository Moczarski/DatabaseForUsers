using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace WPF_test
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string password;
        public string login;

        private delegate void MainDelegate();
        MainDelegate MD;

        public Window1()
        {
            InitializeComponent();
            MD = InitializeUsers;
            MD.Invoke();
        }

        private void InitializeUsers()
        {
            if (File.Exists("MM8983.bin"))
            {
                using (Stream checking = File.Open("MM8983.bin", FileMode.Open))
                {
                    BinaryFormatter database = new BinaryFormatter();
                    var list = (List<User>)database.Deserialize(checking);
                    Lista.getInstance.ListedUsers(list);
                }
            }
            else
            {
                Lista.getInstance.CheckDatabase();
            }
        }

        private void AuthorizeUser()
        {
            password = Haslo_box.Password;
            login = Login_box.Text;
            bool score = Lista.getInstance.Checking(login, password);
            if (!score)
            {
                In_psw.Content = "Nieprawidłowe hasło!";
                Haslo_box.Password = "";
            }
            else
            {
                Window2 okno2 = new Window2(login);
                okno2.Show();
                Hide();
            }
        }

        private void Zaloguj_Click(object sender, RoutedEventArgs e)
        {
            MD = AuthorizeUser;
            MD.Invoke();
        }

        private void Manager_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
