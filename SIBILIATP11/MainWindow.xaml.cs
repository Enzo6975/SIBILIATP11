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

                // Initialiser l'affichage par défaut
                InitialiserAffichageParDefaut();
            }
            else if (resultmc == false)
            {
                Application.Current.Shutdown();
            }
        }

        private void InitialiserAffichageParDefaut()
        {
            try
            {
                // Trouver les contrôles par leur nom
                var btnVoirCommandes = FindName("BtnVoirCommandes") as Button;

                if (btnVoirCommandes != null)
                {
                    ShowUserControl("VoirCommandes");
                    UpdateNavigationButtons(btnVoirCommandes);
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, ne pas planter l'application
                System.Diagnostics.Debug.WriteLine($"Erreur initialisation: {ex.Message}");
            }
        }

        private void MettreAJourAffichageConnexion()
        {
            try
            {
                var txtBlockConnexion = FindName("TxtBlockConnexion") as TextBlock;

                if (txtBlockConnexion != null)
                {
                    if (EmployeConnecte != null)
                    {
                        string nomRole = ObtenirNomRole(EmployeConnecte.UnRole.NumRole);
                        txtBlockConnexion.Text = $"Connecté en tant que -\n{EmployeConnecte.PrenomEmploye} {EmployeConnecte.NomEmploye}\n({nomRole})";
                    }
                    else
                    {
                        txtBlockConnexion.Text = "Connecté en tant que -\nUtilisateur inconnu";
                    }
                }

                GererVisibiliteElements();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur affichage connexion: {ex.Message}");
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
            try
            {
                var btnCreerPlat = FindName("BtnCreerPlat") as Button;
                var creerPlat = FindName("CreerPlat") as UserControl.CreerPlat;

                if (btnCreerPlat != null)
                {
                    if (EmployeConnecte != null && EmployeConnecte.UnRole.NumRole == 1)
                    {
                        // Responsable : accès à tous les boutons
                        btnCreerPlat.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        // Vendeur : masquer le bouton "Créer un plat"
                        btnCreerPlat.Visibility = Visibility.Collapsed;

                        // Si l'utilisateur était sur "Créer un plat", le rediriger
                        if (creerPlat != null && creerPlat.Visibility == Visibility.Visible)
                        {
                            ShowUserControl("VoirCommandes");
                            var btnVoirCommandes = FindName("BtnVoirCommandes") as Button;
                            if (btnVoirCommandes != null)
                                UpdateNavigationButtons(btnVoirCommandes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur gestion visibilité: {ex.Message}");
            }
        }

        #region Navigation par boutons

        private void BtnCreerCommande_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("CreerCommande");
            UpdateNavigationButtons(sender as Button);
        }

        private void BtnVoirCommandes_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("VoirCommandes");
            UpdateNavigationButtons(sender as Button);
        }

        private void BtnCreerPlat_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("CreerPlat");
            UpdateNavigationButtons(sender as Button);
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("VoirClient");
            UpdateNavigationButtons(sender as Button);
        }

        private void ShowUserControl(string controlName)
        {
            try
            {
                // Trouver les contrôles par leur nom
                var creerCommande = FindName("CreerCommande") as FrameworkElement;
                var voirCommandes = FindName("VoirCommandes") as FrameworkElement;
                var creerPlat = FindName("CreerPlat") as FrameworkElement;
                var voirClient = FindName("VoirClient") as FrameworkElement;

                // Masquer tous les UserControls
                if (creerCommande != null) creerCommande.Visibility = Visibility.Collapsed;
                if (voirCommandes != null) voirCommandes.Visibility = Visibility.Collapsed;
                if (creerPlat != null) creerPlat.Visibility = Visibility.Collapsed;
                if (voirClient != null) voirClient.Visibility = Visibility.Collapsed;

                // Afficher le UserControl demandé
                switch (controlName)
                {
                    case "CreerCommande":
                        if (creerCommande != null) creerCommande.Visibility = Visibility.Visible;
                        break;
                    case "VoirCommandes":
                        if (voirCommandes != null) voirCommandes.Visibility = Visibility.Visible;
                        break;
                    case "CreerPlat":
                        if (creerPlat != null) creerPlat.Visibility = Visibility.Visible;
                        break;
                    case "VoirClient":
                        if (voirClient != null) voirClient.Visibility = Visibility.Visible;
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur affichage UserControl: {ex.Message}");
            }
        }

        private void UpdateNavigationButtons(Button selectedButton)
        {
            try
            {
                // Trouver tous les boutons de navigation
                var btnCreerCommande = FindName("BtnCreerCommande") as Button;
                var btnVoirCommandes = FindName("BtnVoirCommandes") as Button;
                var btnCreerPlat = FindName("BtnCreerPlat") as Button;
                var btnClients = FindName("BtnClients") as Button;

                // Retirer la sélection de tous les boutons
                if (btnCreerCommande != null) btnCreerCommande.Tag = null;
                if (btnVoirCommandes != null) btnVoirCommandes.Tag = null;
                if (btnCreerPlat != null) btnCreerPlat.Tag = null;
                if (btnClients != null) btnClients.Tag = null;

                // Marquer le bouton sélectionné
                if (selectedButton != null)
                    selectedButton.Tag = "Selected";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur mise à jour boutons: {ex.Message}");
            }
        }

        #endregion

        private void ButDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            EmployeConnecte = null;
            Window_Loaded();
        }
    }
}