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
                2 => "Vendeur"
            };
        }

        private void GererVisibiliteElements()
        {
            if (EmployeConnecte != null && EmployeConnecte.UnRole.NumRole == 1)
            {
                btnCreerPlat.Visibility = Visibility.Visible;
            }
            else
            {
                btnCreerPlat.Visibility = Visibility.Collapsed;
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

        private void TabButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                int index = int.Parse(tag);

                // Réinitialiser tous les boutons
                ResetAllButtons();

                // Cacher tous les UserControls
                CreerCommande.Visibility = Visibility.Collapsed;
                VoirCommandes.Visibility = Visibility.Collapsed;
                CreerPlat.Visibility = Visibility.Collapsed;
                VoirClient.Visibility = Visibility.Collapsed;

                // Mettre en surbrillance le bouton sélectionné et afficher le UserControl correspondant
                switch (index)
                {
                    case 0: // Créer Commande
                        SetActiveButton(btnCreerCommande);
                        CreerCommande.Visibility = Visibility.Visible;
                        break;
                    case 1: // Voir Commandes
                        SetActiveButton(btnVoirCommandes);
                        VoirCommandes.Visibility = Visibility.Visible;
                        break;
                    case 2: // Créer Plat
                        SetActiveButton(btnCreerPlat);
                        CreerPlat.Visibility = Visibility.Visible;
                        break;
                    case 3: // Clients
                        SetActiveButton(btnClients);
                        VoirClient.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void ResetAllButtons()
        {
            // Réinitialiser le style de tous les boutons
            SetInactiveButton(btnCreerCommande);
            SetInactiveButton(btnVoirCommandes);
            SetInactiveButton(btnCreerPlat);
            SetInactiveButton(btnClients);
        }

        private void SetActiveButton(Button button)
        {
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E30613"));
            button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E30613"));
            button.BorderThickness = new Thickness(2);
        }

        private void SetInactiveButton(Button button)
        {
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.Foreground = new SolidColorBrush(Colors.White);
            button.BorderBrush = new SolidColorBrush(Colors.White);
            button.BorderThickness = new Thickness(1);
        }
    }
}