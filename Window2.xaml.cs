using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logika interakcji dla klasy Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private int status;
        private string login;

        private delegate void Information();
        Information inf;

        public Window2(string login)
        {
            InitializeComponent();
            this.login = login;
            inf = IsUser;
            inf.Invoke();
        }

        private void IsUser()
        {
            Uzytkownik.Content = login;
            int status = Lista.getInstance.ChckStatus(login);
            if (status == 99)
            {
                Ranga.Content = "Administrator";
                this.status = 99;
            }
            else if (status == 1)
            {
                Ranga.Content = "Moderator";
                this.status = 1;
            }
            else
            {
                Ranga.Content = "Użytkownik";
            }
        }

        private void Manager_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }

        private void AdminPrivilage()
        {
            if (status == 99 | status ==1)
            {
                Window3 okno3 = new Window3(status);
                okno3.Show();
                Hide();
            }
            else
            {
                Creating.Content = "Brak uprawnień!";
            }
        }

        private void Wyloguj_click(object sender, RoutedEventArgs e)
        {
            inf = LoggingOut;
            inf.Invoke();
        }

        private void LoggingOut()
        {
            Window1 okno1 = new Window1();
            okno1.Show();
            Hide();
        }

        private void Creating_Click(object sender, RoutedEventArgs e)
        {
            inf = AdminPrivilage;
            inf.Invoke();
        }
    }
}
