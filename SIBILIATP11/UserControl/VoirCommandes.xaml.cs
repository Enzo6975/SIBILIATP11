using SIBILIATP11.Classe;
using SIBILIATP11.Windows;
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
            platsCommandeSelectionnee = new ObservableCollection<Contient>();
            InitialiserGestionCommande();

            if (LaGestionCommande != null)
            {
                dgCommandes.ItemsSource = LaGestionCommande.LesCommandes;
                dgCommandes.Items.Filter = RechercheMotClefCommande;
            }
        }

        private void InitialiserGestionCommande()
        {
            try
            {
                if (App.Current.MainWindow?.DataContext is GestionCommande gestionDC)
                {
                    LaGestionCommande = gestionDC;
                }
                else if (App.Current.MainWindow is MainWindow mainWin && mainWin.LaGestion != null)
                {
                    LaGestionCommande = mainWin.LaGestion;
                }
                else
                {
                    LaGestionCommande = new GestionCommande("Gestion Commandes");
                }
            }
            catch (Exception ex)
            {
                LaGestionCommande = new GestionCommande("Gestion Commandes");
            }
        }

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

        private void ChargerPlatsCommande()
        {
            platsCommandeSelectionnee.Clear();

            if (dgCommandes.SelectedItem != null && LaGestionCommande?.LesContients != null)
            {
                Commande commandeSelectionnee = (Commande)dgCommandes.SelectedItem;

                try
                {
                    var platsCommande = LaGestionCommande.LesContients
                        .Where(c => c.UneCommande.NumCommande == commandeSelectionnee.NumCommande)
                        .ToList();

                    foreach (var contient in platsCommande)
                    {
                        platsCommandeSelectionnee.Add(contient);
                    }

                    dgPlatsCommande.ItemsSource = platsCommandeSelectionnee;
                    txtDetailCommande.Text = $"Détails de la commande #{commandeSelectionnee.NumCommande}";
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                dgPlatsCommande.ItemsSource = null;
                txtDetailCommande.Text = "Sélectionnez une commande pour voir les détails";
            }
        }

        private void inputClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgCommandes.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
            }
        }

        private void dgCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChargerPlatsCommande();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            inputClient.Text = string.Empty;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
                return;

            Commande commandeSelectionnee = (Commande)dgCommandes.SelectedItem;

            try
            {
                Employe employeConnecte = null;
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    employeConnecte = mainWindow.EmployeConnecte;
                }

                WindowModification fenetreModification = new WindowModification(commandeSelectionnee, LaGestionCommande, employeConnecte);
                fenetreModification.Owner = Window.GetWindow(this);

                bool? resultat = fenetreModification.ShowDialog();

                if (resultat == true)
                {
                    // Recharger les données depuis la base
                    RechargerDonnees();

                    // Rafraîchir l'affichage
                    RafraichirAffichage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Aucune sélection",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Commande commande = dgCommandes.SelectedItem as Commande;

            try
            {
                // Compter les plats de la commande
                int nbPlatsCommande = 0;
                if (LaGestionCommande.LesContients != null)
                {
                    nbPlatsCommande = LaGestionCommande.LesContients
                        .Count(c => c.UneCommande.NumCommande == commande.NumCommande);
                }

                // Message de confirmation détaillé
                string messageConfirmation = $"Supprimer définitivement la commande #{commande.NumCommande} ?\n\n" +
                                           $"Client: {commande.UnClient?.NomClient} {commande.UnClient?.PrenomClient}\n" +
                                           $"Date: {commande.DateCommande:dd/MM/yyyy}\n" +
                                           $"Montant: {commande.PrixTotal:F2} €\n";

                if (nbPlatsCommande > 0)
                {
                    messageConfirmation += $"\n⚠️ {nbPlatsCommande} plat(s) seront également supprimés.\n";
                }

                messageConfirmation += "\nCette action est irréversible !";

                MessageBoxResult result = MessageBox.Show(messageConfirmation, "Confirmation de suppression",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Supprimer d'abord tous les contenus (plats) de la commande
                    var contentsASupprimer = LaGestionCommande.LesContients
                        .Where(c => c.UneCommande.NumCommande == commande.NumCommande)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"Suppression de {contentsASupprimer.Count} contenus pour la commande {commande.NumCommande}");

                    foreach (var contient in contentsASupprimer)
                    {
                        try
                        {
                            contient.Delete(); // Supprimer de la base de données
                            LaGestionCommande.LesContients.Remove(contient); // Supprimer de la collection
                            System.Diagnostics.Debug.WriteLine($"Contenu supprimé : {contient.UnPlat?.NomPlat}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Erreur suppression contenu : {ex.Message}");
                        }
                    }

                    // Ensuite supprimer la commande elle-même
                    System.Diagnostics.Debug.WriteLine($"Suppression de la commande {commande.NumCommande}");

                    commande.Delete(); // Supprimer de la base de données
                    LaGestionCommande.LesCommandes.Remove(commande); // Supprimer de la collection

                    System.Diagnostics.Debug.WriteLine("Suppression réussie, rafraîchissement de l'affichage");

                    // Recharger complètement les données pour être sûr
                    RechargerDonnees();

                    // Rafraîchir l'affichage
                    RafraichirAffichage();

                    // Vider les détails
                    ViderDetailsCommande();

                    // Message de succès
                    MessageBox.Show($"La commande #{commande.NumCommande} a été supprimée avec succès.", "Suppression réussie",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erreur lors de la suppression de la commande :\n{ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Erreur suppression commande : {ex.Message}");
                MessageBox.Show(errorMessage, "Erreur de suppression",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RechargerDonnees()
        {
            try
            {
                // Recharger toutes les données depuis la base
                LaGestionCommande.RechargerDonnees();

                System.Diagnostics.Debug.WriteLine($"Données rechargées : {LaGestionCommande.LesCommandes.Count} commandes");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rechargement données : {ex.Message}");
            }
        }

        private void RafraichirAffichage()
        {
            try
            {
                // Mettre à jour la source de données de la DataGrid
                dgCommandes.ItemsSource = null;
                dgCommandes.ItemsSource = LaGestionCommande.LesCommandes;

                // Réappliquer le filtre
                dgCommandes.Items.Filter = RechercheMotClefCommande;

                // Forcer le rafraîchissement
                if (dgCommandes.ItemsSource != null)
                {
                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
                }

                System.Diagnostics.Debug.WriteLine("Affichage rafraîchi");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rafraîchissement affichage : {ex.Message}");
            }
        }

        private void ViderDetailsCommande()
        {
            try
            {
                // Vider les détails de la commande
                platsCommandeSelectionnee.Clear();
                dgPlatsCommande.ItemsSource = null;
                txtDetailCommande.Text = "Sélectionnez une commande pour voir les détails";

                // Désélectionner dans la DataGrid
                dgCommandes.SelectedItem = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur vidage détails : {ex.Message}");
            }
        }

        // Méthode publique pour recharger depuis l'extérieur (appelée par MainWindow)
        public void RafraichirDepuisExterieur()
        {
            RechargerDonnees();
            RafraichirAffichage();
            ViderDetailsCommande();
        }
    }
}