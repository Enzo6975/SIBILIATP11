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
                dpDateCommande.SelectedDate = commandeAModifier.DateCommande;
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
                    var clientActuel = gestionCommande.LesClients.FirstOrDefault(c => c.NumClient == commandeAModifier.UnClient.NumClient);
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

            if (gestionCommande?.LesContients != null)
            {
                var platsCommande = gestionCommande.LesContients
                    .Where(c => c.UneCommande.NumCommande == commandeAModifier.NumCommande)
                    .ToList();

                foreach (var contient in platsCommande)
                {
                    this.platsCommande.Add(contient);
                }
            }

            RecalculerPrixTotal();
        }

        private void RecalculerPrixTotal()
        {
            double total = platsCommande.Sum(p => p.CalculerTotal);
            commandeAModifier.PrixTotal = total;
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

            if (platsCommande.Count == 0)
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
            if (gestionCommande?.LesPlats != null && gestionCommande.LesPlats.Count > 0)
            {
                var platDisponible = gestionCommande.LesPlats.FirstOrDefault(p =>
                    !platsCommande.Any(pc => pc.UnPlat.NumPlat == p.NumPlat));

                if (platDisponible != null)
                {
                    var nouveauContient = new Contient(1, platDisponible.PrixUnitaire, commandeAModifier, platDisponible);
                    platsCommande.Add(nouveauContient);
                    RecalculerPrixTotal();
                }
            }
        }

        private void btnSupprimerPlat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Contient contient)
            {
                platsCommande.Remove(contient);
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

                commandeAModifier.DateCommande = dpDateCommande.SelectedDate ?? DateTime.Now;
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

                var anciensContients = gestionCommande.LesContients
                    .Where(c => c.UneCommande.NumCommande == commandeAModifier.NumCommande)
                    .ToList();

                foreach (var ancien in anciensContients)
                {
                    ancien.Delete();
                    gestionCommande.LesContients.Remove(ancien);
                }

                foreach (var contient in platsCommande)
                {
                    contient.UneCommande = commandeAModifier;
                    contient.Create();
                    gestionCommande.LesContients.Add(contient);
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