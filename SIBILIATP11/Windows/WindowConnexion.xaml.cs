using SIBILIATP11.Classe;
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

namespace SIBILIATP11.Windows
{
    /// <summary>
    /// Logique d'interaction pour WindowConnexion.xaml
    /// </summary>
    public partial class WindowConnexion : Window
    {
        private Dictionary<string, string> utilisateurs = new();

        public WindowConnexion()
        {
            ChargerPasswordEtLogin();
            InitializeComponent();
        }

        private void ButSeConnecter_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text;
            string mdp = TxtPassword.Password;

            if (utilisateurs.ContainsKey(login) && utilisateurs[login] == mdp)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                TxtLogin.BorderBrush = Brushes.Red;
                TxtPassword.BorderBrush = Brushes.Red;
            }
        }
        private void ChargerPasswordEtLogin()
        {
            Employe e = new Employe();
            List<Employe> tousLesEmployes = e.FindAll();

            utilisateurs = tousLesEmployes.ToDictionary(emp => emp.Login, emp => emp.Password);
        }
        private void ButFermer_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TxtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButSeConnecter_Click(ButSeConnecter, new RoutedEventArgs());
            }
        }
    }
}
