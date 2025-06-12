using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIBILIATP11.UserControl
{
    public partial class VoirCommandes : System.Windows.Controls.UserControl
    {
        private GestionCommande LaGestionCommande { get; set; }
        private ObservableCollection<Contient> platsCommandeSelectionnee;

        public VoirCommandes()
        {
            InitializeComponent();

            // Initialiser les collections
            platsCommandeSelectionnee = new ObservableCollection<Contient>();

            // Initialiser GestionCommande de manière sécurisée
            InitialiserGestionCommande();

            // Différer la configuration après le chargement complet
            this.Loaded += VoirCommandes_Loaded;
        }

        private void VoirCommandes_Loaded(object sender, RoutedEventArgs e)
        {
            // Se désabonner pour éviter les appels multiples
            this.Loaded -= VoirCommandes_Loaded;

            // Configurer le DataGrid avec les commandes
            if (LaGestionCommande != null)
            {
                dgCommandes.ItemsSource = LaGestionCommande.LesCommandes;

                // Configurer le filtre pour le DataGrid
                dgCommandes.Items.Filter = RechercheMotClefCommande;

                // Debug: afficher le nombre de commandes chargées
                System.Diagnostics.Debug.WriteLine($"Nombre de commandes chargées: {LaGestionCommande.LesCommandes?.Count ?? 0}");
            }
        }

        private void InitialiserGestionCommande()
        {
            try
            {
                // Méthode 1 : Essayer de récupérer depuis MainWindow via DataContext
                if (App.Current.MainWindow?.DataContext is GestionCommande gestionDC)
                {
                    LaGestionCommande = gestionDC;
                }
                // Méthode 2 : Essayer de récupérer depuis MainWindow via la propriété LaGestion
                else if (App.Current.MainWindow is MainWindow mainWin && mainWin.LaGestion != null)
                {
                    LaGestionCommande = mainWin.LaGestion;
                }
                else
                {
                    // Méthode 3 : Créer une nouvelle instance si pas trouvée
                    MessageBox.Show("Impossible de récupérer les données depuis MainWindow.\nCréation d'une nouvelle instance.",
                        "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LaGestionCommande = new GestionCommande("Gestion Commandes");
                }
            }
            catch (Exception ex)
            {
                // Méthode 4 : En cas d'erreur, créer une instance par défaut
                MessageBox.Show($"Erreur lors de l'initialisation: {ex.Message}\nCréation d'une nouvelle instance.",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                LaGestionCommande = new GestionCommande("Gestion Commandes");
            }
        }

        // Méthode de filtrage (similaire à RechercheMotClefChien)
        private bool RechercheMotClefCommande(object obj)
        {
            if (String.IsNullOrEmpty(inputClient.Text))
                return true;

            Commande uneCommande = obj as Commande;

            if (uneCommande?.UnClient == null)
                return false;

            return (uneCommande.UnClient.NomClient.StartsWith(inputClient.Text, StringComparison.OrdinalIgnoreCase)
                || uneCommande.UnClient.PrenomClient.StartsWith(inputClient.Text, StringComparison.OrdinalIgnoreCase));
        }

        // Event handler pour le changement de texte dans le filtre
        private void inputClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Vérifier que le DataGrid et son ItemsSource sont configurés
            if (dgCommandes.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
            }
        }

        // Event handler pour la sélection d'une commande
        private void dgCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChargerPlatsCommande();
        }

        // Méthode pour charger les plats de la commande sélectionnée
        private void ChargerPlatsCommande()
        {
            platsCommandeSelectionnee.Clear();

            if (dgCommandes.SelectedItem != null && LaGestionCommande?.LesContients != null)
            {
                Commande commandeSelectionnee = (Commande)dgCommandes.SelectedItem;

                try
                {
                    // Utiliser la collection déjà chargée dans GestionCommande
                    var platsCommande = LaGestionCommande.LesContients
                        .Where(c => c.UneCommande.NumCommande == commandeSelectionnee.NumCommande)
                        .ToList();

                    foreach (var contient in platsCommande)
                    {
                        platsCommandeSelectionnee.Add(contient);
                    }

                    // Mettre à jour le DataGrid des plats
                    dgPlatsCommande.ItemsSource = platsCommandeSelectionnee;

                    // Mettre à jour le texte de détail
                    txtDetailCommande.Text = $"Détails de la commande #{commandeSelectionnee.NumCommande}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement des plats: {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                dgPlatsCommande.ItemsSource = null;
                txtDetailCommande.Text = "Sélectionnez une commande pour voir les détails";
            }
        }

        // Bouton pour effacer le filtre
        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            inputClient.Text = string.Empty;
        }

        // Bouton Ajouter
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Commande uneCommande = new Commande();

            // TODO: Créer WindowCommande
            // WindowCommande wCommande = new WindowCommande(uneCommande, Action.Ajouter);
            // bool? result = wCommande.ShowDialog();

            MessageBoxResult result = MessageBox.Show("Ajouter une nouvelle commande ?",
                "Nouvelle commande", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // TODO: Configurer les propriétés de la commande via la fenêtre d'édition
                    uneCommande.DateCommande = DateTime.Now;
                    uneCommande.DateRetraitPrevue = DateTime.Now.AddDays(1);
                    uneCommande.Payee = false;
                    uneCommande.Retiree = false;
                    uneCommande.PrixTotal = 0;

                    // Pour l'instant, utiliser des valeurs par défaut
                    if (LaGestionCommande.LesEmploye?.Count > 0)
                        uneCommande.UnEmploye = LaGestionCommande.LesEmploye[0];
                    if (LaGestionCommande.LesClients?.Count > 0)
                        uneCommande.UnClient = LaGestionCommande.LesClients[0];

                    uneCommande.NumCommande = uneCommande.Create();
                    LaGestionCommande.LesCommandes.Add(uneCommande);

                    // Rafraîchir l'affichage
                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();

                    MessageBox.Show("Commande créée avec succès", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"La commande n'a pas pu être créée: {ex.Message}", "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Bouton Modifier
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Commande commandeSelectionnee = (Commande)dgCommandes.SelectedItem;

            // Créer une copie pour l'édition
            Commande copie = new Commande(commandeSelectionnee.NumCommande,
                commandeSelectionnee.DateCommande,
                commandeSelectionnee.DateRetraitPrevue,
                commandeSelectionnee.Payee,
                commandeSelectionnee.Retiree,
                commandeSelectionnee.PrixTotal,
                commandeSelectionnee.UnEmploye,
                commandeSelectionnee.UnClient);

            // TODO: Créer WindowCommande similaire à WindowChien
            // WindowCommande wCommande = new WindowCommande(copie, Action.Modifier);
            // bool? result = wCommande.ShowDialog();

            // Pour l'instant, simuler avec une MessageBox
            MessageBoxResult result = MessageBox.Show(
                $"Modifier la commande #{commandeSelectionnee.NumCommande} ?\n" +
                $"Client: {commandeSelectionnee.UnClient?.NomClient} {commandeSelectionnee.UnClient?.PrenomClient}\n" +
                $"Date: {commandeSelectionnee.DateCommande:dd/MM/yyyy}",
                "Modifier commande", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Appliquer les modifications (à adapter selon votre fenêtre d'édition)
                    // commandeSelectionnee.DateRetraitPrevue = copie.DateRetraitPrevue;
                    // commandeSelectionnee.Payee = copie.Payee;
                    // commandeSelectionnee.Retiree = copie.Retiree;
                    commandeSelectionnee.Update();

                    // Rafraîchir l'affichage
                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();

                    MessageBox.Show("Commande modifiée avec succès", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"La commande n'a pas pu être modifiée: {ex.Message}", "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Bouton Supprimer
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Commande commande = dgCommandes.SelectedItem as Commande;

            // Vérifier s'il y a des plats dans cette commande
            int nbPlatsCommande = 0;
            if (LaGestionCommande.LesContients != null)
            {
                nbPlatsCommande = LaGestionCommande.LesContients
                    .Count(c => c.UneCommande.NumCommande == commande.NumCommande);
            }

            if (nbPlatsCommande > 0)
            {
                MessageBoxResult result = MessageBox.Show("Attention, des plats seront supprimés aussi.",
                    "Attention", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                    return;
            }

            try
            {
                commande.Delete();
                LaGestionCommande.LesCommandes.Remove(commande);

                // Rafraîchir l'affichage
                CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();

                MessageBox.Show("Commande supprimée avec succès", "Succès",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"La commande n'a pas pu être supprimée: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler pour la case à cocher "Commande retirée"
        private void OnCommandeRetireeChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Commande commande)
            {
                try
                {
                    commande.Update();
                    // MessageBox supprimé - la mise à jour se fait silencieusement
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour: {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Annuler le changement en cas d'erreur
                    commande.Retiree = !commande.Retiree;
                }
            }
        }
    }
}