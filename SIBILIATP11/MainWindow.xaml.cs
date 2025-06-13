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
                MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
                Button btnVoirCommandes = FindName("BtnVoirCommandes") as Button;
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
                TextBlock txtBlockConnexion = FindName("TxtBlockConnexion") as TextBlock;
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
            switch (numRole) { 
                case 1: return "Responsable des Ventes"; 
                case 2: return "Vendeur"; 
                default: return ""; };
        }

        private void GererVisibiliteElements()
        {
            try
            {
                Button btnCreerPlat = FindName("BtnCreerPlat") as Button;
                UserControl.CreerPlat creerPlat = FindName("CreerPlat") as UserControl.CreerPlat;
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
                            Button btnVoirCommandes = FindName("BtnVoirCommandes") as Button;
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
                        UserControl.CreerCommande creerCommande = FindName("CreerCommande") as UserControl.CreerCommande;
                        if (creerCommande != null) RefreshCreerCommande(creerCommande);
                        break;
                    case "VoirCommandes":
                        UserControl.VoirCommandes voirCommandes = FindName("VoirCommandes") as UserControl.VoirCommandes;
                        if (voirCommandes != null) RefreshVoirCommandes(voirCommandes);
                        break;
                    case "CreerPlat":
                        UserControl.CreerPlat creerPlat = FindName("CreerPlat") as UserControl.CreerPlat;
                        if (creerPlat != null) RefreshCreerPlat(creerPlat);
                        break;
                    case "VoirClient":
                        UserControl.VoirClient voirClient = FindName("VoirClient") as UserControl.VoirClient;
                        if (voirClient != null) RefreshVoirClient(voirClient);
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
                LaGestion.RechargerDonnees();
                this.DataContext = LaGestion;
                DataGrid platsDataGrid = control.FindName("plats") as DataGrid;
                if (platsDataGrid != null)
                {
                    platsDataGrid.ItemsSource = null;
                    platsDataGrid.ItemsSource = LaGestion.LesPlats;
                    CollectionViewSource.GetDefaultView(platsDataGrid.ItemsSource).Refresh();
                }
                ComboBox cbCategorie = control.FindName("cbCategorie") as ComboBox;
                if (cbCategorie != null)
                {
                    List<object> categoriesAvecTous = new List<object>();
                    categoriesAvecTous.Add(new { NumCategorie = -1, NomCategorie = "Toutes les catégories" });
                    if (LaGestion.LesCategories != null)
                    {
                        foreach (Categorie categorie in LaGestion.LesCategories)
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
                control.RafraichirDepuisExterieur();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RefreshVoirCommandes: {ex.Message}");
                try
                {
                    LaGestion.RechargerDonnees();
                    this.DataContext = LaGestion;
                    DataGrid dgCommandes = control.FindName("dgCommandes") as DataGrid;
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
                LaGestion.RechargerDonnees();
                this.DataContext = LaGestion;
                control.RafraichirDonnees();
                System.Diagnostics.Debug.WriteLine("RefreshCreerPlat terminé");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RefreshCreerPlat: {ex.Message}");
                try
                {
                    ComboBox cbSousCategorie = control.FindName("cbSousCategorie") as ComboBox;
                    if (cbSousCategorie != null) cbSousCategorie.ItemsSource = LaGestion.LesSousCategories;
                    ComboBox cbPeriode = control.FindName("cbPeriode") as ComboBox;
                    if (cbPeriode != null) cbPeriode.ItemsSource = LaGestion.LesPeriodes;
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
                if (control != null)
                {
                    MethodInfo method = control.GetType().GetMethod("RafraichirDonnees");
                    if (method != null)
                    {
                        method.Invoke(control, null);
                    }
                    else
                    {
                        DataGrid clientsDataGrid = control.FindName("clients") as DataGrid;
                        if (clientsDataGrid != null)
                        {
                            List<Client> nouveauxClients = new Client().FindAll();
                            clientsDataGrid.ItemsSource = nouveauxClients;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RefreshVoirClient: {ex.Message}");
                try
                {
                    DataGrid clientsDataGrid = control?.FindName("clients") as DataGrid;
                    if (clientsDataGrid != null)
                    {
                        List<Client> nouveauxClients = new Client().FindAll();
                        clientsDataGrid.ItemsSource = nouveauxClients;
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Erreur fallback RefreshVoirClient: {ex2.Message}");
                }
            }
        }

        private void ShowUserControl(string controlName)
        {
            try
            {
                FrameworkElement creerCommande = FindName("CreerCommande") as FrameworkElement;
                FrameworkElement voirCommandes = FindName("VoirCommandes") as FrameworkElement;
                FrameworkElement creerPlat = FindName("CreerPlat") as FrameworkElement;
                FrameworkElement voirClient = FindName("VoirClient") as FrameworkElement;
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
                Button btnCreerCommande = FindName("BtnCreerCommande") as Button;
                Button btnVoirCommandes = FindName("BtnVoirCommandes") as Button;
                Button btnCreerPlat = FindName("BtnCreerPlat") as Button;
                Button btnClients = FindName("BtnClients") as Button;
                if (btnCreerCommande != null) btnCreerCommande.Tag = null;
                if (btnVoirCommandes != null) btnVoirCommandes.Tag = null;
                if (btnCreerPlat != null) btnCreerPlat.Tag = null;
                if (btnClients != null) btnClients.Tag = null;
                if (selectedButton != null) selectedButton.Tag = "Selected";
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