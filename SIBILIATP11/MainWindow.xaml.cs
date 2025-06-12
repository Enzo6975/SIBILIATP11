using SIBILIATP11.Classe;
using SIBILIATP11.Windows;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIBILIATP11
{
    public partial class MainWindow : Window
    {
        public GestionCommande LaGestion { get; set; }
        public Employe EmployeConnecte { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ChargeData();
            Window_Loaded();
        }

        public void Window_Loaded()
        {
            this.Hide();
            WindowConnexion dialogwindowmc = new WindowConnexion();
            bool? resultmc = dialogwindowmc.ShowDialog();

            if (resultmc == true)
            {
                EmployeConnecte = dialogwindowmc.EmployeConnecte;
                MettreAJourAffichageConnexion();
                this.Show();
            }
            else if (resultmc == false)
            {
                Application.Current.Shutdown();
            }
        }

        private void MettreAJourAffichageConnexion()
        {
            if (EmployeConnecte != null)
            {
                TxtBlockConnexion.Text = $"Connecté en tant que -\n{EmployeConnecte.PrenomEmploye} {EmployeConnecte.NomEmploye}";
            }
            else
            {
                TxtBlockConnexion.Text = "Connecté en tant que -\nUtilisateur inconnu";
            }
        }

        public void ChargeData()
        {
            try
            {
                LaGestion = new GestionCommande("Application Sibilia");
                this.DataContext = LaGestion;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                LaGestion = new GestionCommande("Application Sibilia - Mode dégradé");
                this.DataContext = LaGestion;
            }
        }

        private void ButDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            EmployeConnecte = null;
            Window_Loaded();
        }
    }
}