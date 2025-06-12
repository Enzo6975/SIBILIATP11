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

            // Récupérer la gestion depuis le DataContext de la MainWindow (similaire à l'exemple)
            LaGestionCommande = ((GestionCommande)App.Current.MainWindow.DataContext);

            // Initialiser les collections
            PlatsCommandeSelectionnee = new ObservableCollection<Contient>();

            // Configurer le DataGrid avec les commandes de la gestion
            dgCommandes.ItemsSource = LaGestionCommande.LesCommandes;

            // Configurer le filtre
            dgCommandes.Items.Filter = RechercheMotClefCommande;
        }

        // Propriété pour les plats de la commande sélectionnée
        public ObservableCollection<Contient> PlatsCommandeSelectionnee
        {
            get { return platsCommandeSelectionnee; }
            set { platsCommandeSelectionnee = value; }
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
            CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
        }

        // Méthode pour charger les plats de la commande sélectionnée
        private void ChargerPlatsCommande()
        {
            PlatsCommandeSelectionnee.Clear();

            if (dgCommandes.SelectedItem != null)
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
                        PlatsCommandeSelectionnee.Add(contient);
                    }

                    // Mettre à jour le DataGrid des plats si vous en avez un
                    if (dgPlatsCommande != null)
                        dgPlatsCommande.ItemsSource = PlatsCommandeSelectionnee;
                }
                catch (Exception ex)
                {
                    Window parent = Window.GetWindow(this);
                    MessageBox.Show(parent, $"Erreur lors du chargement des plats: {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Event handler pour la sélection d'une commande
        private void dgCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChargerPlatsCommande();
        }

        // Bouton Modifier (similaire à btnEdit_Click)
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);

            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show(parent, "Veuillez sélectionner une commande", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
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
                MessageBoxResult result = MessageBox.Show(parent,
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

                        MessageBox.Show(parent, "Commande modifiée avec succès", "Succès",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(parent, "La commande n'a pas pu être modifiée.", "Attention",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Bouton Supprimer (similaire à btnDel_Click)
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);

            if (dgCommandes.SelectedItem == null)
            {
                MessageBox.Show(parent, "Veuillez sélectionner une commande", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Commande commande = dgCommandes.SelectedItem as Commande;

            // Vérifier s'il y a des plats dans cette commande
            int nbPlatsCommande = LaGestionCommande.LesContients
                .Count(c => c.UneCommande.NumCommande == commande.NumCommande);

            if (nbPlatsCommande > 0)
            {
                MessageBoxResult result = MessageBox.Show(parent,
                    "Attention, des plats seront supprimés aussi.", "Attention",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                    return;
            }

            try
            {
                commande.Delete();
                LaGestionCommande.LesCommandes.Remove(commande);

                MessageBox.Show(parent, "Commande supprimée avec succès", "Succès",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(parent, "La commande n'a pas pu être supprimée.", "Attention",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Bouton Ajouter (similaire à btnAdd_Click)
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Commande uneCommande = new Commande();

            // TODO: Créer WindowCommande
            // WindowCommande wCommande = new WindowCommande(uneCommande, Action.Ajouter);
            // bool? result = wCommande.ShowDialog();

            Window parent = Window.GetWindow(this);

            // Pour l'instant, simuler
            MessageBoxResult result = MessageBox.Show(parent,
                "Ajouter une nouvelle commande ?", "Nouvelle commande",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

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
                    // uneCommande.UnEmploye = ...; // À définir
                    // uneCommande.UnClient = ...; // À définir

                    uneCommande.NumCommande = uneCommande.Create();
                    LaGestionCommande.LesCommandes.Add(uneCommande);

                    MessageBox.Show(parent, "Commande créée avec succès", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(parent, "La commande n'a pas pu être créée.", "Attention",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Méthode appelée quand la case à cocher "Commande retirée" est modifiée
        private void OnCommandeRetireeChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Commande commande)
            {
                try
                {
                    commande.Update();

                    string status = commande.Retiree ? "retirée" : "en attente";
                    Window parent = Window.GetWindow(this);
                    MessageBox.Show(parent, $"Commande #{commande.NumCommande} marquée comme {status}",
                        "Mise à jour", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    Window parent = Window.GetWindow(this);
                    MessageBox.Show(parent, $"Erreur lors de la mise à jour: {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Annuler le changement en cas d'erreur
                    commande.Retiree = !commande.Retiree;
                }
            }
        }

        // Méthode pour effacer le filtre
        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            inputClient.Text = string.Empty;
        }
    }
}