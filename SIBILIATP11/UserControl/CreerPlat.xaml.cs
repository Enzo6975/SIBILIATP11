using SIBILIATP11.Classe;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIBILIATP11.UserControl
{
    public partial class CreerPlat : System.Windows.Controls.UserControl
    {
        private GestionCommande LaGestionCommande { get; set; }

        public CreerPlat()
        {
            InitializeComponent();
            InitialiserGestionCommande();
            this.Loaded += CreerPlat_Loaded;
        }

        private void CreerPlat_Loaded(object sender, RoutedEventArgs e)
        {
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
            try
            {
                if (LaGestionCommande != null)
                {
                    if (LaGestionCommande.LesSousCategories != null && LaGestionCommande.LesSousCategories.Count > 0)
                    {
                        cbSousCategorie.ItemsSource = LaGestionCommande.LesSousCategories;
                        cbSousCategorie.DisplayMemberPath = "NomSousCategorie";
                        cbSousCategorie.SelectedValuePath = "NumSousCategorie";
                    }
                    else
                    {
                        MessageBox.Show("Aucune sous-catégorie disponible. Veuillez contacter l'administrateur.", "Données manquantes",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    if (LaGestionCommande.LesPeriodes != null && LaGestionCommande.LesPeriodes.Count > 0)
                    {
                        cbPeriode.ItemsSource = LaGestionCommande.LesPeriodes;
                        cbPeriode.DisplayMemberPath = "LibellePeriode";
                        cbPeriode.SelectedValuePath = "NumPeriode";
                    }
                    else
                    {
                        MessageBox.Show("Aucune période disponible. Veuillez contacter l'administrateur.", "Données manquantes",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Erreur de chargement des données. Veuillez redémarrer l'application.", "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValiderSaisie()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNomPlat.Text))
                {
                    MessageBox.Show("Le nom du plat est obligatoire", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNomPlat.Focus();
                    return false;
                }
                if (!double.TryParse(txtPrixUnitaire.Text, out double prix) || prix <= 0)
                {
                    MessageBox.Show("Le prix unitaire doit être un nombre positif", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPrixUnitaire.Focus();
                    return false;
                }
                if (!int.TryParse(txtDelaiPreparation.Text, out int delai) || delai <= 0)
                {
                    MessageBox.Show("Le délai de préparation doit être un nombre positif (en jours)", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtDelaiPreparation.Focus();
                    return false;
                }
                if (!int.TryParse(txtNbPersonnes.Text, out int personnes) || personnes <= 0)
                {
                    MessageBox.Show("Le nombre de personnes doit être un nombre positif", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNbPersonnes.Focus();
                    return false;
                }
                if (cbSousCategorie.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une sous-catégorie", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbSousCategorie.Focus();
                    return false;
                }
                if (cbPeriode.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner une période", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbPeriode.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la validation : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void ViderChamps()
        {
            try
            {
                txtNomPlat.Text = "";
                txtPrixUnitaire.Text = "";
                txtDelaiPreparation.Text = "";
                txtNbPersonnes.Text = "";
                cbSousCategorie.SelectedIndex = -1;
                cbPeriode.SelectedIndex = -1;
                txtNomPlat.Focus();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCreerPlat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValiderSaisie())
                {
                    return;
                }
                Plat nouveauPlat = new Plat();
                nouveauPlat.NomPlat = txtNomPlat.Text.Trim();
                nouveauPlat.PrixUnitaire = double.Parse(txtPrixUnitaire.Text);
                nouveauPlat.DelaiPreparation = int.Parse(txtDelaiPreparation.Text);
                nouveauPlat.NbPersonnes = int.Parse(txtNbPersonnes.Text);
                SousCategorie sousCategorie = (SousCategorie)cbSousCategorie.SelectedItem;
                Periode periode = (Periode)cbPeriode.SelectedItem;
                if (sousCategorie == null || periode == null)
                {
                    MessageBox.Show("Erreur : sous-catégorie ou période non sélectionnée", "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                nouveauPlat.UneSousCategorie = sousCategorie;
                nouveauPlat.UnePeriode = periode;
                int nouveauNumPlat = nouveauPlat.Create();
                nouveauPlat.NumPlat = nouveauNumPlat;
                if (LaGestionCommande.LesPlats != null)
                {
                    LaGestionCommande.LesPlats.Add(nouveauPlat);
                }
                MessageBox.Show($"Le plat '{nouveauPlat.NomPlat}' a été créé avec succès !\nNuméro : {nouveauPlat.NumPlat}",
                    "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                ViderChamps();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Veuillez vérifier le format des nombres saisis (prix, délai, nombre de personnes).",
                    "Erreur de format", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du plat :\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ViderChamps();
        }

        public void RafraichirDonnees()
        {
            try
            {
                ConfigurerComboBoxes();
            }
            catch (Exception ex)
            {
            }
        }
    }
}