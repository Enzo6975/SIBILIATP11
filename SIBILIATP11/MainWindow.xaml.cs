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
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
            Window_Loaded();
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
                string nomRole = ObtenirNomRole(EmployeConnecte.UnRole.NumRole);
                TxtBlockConnexion.Text = $"Connecté en tant que -\n{EmployeConnecte.PrenomEmploye} {EmployeConnecte.NomEmploye}\n({nomRole})";
                GererVisibiliteElements();
            }
            else
            {
                TxtBlockConnexion.Text = "Connecté en tant que -\nUtilisateur inconnu";
                GererVisibiliteElements();
            }
        }

        private string ObtenirNomRole(int numRole)
        {
            return numRole switch
            {
                1 => "Responsable des Ventes",
                2 => "Vendeur",
                _ => "Rôle inconnu"
            };
        }

        private void GererVisibiliteElements()
        {
            if (EmployeConnecte != null && EmployeConnecte.UnRole.NumRole == 1)
            {
                TabCreerPlat.Visibility = Visibility.Visible;
            }
            else
            {
                TabCreerPlat.Visibility = Visibility.Collapsed;

                if (TabCreerPlat.IsSelected)
                {
                    TabVoirCommandes.IsSelected = true;
                }
            }
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source != MainTabControl) return;

            CreerCommande.Visibility = Visibility.Collapsed;
            VoirCommandes.Visibility = Visibility.Collapsed;
            CreerPlat.Visibility = Visibility.Collapsed;
            VoirClient.Visibility = Visibility.Collapsed;

            if (MainTabControl.SelectedItem == TabCreerCommande)
            {
                CreerCommande.Visibility = Visibility.Visible;
            }
            else if (MainTabControl.SelectedItem == TabVoirCommandes)
            {
                VoirCommandes.Visibility = Visibility.Visible;
            }
            else if (MainTabControl.SelectedItem == TabCreerPlat)
            {
                CreerPlat.Visibility = Visibility.Visible;
            }
            else if (MainTabControl.SelectedItem == TabClients)
            {
                VoirClient.Visibility = Visibility.Visible;
            }
        }

        private void ButDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            EmployeConnecte = null;
            Window_Loaded();
        }
    }
}