using SIBILIATP11.Classe;
using SIBILIATP11.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SIBILIATP11.UserControl
{
    public partial class VoirCommandes : System.Windows.Controls.UserControl
    {
        private GestionCommande LaGestionCommande { get; set; }
        private ObservableCollection<Contient> platsCommandeSelectionnee;
        private bool filtreCommandesDuJourActif = false;

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
            Commande uneCommande = obj as Commande;
            if (uneCommande == null) return false;
            if (filtreCommandesDuJourActif)
            {
                DateTime aujourdhui = DateTime.Today;
                bool estCommandeDuJour = uneCommande.DateRetraitPrevue.Date == aujourdhui && !uneCommande.Retiree;

                if (!estCommandeDuJour) return false;
            }
            if (!String.IsNullOrEmpty(inputClient.Text))
            {
                if (uneCommande?.UnClient == null) return false;
                return (uneCommande.UnClient.NomClient.StartsWith(inputClient.Text, StringComparison.OrdinalIgnoreCase) ||
                    uneCommande.UnClient.PrenomClient.StartsWith(inputClient.Text, StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

        private void ChargerPlatsCommande()
        {
            platsCommandeSelectionnee.Clear();
            if (dgCommandes.SelectedItem != null && LaGestionCommande?.LesContients != null)
            {
                Commande commandeSelectionnee = (Commande)dgCommandes.SelectedItem;
                try
                {
                    List<Contient> platsCommande = LaGestionCommande.LesContients
                        .Where(c => c.UneCommande.NumCommande == commandeSelectionnee.NumCommande)
                        .ToList();
                    foreach (Contient contient in platsCommande)
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
            if (filtreCommandesDuJourActif)
            {
                filtreCommandesDuJourActif = false;
                btnCommandesDuJour.Content = "📅 Commandes du jour";
                btnCommandesDuJour.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                btnCommandesDuJour.ToolTip = "Afficher les commandes à retirer aujourd'hui";
                txtFiltreActif.Visibility = Visibility.Collapsed;

                if (dgCommandes.ItemsSource != null)
                {
                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null) return;
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
                    RechargerDonnees();
                    RafraichirAffichage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Aucune sélection", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Commande commande = dgCommandes.SelectedItem as Commande;
            try
            {
                int nbPlatsCommande = 0;
                if (LaGestionCommande.LesContients != null)
                {
                    nbPlatsCommande = LaGestionCommande.LesContients
                        .Count(c => c.UneCommande.NumCommande == commande.NumCommande);
                }
                string messageConfirmation = $"Supprimer définitivement la commande #{commande.NumCommande} ?\n\n" +
                                            $"Client: {commande.UnClient?.NomClient} {commande.UnClient?.PrenomClient}\n" +
                                            $"Date: {commande.DateCommande:dd/MM/yyyy}\n" +
                                            $"Montant: {commande.PrixTotal:F2} €\n";
                if (nbPlatsCommande > 0)
                {
                    messageConfirmation += $"\n⚠️ {nbPlatsCommande} plat(s) seront également supprimés.\n";
                }
                messageConfirmation += "\nCette action est irréversible !";
                MessageBoxResult result = MessageBox.Show(messageConfirmation, "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    List<Contient> contentsASupprimer = LaGestionCommande.LesContients
                        .Where(c => c.UneCommande.NumCommande == commande.NumCommande)
                        .ToList();
                    foreach (Contient contient in contentsASupprimer)
                    {
                        try
                        {
                            contient.Delete();
                            LaGestionCommande.LesContients.Remove(contient);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    commande.Delete();
                    LaGestionCommande.LesCommandes.Remove(commande);
                    RechargerDonnees();
                    RafraichirAffichage();
                    ViderDetailsCommande();
                    MessageBox.Show($"La commande #{commande.NumCommande} a été supprimée avec succès.", "Suppression réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erreur lors de la suppression de la commande :\n{ex.Message}";
                MessageBox.Show(errorMessage, "Erreur de suppression", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RechargerDonnees()
        {
            try
            {
                LaGestionCommande.RechargerDonnees();
            }
            catch (Exception ex)
            {
            }
        }

        private void RafraichirAffichage()
        {
            try
            {
                dgCommandes.ItemsSource = null;
                dgCommandes.ItemsSource = LaGestionCommande.LesCommandes;
                dgCommandes.Items.Filter = RechercheMotClefCommande;
                if (dgCommandes.ItemsSource != null)
                {
                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViderDetailsCommande()
        {
            try
            {
                platsCommandeSelectionnee.Clear();
                dgPlatsCommande.ItemsSource = null;
                txtDetailCommande.Text = "Sélectionnez une commande pour voir les détails";
                dgCommandes.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public void RafraichirDepuisExterieur()
        {
            RechargerDonnees();
            filtreCommandesDuJourActif = false;
            btnCommandesDuJour.Content = "📅 Commandes du jour";
            btnCommandesDuJour.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            txtFiltreActif.Visibility = Visibility.Collapsed;
            inputClient.Text = string.Empty;

            RafraichirAffichage();
            ViderDetailsCommande();
        }

        private void btnCommandesDuJour_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filtreCommandesDuJourActif = !filtreCommandesDuJourActif;

                if (filtreCommandesDuJourActif)
                {
                    btnCommandesDuJour.Content = "📅 Toutes les commandes";
                    btnCommandesDuJour.Background = new SolidColorBrush(Color.FromRgb(255, 152, 0));
                    btnCommandesDuJour.ToolTip = "Afficher toutes les commandes";
                    DateTime aujourdhui = DateTime.Today;
                    txtFiltreActif.Text = $"📌 Filtre actif : Commandes à retirer le {aujourdhui:dd/MM/yyyy}";
                    txtFiltreActif.Visibility = Visibility.Visible;
                    inputClient.Text = string.Empty;
                }
                else
                {
                    btnCommandesDuJour.Content = "📅 Commandes du jour";
                    btnCommandesDuJour.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                    btnCommandesDuJour.ToolTip = "Afficher les commandes à retirer aujourd'hui";
                    txtFiltreActif.Visibility = Visibility.Collapsed;
                }
                if (dgCommandes.ItemsSource != null)
                {
                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
                }
                if (filtreCommandesDuJourActif)
                {
                    var commandesDuJour = LaGestionCommande.LesCommandes
                        .Where(c => c.DateRetraitPrevue.Date == DateTime.Today && !c.Retiree)
                        .ToList();

                    txtFiltreActif.Text += $" ({commandesDuJour.Count} commande(s) trouvée(s))";

                    if (commandesDuJour.Count == 0)
                    {
                        MessageBox.Show("Aucune commande à retirer aujourd'hui.", "Information",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'application du filtre : {ex.Message}", "Erreur",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}