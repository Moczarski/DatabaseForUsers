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
    /// Logika interakcji dla klasy Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        private int status;

        private delegate void ModifyUser();
        ModifyUser MU;

        public Window3(int status)
        {
            InitializeComponent();
            this.status = status;
            MU = RefreshList;
            MU.Invoke();
        }

        public void RefreshList()
        {
            if (File.Exists("MM8983.bin"))
            {
                using (Stream checking = File.Open("MM8983.bin", FileMode.Open))
                {
                    BinaryFormatter database = new BinaryFormatter();
                    var list = (List<User>)database.Deserialize(checking);
                    Lista.getInstance.ListedUsers(list);
                    users_box.Items.Clear();
                    MU = ListToString;
                    MU.Invoke();
                }
            }
        }

        private void ListToString()
        {
            string ID;
            string login;
            string passw;
            string status;
            string list = "";
            int i = 1;
            list = "Nr___ID___Login___Hasło___Status";
            users_box.Items.Add(list);
            foreach (User user in Lista.getInstance.database)
            {
                ID = user.IDUser.ToString();
                login = user.Login;
                passw = user.Passw;
                status = user.status.ToString();
                list = i.ToString() + ". " + ID + " " + login + " " + passw + " " + status;
                users_box.Items.Add(list);
                i++;
            }
        }

        private void LetsClean()
        {
            New_login.Text = "";
            New_password.Password = "";
            radio_admin.IsChecked = false;
            radio_mod.IsChecked = false;
            radio_user.IsChecked = true;
        }

        private void NewUser()
        {
            bool status = Lista.getInstance.ChckLogin(New_login.Text);

            if (status)
            {
                Login_exist.Content = "Login już istnieje!!";
                New_login.Text = "";
            }
            else if (New_login.Text.Equals("") || New_password.Password.Equals(""))
            {
                Login_exist.Content = "Pola nie mogą być puste!";
            }
            else
            {
                int ranga = 0;
                if(radio_admin.IsChecked == true)
                {
                    ranga = 99;
                }
                else if (radio_mod.IsChecked == true)
                {
                    ranga = 1;
                }
                else if (radio_user.IsChecked == true)
                {
                    ranga = 0;
                }
                else
                {
                    Login_exist.Content = "Wybierz rangę!";
                }
                Lista.getInstance.NewUser(New_login.Text, New_password.Password, ranga);
                Lista.getInstance.CheckDatabase();
                MU = RefreshList;
                MU.Invoke();
                Login_exist.Content = "Utworzono użytkownika " + New_login.Text + "!";
                MU = LetsClean;
                MU.Invoke();
            }
        }

        private void DeleteUser()
        {
            bool status = Lista.getInstance.ChckLogin(New_login.Text);

            if (status)
            {
                Lista.getInstance.DelUser(New_login.Text);
                Lista.getInstance.CheckDatabase();
                MU = RefreshList;
                MU.Invoke();
                Login_exist.Content = "Usunięto użytkownika " + New_login.Text + "!";
                MU = LetsClean;
                MU.Invoke();
            }
            else if (New_login.Text.Equals(""))
            {
                Login_exist.Content = "Podaj nazwę użytkownika!";
            }
            else
            {
                Login_exist.Content = "Wpisz istniejący login!";
                MU = LetsClean;
                MU.Invoke();
            }
        }

        private void Manager_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }

        private void Delete_user_Click(object sender, RoutedEventArgs e)
        {
            if(status == 99)
            {
                MU = DeleteUser;
                MU.Invoke();
            }
            else
            {
                Login_exist.Content = "Brak uprawnień!";
            }
        }

        private void Create_user(object sender, RoutedEventArgs e)
        {
            if (status == 99)
            {
                MU = NewUser;
                MU.Invoke();
            }
            else
            {
                Login_exist.Content = "Brak uprawnień!";
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            MU = LoggingOut;
            MU.Invoke();
        }

        private void LoggingOut()
        {
            Window1 okno1 = new Window1();
            okno1.Show();
            Hide();
        }

        private void Clear_click(object sender, RoutedEventArgs e)
        {
            MU = LetsClean;
            MU.Invoke();
        }

        private void Change_password(object sender, RoutedEventArgs e)
        {
            MU = ChangePsw;
            MU.Invoke();
        }

        private void ChangePsw()
        {
            bool status = Lista.getInstance.ChckLogin(New_login.Text);

            if (status)
            {
                Lista.getInstance.PswUser(New_login.Text, New_password.Password);
                Lista.getInstance.CheckDatabase();
                MU = RefreshList;
                MU.Invoke();
                Login_exist.Content = "Zmieniono hasło użytkownika " + New_login.Text + "!";
                MU = LetsClean;
                MU.Invoke();
            }
            else if (New_login.Text.Equals("") || New_password.Password.Equals(""))
            {
                Login_exist.Content = "Pola nie mogą być puste!";
            }
            else
            {
                Login_exist.Content = "Wpisz istniejący login!";
                MU = LetsClean;
                MU.Invoke();
            }
        }
    }
}
