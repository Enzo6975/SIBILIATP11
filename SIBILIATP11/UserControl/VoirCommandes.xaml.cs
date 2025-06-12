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

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            inputClient.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Commande uneCommande = new Commande();

            MessageBoxResult result = MessageBox.Show("Ajouter une nouvelle commande ?",
                "Nouvelle commande", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    uneCommande.DateCommande = DateTime.Now;
                    uneCommande.DateRetraitPrevue = DateTime.Now.AddDays(1);
                    uneCommande.Payee = false;
                    uneCommande.Retiree = false;
                    uneCommande.PrixTotal = 0;

                    if (LaGestionCommande.LesEmploye?.Count > 0)
                        uneCommande.UnEmploye = LaGestionCommande.LesEmploye[0];
                    if (LaGestionCommande.LesClients?.Count > 0)
                        uneCommande.UnClient = LaGestionCommande.LesClients[0];

                    uneCommande.NumCommande = uneCommande.Create();
                    LaGestionCommande.LesCommandes.Add(uneCommande);

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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Commande commandeSelectionnee = (Commande)dgCommandes.SelectedItem;

            MessageBoxResult result = MessageBox.Show(
                $"Modifier la commande #{commandeSelectionnee.NumCommande} ?\n" +
                $"Client: {commandeSelectionnee.UnClient?.NomClient} {commandeSelectionnee.UnClient?.PrenomClient}\n" +
                $"Date: {commandeSelectionnee.DateCommande:dd/MM/yyyy}",
                "Modifier commande", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    commandeSelectionnee.Update();

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

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Commande commande = dgCommandes.SelectedItem as Commande;

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

        private void OnCommandeRetireeChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Commande commande)
            {
                try
                {
                    commande.Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour: {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    commande.Retiree = !commande.Retiree;
                }
            }
        }
    }
}