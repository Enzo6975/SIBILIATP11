using SIBILIATP11.Classe;
using SIBILIATP11.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var btnVoirCommandes = FindName("BtnVoirCommandes") as Button;

                if (btnVoirCommandes != null)
                {
                    ShowUserControl("VoirCommandes");
                    UpdateNavigationButtons(btnVoirCommandes);
                }
            }
            catch (Exception ex)
            {
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
                        btnCreerPlat.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnCreerPlat.Visibility = Visibility.Collapsed;

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

        #region Navigation par boutons avec rafraîchissement

        private void BtnCreerCommande_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("CreerCommande");
            UpdateNavigationButtons(sender as Button);
            RefreshUserControl("CreerCommande");
        }

        private void BtnVoirCommandes_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("VoirCommandes");
            UpdateNavigationButtons(sender as Button);
            RefreshUserControl("VoirCommandes");
        }

        private void BtnCreerPlat_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("CreerPlat");
            UpdateNavigationButtons(sender as Button);
            RefreshUserControl("CreerPlat");
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            ShowUserControl("VoirClient");
            UpdateNavigationButtons(sender as Button);
            RefreshUserControl("VoirClient");
        }

        private void RefreshUserControl(string controlName)
        {
            try
            {
                switch (controlName)
                {
                    case "CreerCommande":
                        var creerCommande = FindName("CreerCommande") as UserControl.CreerCommande;
                        if (creerCommande != null)
                        {
                            RefreshCreerCommande(creerCommande);
                        }
                        break;

                    case "VoirCommandes":
                        var voirCommandes = FindName("VoirCommandes") as UserControl.VoirCommandes;
                        if (voirCommandes != null)
                        {
                            RefreshVoirCommandes(voirCommandes);
                        }
                        break;

                    case "CreerPlat":
                        var creerPlat = FindName("CreerPlat") as UserControl.CreerPlat;
                        if (creerPlat != null)
                        {
                            RefreshCreerPlat(creerPlat);
                        }
                        break;

                    case "VoirClient":
                        var voirClient = FindName("VoirClient") as UserControl.VoirClient;
                        if (voirClient != null)
                        {
                            RefreshVoirClient(voirClient);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rafraîchissement {controlName}: {ex.Message}");
            }
        }

        private void RefreshCreerCommande(UserControl.CreerCommande control)
        {
            try
            {
                // Utiliser la méthode existante RechargerDonnees
                LaGestion.RechargerDonnees();

                // Mettre à jour le DataContext
                this.DataContext = LaGestion;

                // Rafraîchir la liste des plats
                var platsDataGrid = control.FindName("plats") as DataGrid;
                if (platsDataGrid != null)
                {
                    platsDataGrid.ItemsSource = null;
                    platsDataGrid.ItemsSource = LaGestion.LesPlats;

                    // Réappliquer le filtre
                    CollectionViewSource.GetDefaultView(platsDataGrid.ItemsSource).Refresh();
                }

                // Mettre à jour les ComboBoxes
                var cbCategorie = control.FindName("cbCategorie") as ComboBox;
                if (cbCategorie != null)
                {
                    var categoriesAvecTous = new List<object>();
                    categoriesAvecTous.Add(new { NumCategorie = -1, NomCategorie = "Toutes les catégories" });
                    if (LaGestion.LesCategories != null)
                    {
                        foreach (var categorie in LaGestion.LesCategories)
                        {
                            categoriesAvecTous.Add(categorie);
                        }
                    }
                    cbCategorie.ItemsSource = categoriesAvecTous;
                    cbCategorie.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RefreshCreerCommande: {ex.Message}");
            }
        }

        private void RefreshVoirCommandes(UserControl.VoirCommandes control)
        {
            try
            {
                // Utiliser la nouvelle méthode de rafraîchissement
                control.RafraichirDepuisExterieur();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RefreshVoirCommandes: {ex.Message}");

                // Fallback : méthode manuelle
                try
                {
                    LaGestion.RechargerDonnees();
                    this.DataContext = LaGestion;

                    var dgCommandes = control.FindName("dgCommandes") as DataGrid;
                    if (dgCommandes != null)
                    {
                        dgCommandes.ItemsSource = null;
                        dgCommandes.ItemsSource = LaGestion.LesCommandes;
                        CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Erreur fallback RefreshVoirCommandes: {ex2.Message}");
                }
            }
        }

        private void RefreshCreerPlat(UserControl.CreerPlat control)
        {
            try
            {
                // Utiliser la méthode existante RechargerDonnees
                LaGestion.RechargerDonnees();

                // Mettre à jour le DataContext
                this.DataContext = LaGestion;

                // Utiliser la nouvelle méthode de rafraîchissement du contrôle
                control.RafraichirDonnees();

                System.Diagnostics.Debug.WriteLine("RefreshCreerPlat terminé");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RefreshCreerPlat: {ex.Message}");

                // Fallback : essayer de recharger manuellement
                try
                {
                    var cbSousCategorie = control.FindName("cbSousCategorie") as ComboBox;
                    if (cbSousCategorie != null)
                    {
                        cbSousCategorie.ItemsSource = LaGestion.LesSousCategories;
                    }

                    var cbPeriode = control.FindName("cbPeriode") as ComboBox;
                    if (cbPeriode != null)
                    {
                        cbPeriode.ItemsSource = LaGestion.LesPeriodes;
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Erreur fallback RefreshCreerPlat: {ex2.Message}");
                }
            }
        }

        private void RefreshVoirClient(UserControl.VoirClient control)
        {
            try
            {
                // Appeler la méthode de rafraîchissement si elle existe
                if (control != null)
                {
                    // Utiliser la réflexion pour appeler la méthode RafraichirDonnees
                    var method = control.GetType().GetMethod("RafraichirDonnees");
                    if (method != null)
                    {
                        method.Invoke(control, null);
                    }
                    else
                    {
                        // Alternative : recharger manuellement les données
                        var clientsDataGrid = control.FindName("clients") as DataGrid;
                        if (clientsDataGrid != null)
                        {
                            // Recharger les clients depuis la base
                            var nouveauxClients = new Client().FindAll();
                            clientsDataGrid.ItemsSource = nouveauxClients;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RefreshVoirClient: {ex.Message}");

                // Fallback : essayer de recharger manuellement
                try
                {
                    var clientsDataGrid = control?.FindName("clients") as DataGrid;
                    if (clientsDataGrid != null)
                    {
                        var nouveauxClients = new Client().FindAll();
                        clientsDataGrid.ItemsSource = nouveauxClients;
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Erreur fallback RefreshVoirClient: {ex2.Message}");
                }
            }
        }

        #endregion

        private void ShowUserControl(string controlName)
        {
            try
            {
                var creerCommande = FindName("CreerCommande") as FrameworkElement;
                var voirCommandes = FindName("VoirCommandes") as FrameworkElement;
                var creerPlat = FindName("CreerPlat") as FrameworkElement;
                var voirClient = FindName("VoirClient") as FrameworkElement;

                if (creerCommande != null) creerCommande.Visibility = Visibility.Collapsed;
                if (voirCommandes != null) voirCommandes.Visibility = Visibility.Collapsed;
                if (creerPlat != null) creerPlat.Visibility = Visibility.Collapsed;
                if (voirClient != null) voirClient.Visibility = Visibility.Collapsed;

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
                var btnCreerCommande = FindName("BtnCreerCommande") as Button;
                var btnVoirCommandes = FindName("BtnVoirCommandes") as Button;
                var btnCreerPlat = FindName("BtnCreerPlat") as Button;
                var btnClients = FindName("BtnClients") as Button;

                if (btnCreerCommande != null) btnCreerCommande.Tag = null;
                if (btnVoirCommandes != null) btnVoirCommandes.Tag = null;
                if (btnCreerPlat != null) btnCreerPlat.Tag = null;
                if (btnClients != null) btnClients.Tag = null;

                if (selectedButton != null)
                    selectedButton.Tag = "Selected";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur mise à jour boutons: {ex.Message}");
            }
        }

        private void ButDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            EmployeConnecte = null;
            Window_Loaded();
        }
    }
}