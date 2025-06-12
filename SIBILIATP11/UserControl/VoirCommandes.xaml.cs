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
                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
                    ChargerPlatsCommande();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgCommandes.SelectedItem == null)
                return;

            Commande commande = dgCommandes.SelectedItem as Commande;

            int nbPlatsCommande = 0;
            if (LaGestionCommande.LesContients != null)
            {
                nbPlatsCommande = LaGestionCommande.LesContients
                    .Count(c => c.UneCommande.NumCommande == commande.NumCommande);
            }

            string messageConfirmation = $"Supprimer la commande #{commande.NumCommande} ?\n" +
                                       $"Client: {commande.UnClient?.NomClient} {commande.UnClient?.PrenomClient}\n" +
                                       $"Date: {commande.DateCommande:dd/MM/yyyy}\n" +
                                       $"Montant: {commande.PrixTotal:F2} €";

            if (nbPlatsCommande > 0)
            {
                messageConfirmation += $"\n({nbPlatsCommande} plat(s) seront supprimés)";
            }

            MessageBoxResult result = MessageBox.Show(messageConfirmation, "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    commande.Delete();
                    LaGestionCommande.LesCommandes.Remove(commande);

                    CollectionViewSource.GetDefaultView(dgCommandes.ItemsSource).Refresh();
                    ChargerPlatsCommande();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}