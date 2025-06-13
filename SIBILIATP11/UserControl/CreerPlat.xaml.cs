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
            this.Loaded += CreerPlat_Loaded;
        }

        private void CreerPlat_Loaded(object sender, RoutedEventArgs e)
        {
            // Configurer les ComboBoxes quand le contrôle est chargé
            ConfigurerComboBoxes();
        }

        private void InitialiserGestionCommande()
        {
            try
            {
                if (App.Current.MainWindow?.DataContext is GestionCommande gestionDC)
                {
                    LaGestionCommande = gestionDC;
                    System.Diagnostics.Debug.WriteLine("GestionCommande récupérée depuis MainWindow.DataContext");
                }
                else if (App.Current.MainWindow is MainWindow mainWin && mainWin.LaGestion != null)
                {
                    LaGestionCommande = mainWin.LaGestion;
                    System.Diagnostics.Debug.WriteLine("GestionCommande récupérée depuis MainWindow.LaGestion");
                }
                else
                {
                    LaGestionCommande = new GestionCommande("Gestion Commandes");
                    System.Diagnostics.Debug.WriteLine("Nouvelle GestionCommande créée");
                }
            }
            catch (Exception ex)
            {
                LaGestionCommande = new GestionCommande("Gestion Commandes");
                System.Diagnostics.Debug.WriteLine($"Erreur initialisation GestionCommande : {ex.Message}");
            }
        }

        private void ConfigurerComboBoxes()
        {
            try
            {
                if (LaGestionCommande != null)
                {
                    // Configuration des sous-catégories
                    if (LaGestionCommande.LesSousCategories != null && LaGestionCommande.LesSousCategories.Count > 0)
                    {
                        cbSousCategorie.ItemsSource = LaGestionCommande.LesSousCategories;
                        cbSousCategorie.DisplayMemberPath = "NomSousCategorie";
                        cbSousCategorie.SelectedValuePath = "NumSousCategorie";
                        System.Diagnostics.Debug.WriteLine($"Sous-catégories chargées : {LaGestionCommande.LesSousCategories.Count}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Aucune sous-catégorie trouvée");
                        MessageBox.Show("Aucune sous-catégorie disponible. Veuillez contacter l'administrateur.", "Données manquantes",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    // Configuration des périodes
                    if (LaGestionCommande.LesPeriodes != null && LaGestionCommande.LesPeriodes.Count > 0)
                    {
                        cbPeriode.ItemsSource = LaGestionCommande.LesPeriodes;
                        cbPeriode.DisplayMemberPath = "LibellePeriode";
                        cbPeriode.SelectedValuePath = "NumPeriode";
                        System.Diagnostics.Debug.WriteLine($"Périodes chargées : {LaGestionCommande.LesPeriodes.Count}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Aucune période trouvée");
                        MessageBox.Show("Aucune période disponible. Veuillez contacter l'administrateur.", "Données manquantes",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("LaGestionCommande est null");
                    MessageBox.Show("Erreur de chargement des données. Veuillez redémarrer l'application.", "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur configuration ComboBoxes : {ex.Message}");
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValiderSaisie()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Début validation saisie");

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

                System.Diagnostics.Debug.WriteLine("Validation saisie réussie");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur validation : {ex.Message}");
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

                // Remettre le focus sur le premier champ
                txtNomPlat.Focus();

                System.Diagnostics.Debug.WriteLine("Champs vidés");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur vidage champs : {ex.Message}");
            }
        }

        private void btnCreerPlat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Début création plat");

                if (!ValiderSaisie())
                {
                    System.Diagnostics.Debug.WriteLine("Validation échouée");
                    return;
                }

                // Créer le nouveau plat
                Plat nouveauPlat = new Plat();
                nouveauPlat.NomPlat = txtNomPlat.Text.Trim();
                nouveauPlat.PrixUnitaire = double.Parse(txtPrixUnitaire.Text);
                nouveauPlat.DelaiPreparation = int.Parse(txtDelaiPreparation.Text);
                nouveauPlat.NbPersonnes = int.Parse(txtNbPersonnes.Text);

                // Récupérer les objets sélectionnés
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

                System.Diagnostics.Debug.WriteLine($"Création plat : {nouveauPlat.NomPlat}, Prix: {nouveauPlat.PrixUnitaire}€, " +
                    $"Délai: {nouveauPlat.DelaiPreparation} jours, Personnes: {nouveauPlat.NbPersonnes}, " +
                    $"Sous-catégorie: {sousCategorie.NomSousCategorie}, Période: {periode.LibellePeriode}");

                // Sauvegarder en base de données
                int nouveauNumPlat = nouveauPlat.Create();
                nouveauPlat.NumPlat = nouveauNumPlat;

                System.Diagnostics.Debug.WriteLine($"Plat créé avec le numéro : {nouveauNumPlat}");

                // Ajouter à la gestion
                if (LaGestionCommande.LesPlats != null)
                {
                    LaGestionCommande.LesPlats.Add(nouveauPlat);
                    System.Diagnostics.Debug.WriteLine($"Plat ajouté à la collection. Total plats : {LaGestionCommande.LesPlats.Count}");
                }

                // Message de succès
                MessageBox.Show($"Le plat '{nouveauPlat.NomPlat}' a été créé avec succès !\nNuméro : {nouveauPlat.NumPlat}",
                    "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Vider les champs pour une nouvelle saisie
                ViderChamps();

                System.Diagnostics.Debug.WriteLine("Création plat terminée avec succès");
            }
            catch (FormatException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur format : {ex.Message}");
                MessageBox.Show("Veuillez vérifier le format des nombres saisis (prix, délai, nombre de personnes).",
                    "Erreur de format", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur création plat : {ex.Message}");
                MessageBox.Show($"Erreur lors de la création du plat :\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ViderChamps();
        }

        // Méthode publique pour recharger les données (appelée par MainWindow)
        public void RafraichirDonnees()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Rafraîchissement données CreerPlat");
                ConfigurerComboBoxes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rafraîchissement CreerPlat : {ex.Message}");
            }
        }
    }
}