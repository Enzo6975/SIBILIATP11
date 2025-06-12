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

            if (_gestionCommande?.LesContients != null)
            {
                var platsCommande = _gestionCommande.LesContients
                    .Where(c => c.UneCommande.NumCommande == _commandeAModifier.NumCommande)
                    .ToList();

                foreach (var contient in platsCommande)
                {
                    _platsCommande.Add(contient);
                }
            }

            RecalculerPrixTotal();
        }

        private void RecalculerPrixTotal()
        {
            double total = _platsCommande.Sum(p => p.CalculerTotal);
            _commandeAModifier.PrixTotal = total;
            txtPrixTotal.Text = total.ToString("F2");

            if (dgPlatsModification != null)
            {
                dgPlatsModification.Items.Refresh();
            }
        }

        private bool ValiderDonnees()
        {
            if (!dpDateCommande.SelectedDate.HasValue)
            {
                return false;
            }
            else if (dpDateCommande.SelectedDate.Value > DateTime.Now)
            {
                return false;
            }

            if (!dpDatePrevue.SelectedDate.HasValue)
            {
                return false;
            }
            else if (dpDateCommande.SelectedDate.HasValue &&
                dpDatePrevue.SelectedDate.Value < dpDateCommande.SelectedDate.Value)
            {
                return false;
            }

            if (cbClient.SelectedItem == null)
            {
                return false;
            }

            if (_platsCommande.Count == 0)
            {
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
            if (_gestionCommande?.LesPlats != null && _gestionCommande.LesPlats.Count > 0)
            {
                var platDisponible = _gestionCommande.LesPlats.FirstOrDefault(p =>
                    !_platsCommande.Any(pc => pc.UnPlat.NumPlat == p.NumPlat));

                if (platDisponible != null)
                {
                    var nouveauContient = new Contient(1, platDisponible.PrixUnitaire, _commandeAModifier, platDisponible);
                    _platsCommande.Add(nouveauContient);
                    RecalculerPrixTotal();
                }
            }
        }

        private void btnSupprimerPlat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Contient contient)
            {
                _platsCommande.Remove(contient);
                RecalculerPrixTotal();
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

                _commandeAModifier.Update();

                var anciensContients = _gestionCommande.LesContients
                    .Where(c => c.UneCommande.NumCommande == _commandeAModifier.NumCommande)
                    .ToList();

                foreach (var ancien in anciensContients)
                {
                    ancien.Delete();
                    _gestionCommande.LesContients.Remove(ancien);
                }

                foreach (var contient in _platsCommande)
                {
                    contient.UneCommande = _commandeAModifier;
                    contient.Create();
                    _gestionCommande.LesContients.Add(contient);
                }

                ModificationReussie = true;
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}