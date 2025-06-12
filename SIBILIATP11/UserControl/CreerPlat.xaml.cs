using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
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
    public partial class CreerPlat : System.Windows.Controls.UserControl
    {
        private GestionCommande LaGestionCommande { get; set; }

        public CreerPlat()
        {
            InitializeComponent();
            InitialiserGestionCommande();
            ConfigurerComboBoxes();
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

        private void ConfigurerComboBoxes()
        {
            if (LaGestionCommande != null)
            {
                cbSousCategorie.ItemsSource = LaGestionCommande.LesSousCategories;
                cbSousCategorie.DisplayMemberPath = "NomSousCategorie";
                cbSousCategorie.SelectedValuePath = "NumSousCategorie";

                cbPeriode.ItemsSource = LaGestionCommande.LesPeriodes;
                cbPeriode.DisplayMemberPath = "LibellePeriode";
                cbPeriode.SelectedValuePath = "NumPeriode";
            }
        }

        private bool ValiderSaisie()
        {
            if (string.IsNullOrWhiteSpace(txtNomPlat.Text))
            {
                MessageBox.Show("Le nom du plat est obligatoire", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(txtPrixUnitaire.Text, out double prix) || prix <= 0)
            {
                MessageBox.Show("Le prix unitaire doit être un nombre positif", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtDelaiPreparation.Text, out int delai) || delai <= 0)
            {
                MessageBox.Show("Le délai de préparation doit être un nombre positif", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(txtNbPersonnes.Text, out int personnes) || personnes <= 0)
            {
                MessageBox.Show("Le nombre de personnes doit être un nombre positif", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cbSousCategorie.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une sous-catégorie", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cbPeriode.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une période", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ViderChamps()
        {
            txtNomPlat.Text = "";
            txtPrixUnitaire.Text = "";
            txtDelaiPreparation.Text = "";
            txtNbPersonnes.Text = "";
            cbSousCategorie.SelectedIndex = -1;
            cbPeriode.SelectedIndex = -1;
        }

        private void btnCreerPlat_Click(object sender, RoutedEventArgs e)
        {
            if (ValiderSaisie())
            {
                try
                {
                    Plat nouveauPlat = new Plat();
                    nouveauPlat.NomPlat = txtNomPlat.Text;
                    nouveauPlat.PrixUnitaire = double.Parse(txtPrixUnitaire.Text);
                    nouveauPlat.DelaiPreparation = int.Parse(txtDelaiPreparation.Text);
                    nouveauPlat.NbPersonnes = int.Parse(txtNbPersonnes.Text);

                    SousCategorie sousCategorie = (SousCategorie)cbSousCategorie.SelectedItem;
                    Periode periode = (Periode)cbPeriode.SelectedItem;

                    nouveauPlat.UneSousCategorie = sousCategorie;
                    nouveauPlat.UnePeriode = periode;

                    nouveauPlat.NumPlat = nouveauPlat.Create();
                    LaGestionCommande.LesPlats.Add(nouveauPlat);

                    MessageBox.Show("Plat créé avec succès !", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    ViderChamps();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la création du plat : {ex.Message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ViderChamps();
        }
    }
}