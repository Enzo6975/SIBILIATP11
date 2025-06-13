using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIBILIATP11.Windows
{
    public partial class WindowModification : Window
    {
        private Commande commandeAModifier;
        private GestionCommande gestionCommande;
        private Employe employeConnecte;
        private ObservableCollection<Contient> platsCommande;

        public bool ModificationReussie { get; private set; }

        public WindowModification(Commande commande, GestionCommande gestionCommande, Employe employeConnecte = null)
        {
            InitializeComponent();
            commandeAModifier = commande;
            this.gestionCommande = gestionCommande;
            this.employeConnecte = employeConnecte ?? commande.UnEmploye;
            platsCommande = new ObservableCollection<Contient>();
            ModificationReussie = false;
            InitialiserInterface();
            ChargerDonnees();
            ChargerPlatsCommande();
        }

        private void InitialiserInterface()
        {
            if (gestionCommande?.LesClients != null)
            {
                cbClient.ItemsSource = gestionCommande.LesClients;
            }
            dgPlatsModification.ItemsSource = platsCommande;
        }

        private void ChargerDonnees()
        {
            if (commandeAModifier != null)
            {
                txtNumCommande.Text = commandeAModifier.NumCommande.ToString();
                txtDateCommande.Text = commandeAModifier.DateCommande.ToString("dd/MM/yyyy");
                dpDatePrevue.SelectedDate = commandeAModifier.DateRetraitPrevue;
                txtPrixTotal.Text = commandeAModifier.PrixTotal.ToString("F2");
                chkPayee.IsChecked = commandeAModifier.Payee;
                chkRetiree.IsChecked = commandeAModifier.Retiree;
                if (employeConnecte != null)
                {
                    txtEmploye.Text = $"{employeConnecte.NumEmploye} - {employeConnecte.NomEmploye} {employeConnecte.PrenomEmploye}";
                }
                if (commandeAModifier.UnClient != null)
                {
                    Client clientActuel = gestionCommande.LesClients.FirstOrDefault(c => c.NumClient == commandeAModifier.UnClient.NumClient);
                    if (clientActuel != null)
                    {
                        cbClient.SelectedItem = clientActuel;
                    }
                }
            }
        }

        private void ChargerPlatsCommande()
        {
            platsCommande.Clear();
            try
            {
                System.Diagnostics.Debug.WriteLine($"Chargement plats pour commande {commandeAModifier.NumCommande}");
                System.Diagnostics.Debug.WriteLine($"Nombre total de Contients : {gestionCommande?.LesContients?.Count ?? 0}");
                if (gestionCommande?.LesContients != null)
                {
                    List<Contient> platsCommandeList = gestionCommande.LesContients
                        .Where(c => c.UneCommande != null && c.UneCommande.NumCommande == commandeAModifier.NumCommande)
                        .ToList();
                    System.Diagnostics.Debug.WriteLine($"Plats trouvés pour cette commande : {platsCommandeList.Count}");
                    foreach (Contient contient in platsCommandeList)
                    {
                        if (contient.UnPlat != null)
                        {
                            platsCommande.Add(contient);
                            System.Diagnostics.Debug.WriteLine($"Ajouté : {contient.UnPlat.NomPlat} (Qty: {contient.Quantite})");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Contient sans plat trouvé !");
                        }
                    }
                }
                if (platsCommande.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Aucun plat trouvé, tentative de rechargement direct depuis la DB...");
                    ChargerPlatsDepuisDB();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des plats : {ex.Message}");
                MessageBox.Show($"Erreur lors du chargement des plats : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            RecalculerPrixTotal();
        }

        private void ChargerPlatsDepuisDB()
        {
            try
            {
                List<Contient> contients = new Contient().FindAll();
                List<Contient> platsCommandeList = contients
                    .Where(c => c.UneCommande != null && c.UneCommande.NumCommande == commandeAModifier.NumCommande)
                    .ToList();
                System.Diagnostics.Debug.WriteLine($"Plats trouvés directement en DB : {platsCommandeList.Count}");
                foreach (Contient contient in platsCommandeList)
                {
                    if (contient.UnPlat != null)
                    {
                        Plat platComplet = gestionCommande.LesPlats?.FirstOrDefault(p => p.NumPlat == contient.UnPlat.NumPlat);
                        if (platComplet != null)
                        {
                            contient.UnPlat = platComplet;
                        }
                        else
                        {
                            contient.UnPlat.Read();
                        }
                        platsCommande.Add(contient);
                        System.Diagnostics.Debug.WriteLine($"Ajouté depuis DB : {contient.UnPlat.NomPlat} (Qty: {contient.Quantite})");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur chargement depuis DB : {ex.Message}");
            }
        }

        private void RecalculerPrixTotal()
        {
            try
            {
                double total = 0;
                foreach (Contient contient in platsCommande)
                {
                    if (contient.UnPlat != null)
                    {
                        double prixUnitaire = contient.Prix > 0 ? contient.Prix : contient.UnPlat.PrixUnitaire;
                        total += prixUnitaire * contient.Quantite;
                    }
                }
                commandeAModifier.PrixTotal = total;
                txtPrixTotal.Text = total.ToString("F2");
                System.Diagnostics.Debug.WriteLine($"Prix total recalculé : {total:F2}€");
                if (dgPlatsModification != null)
                {
                    dgPlatsModification.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur recalcul prix : {ex.Message}");
            }
        }

        private bool ValiderDonnees()
        {
            if (!dpDatePrevue.SelectedDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une date de retrait prévue.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (dpDatePrevue.SelectedDate.Value < commandeAModifier.DateCommande)
            {
                MessageBox.Show("La date de retrait ne peut pas être antérieure à la date de commande.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (cbClient.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (platsCommande.Count == 0)
            {
                MessageBox.Show("La commande doit contenir au moins un plat.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void dgPlatsModification_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() => {
                    RecalculerPrixTotal();
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        private void btnAjouterPlat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gestionCommande?.LesPlats != null && gestionCommande.LesPlats.Count > 0)
                {
                    Plat platDisponible = gestionCommande.LesPlats.FirstOrDefault(p => !platsCommande.Any(pc => pc.UnPlat.NumPlat == p.NumPlat));
                    if (platDisponible != null)
                    {
                        Contient nouveauContient = new Contient
                        {
                            Quantite = 1,
                            Prix = platDisponible.PrixUnitaire,
                            UneCommande = commandeAModifier,
                            UnPlat = platDisponible
                        };
                        platsCommande.Add(nouveauContient);
                        RecalculerPrixTotal();
                        System.Diagnostics.Debug.WriteLine($"Plat ajouté : {platDisponible.NomPlat}");
                    }
                    else
                    {
                        MessageBox.Show("Tous les plats disponibles sont déjà dans la commande.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Aucun plat disponible.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout du plat : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSupprimerPlat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.DataContext is Contient contient)
                {
                    if (MessageBox.Show($"Supprimer le plat '{contient.UnPlat?.NomPlat}' de la commande ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        platsCommande.Remove(contient);
                        RecalculerPrixTotal();
                        System.Diagnostics.Debug.WriteLine($"Plat supprimé : {contient.UnPlat?.NomPlat}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRecalculerTotal_Click(object sender, RoutedEventArgs e)
        {
            RecalculerPrixTotal();
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValiderDonnees()) return;

                // Ne pas modifier la date de commande - elle reste celle d'origine
                // commandeAModifier.DateCommande = dpDateCommande.SelectedDate ?? DateTime.Now; // Ligne à supprimer

                commandeAModifier.DateRetraitPrevue = dpDatePrevue.SelectedDate ?? DateTime.Now;
                commandeAModifier.Payee = chkPayee.IsChecked ?? false;
                commandeAModifier.Retiree = chkRetiree.IsChecked ?? false;
                commandeAModifier.UnEmploye = employeConnecte;
                if (cbClient.SelectedItem is Client clientSelectionne)
                {
                    commandeAModifier.UnClient = clientSelectionne;
                }
                RecalculerPrixTotal();
                commandeAModifier.Update();
                List<Contient> anciensContients = gestionCommande.LesContients
                    .Where(c => c.UneCommande.NumCommande == commandeAModifier.NumCommande)
                    .ToList();
                foreach (Contient ancien in anciensContients)
                {
                    ancien.Delete();
                    gestionCommande.LesContients.Remove(ancien);
                }
                foreach (Contient contient in platsCommande)
                {
                    contient.UneCommande = commandeAModifier;
                    contient.Create();
                    gestionCommande.LesContients.Add(contient);
                }
                ModificationReussie = true;
                MessageBox.Show("Commande modifiée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Erreur validation : {ex.Message}");
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}