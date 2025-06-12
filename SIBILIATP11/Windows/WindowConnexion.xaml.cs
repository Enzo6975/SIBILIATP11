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
    public partial class WindowConnexion : Window
    {
        private Dictionary<string, string> utilisateurs = new();
        private List<Employe> tousLesEmployes = new();

        public Employe EmployeConnecte { get; private set; }

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
                EmployeConnecte = tousLesEmployes.FirstOrDefault(emp => emp.Login == login);

                DialogResult = true;
                Close();
            }
            else
            {
                TxtLogin.BorderBrush = Brushes.Red;
                TxtPassword.BorderBrush = Brushes.Red;
                MessageBox.Show("Identifiant ou mot de passe incorrect", "Erreur de connexion",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ChargerPasswordEtLogin()
        {
            try
            {
                Employe e = new Employe();
                tousLesEmployes = e.FindAll();

                foreach (var employe in tousLesEmployes)
                {
                    employe.Read();
                }

                utilisateurs = tousLesEmployes.ToDictionary(emp => emp.Login, emp => emp.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des employés: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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