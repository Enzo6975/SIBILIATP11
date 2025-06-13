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
        private Commande _commandeAModifier;
        private GestionCommande _gestionCommande;
        private Employe _employeConnecte;
        private ObservableCollection<Contient> _platsCommande;

        public bool ModificationReussie { get; private set; }

        public WindowModification(Commande commande, GestionCommande gestionCommande, Employe employeConnecte = null)
        {
            InitializeComponent();
            _commandeAModifier = commande;
            _gestionCommande = gestionCommande;
            _employeConnecte = employeConnecte ?? commande.UnEmploye;
            _platsCommande = new ObservableCollection<Contient>();
            ModificationReussie = false;

            InitialiserInterface();
            ChargerDonnees();
            ChargerPlatsCommande();
        }

        private void InitialiserInterface()
        {
            if (_gestionCommande?.LesClients != null)
            {
                cbClient.ItemsSource = _gestionCommande.LesClients;
            }

            dgPlatsModification.ItemsSource = _platsCommande;
        }

        private void ChargerDonnees()
        {
            if (_commandeAModifier != null)
            {
                txtNumCommande.Text = _commandeAModifier.NumCommande.ToString();
                dpDateCommande.SelectedDate = _commandeAModifier.DateCommande;
                dpDatePrevue.SelectedDate = _commandeAModifier.DateRetraitPrevue;
                txtPrixTotal.Text = _commandeAModifier.PrixTotal.ToString("F2");
                chkPayee.IsChecked = _commandeAModifier.Payee;
                chkRetiree.IsChecked = _commandeAModifier.Retiree;

                if (_employeConnecte != null)
                {
                    txtEmploye.Text = $"{_employeConnecte.NumEmploye} - {_employeConnecte.NomEmploye} {_employeConnecte.PrenomEmploye}";
                }

                if (_commandeAModifier.UnClient != null)
                {
                    var clientActuel = _gestionCommande.LesClients.FirstOrDefault(c => c.NumClient == _commandeAModifier.UnClient.NumClient);
                    if (clientActuel != null)
                    {
                        cbClient.SelectedItem = clientActuel;
                    }
                }
            }
        }

        private void ChargerPlatsCommande()
        {
            _platsCommande.Clear();

            try
            {
                // Debug : Vérifier les données disponibles
                System.Diagnostics.Debug.WriteLine($"Chargement plats pour commande {_commandeAModifier.NumCommande}");
                System.Diagnostics.Debug.WriteLine($"Nombre total de Contients : {_gestionCommande?.LesContients?.Count ?? 0}");

                if (_gestionCommande?.LesContients != null)
                {
                    // Filtrer les contenus pour cette commande
                    var platsCommande = _gestionCommande.LesContients
                        .Where(c => c.UneCommande != null && c.UneCommande.NumCommande == _commandeAModifier.NumCommande)
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"Plats trouvés pour cette commande : {platsCommande.Count}");

                    foreach (var contient in platsCommande)
                    {
                        // Vérifier que le plat est correctement lié
                        if (contient.UnPlat != null)
                        {
                            _platsCommande.Add(contient);
                            System.Diagnostics.Debug.WriteLine($"Ajouté : {contient.UnPlat.NomPlat} (Qty: {contient.Quantite})");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Contient sans plat trouvé !");
                        }
                    }
                }

                // Si aucun plat trouvé, essayer de recharger depuis la base
                if (_platsCommande.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Aucun plat trouvé, tentative de rechargement direct depuis la DB...");
                    ChargerPlatsDepuisDB();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des plats : {ex.Message}");
                MessageBox.Show($"Erreur lors du chargement des plats : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            RecalculerPrixTotal();
        }

        private void ChargerPlatsDepuisDB()
        {
            try
            {
                // Charger directement depuis la base de données
                var contients = new Contient().FindAll();
                var platsCommande = contients
                    .Where(c => c.UneCommande != null && c.UneCommande.NumCommande == _commandeAModifier.NumCommande)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"Plats trouvés directement en DB : {platsCommande.Count}");

                foreach (var contient in platsCommande)
                {
                    // S'assurer que les relations sont correctes
                    if (contient.UnPlat != null)
                    {
                        // Trouver le plat complet dans la gestion
                        var platComplet = _gestionCommande.LesPlats?.FirstOrDefault(p => p.NumPlat == contient.UnPlat.NumPlat);
                        if (platComplet != null)
                        {
                            contient.UnPlat = platComplet;
                        }
                        else
                        {
                            // Charger le plat depuis la DB si pas trouvé
                            contient.UnPlat.Read();
                        }

                        _platsCommande.Add(contient);
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

                foreach (var contient in _platsCommande)
                {
                    if (contient.UnPlat != null)
                    {
                        double prixUnitaire = contient.Prix > 0 ? contient.Prix : contient.UnPlat.PrixUnitaire;
                        total += prixUnitaire * contient.Quantite;
                    }
                }

                _commandeAModifier.PrixTotal = total;
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
            if (!dpDateCommande.SelectedDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une date de commande.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (dpDateCommande.SelectedDate.Value > DateTime.Now)
            {
                MessageBox.Show("La date de commande ne peut pas être dans le futur.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!dpDatePrevue.SelectedDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une date de retrait prévue.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (dpDateCommande.SelectedDate.HasValue &&
                dpDatePrevue.SelectedDate.Value < dpDateCommande.SelectedDate.Value)
            {
                MessageBox.Show("La date de retrait ne peut pas être antérieure à la date de commande.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cbClient.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_platsCommande.Count == 0)
            {
                MessageBox.Show("La commande doit contenir au moins un plat.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (_gestionCommande?.LesPlats != null && _gestionCommande.LesPlats.Count > 0)
                {
                    // Créer une fenêtre de sélection de plat ou utiliser le premier plat disponible
                    var platDisponible = _gestionCommande.LesPlats.FirstOrDefault(p =>
                        !_platsCommande.Any(pc => pc.UnPlat.NumPlat == p.NumPlat));

                    if (platDisponible != null)
                    {
                        var nouveauContient = new Contient
                        {
                            Quantite = 1,
                            Prix = platDisponible.PrixUnitaire,
                            UneCommande = _commandeAModifier,
                            UnPlat = platDisponible
                        };

                        _platsCommande.Add(nouveauContient);
                        RecalculerPrixTotal();

                        System.Diagnostics.Debug.WriteLine($"Plat ajouté : {platDisponible.NomPlat}");
                    }
                    else
                    {
                        MessageBox.Show("Tous les plats disponibles sont déjà dans la commande.", "Information",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Aucun plat disponible.", "Information",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout du plat : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSupprimerPlat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.DataContext is Contient contient)
                {
                    if (MessageBox.Show($"Supprimer le plat '{contient.UnPlat?.NomPlat}' de la commande ?",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _platsCommande.Remove(contient);
                        RecalculerPrixTotal();

                        System.Diagnostics.Debug.WriteLine($"Plat supprimé : {contient.UnPlat?.NomPlat}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
                if (!ValiderDonnees())
                    return;

                _commandeAModifier.DateCommande = dpDateCommande.SelectedDate ?? DateTime.Now;
                _commandeAModifier.DateRetraitPrevue = dpDatePrevue.SelectedDate ?? DateTime.Now;
                _commandeAModifier.Payee = chkPayee.IsChecked ?? false;
                _commandeAModifier.Retiree = chkRetiree.IsChecked ?? false;

                _commandeAModifier.UnEmploye = _employeConnecte;

                if (cbClient.SelectedItem is Client clientSelectionne)
                {
                    _commandeAModifier.UnClient = clientSelectionne;
                }

                RecalculerPrixTotal();

                // Mettre à jour la commande
                _commandeAModifier.Update();

                // Supprimer les anciens contenus
                var anciensContients = _gestionCommande.LesContients
                    .Where(c => c.UneCommande.NumCommande == _commandeAModifier.NumCommande)
                    .ToList();

                foreach (var ancien in anciensContients)
                {
                    ancien.Delete();
                    _gestionCommande.LesContients.Remove(ancien);
                }

                // Ajouter les nouveaux contenus
                foreach (var contient in _platsCommande)
                {
                    contient.UneCommande = _commandeAModifier;
                    contient.Create();
                    _gestionCommande.LesContients.Add(contient);
                }

                ModificationReussie = true;
                MessageBox.Show("Commande modifiée avec succès !", "Succès",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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