using SIBILIATP11.Classe;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SIBILIATP11.Model;

namespace SIBILIATP11.UserControl
{
    public partial class CreerPlat : System.Windows.Controls.UserControl
    {
        private GestionCommande LaGestionCommande { get; set; }
        private ObservableCollection<Plat> PlatsExistants { get; set; }

        public CreerPlat()
        {
            InitializeComponent();
            PlatsExistants = new ObservableCollection<Plat>();
            InitialiserGestionCommande();
            this.Loaded += CreerPlat_Loaded;
        }

        private void CreerPlat_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigurerComboBoxes();
            ChargerPlatsExistants();
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
                System.Diagnostics.Debug.WriteLine($"Erreur initialisation GestionCommande: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"Erreur ConfigurerComboBoxes: {ex.Message}");
            }
        }

        /// <summary>
        /// Charge tous les plats existants dans le DataGrid
        /// </summary>
        private void ChargerPlatsExistants()
        {
            try
            {
                PlatsExistants.Clear();

                if (LaGestionCommande?.LesPlats != null)
                {
                    // Utiliser les plats déjà chargés dans LaGestionCommande
                    foreach (Plat plat in LaGestionCommande.LesPlats.OrderBy(p => p.NomPlat))
                    {
                        PlatsExistants.Add(plat);
                    }
                }
                else
                {
                    // Fallback : charger directement depuis la base de données
                    var platsFromDb = new Plat().FindAll();

                    // Compléter les informations des sous-catégories et périodes
                    foreach (Plat plat in platsFromDb.OrderBy(p => p.NomPlat))
                    {
                        if (plat.UneSousCategorie != null)
                        {
                            var sousCategComplete = LaGestionCommande?.LesSousCategories?.FirstOrDefault(sc =>
                                sc.NumSousCategorie == plat.UneSousCategorie.NumSousCategorie);
                            if (sousCategComplete != null)
                                plat.UneSousCategorie = sousCategComplete;
                            else
                                plat.UneSousCategorie.Read();
                        }

                        if (plat.UnePeriode != null)
                        {
                            var periodeComplete = LaGestionCommande?.LesPeriodes?.FirstOrDefault(p =>
                                p.NumPeriode == plat.UnePeriode.NumPeriode);
                            if (periodeComplete != null)
                                plat.UnePeriode = periodeComplete;
                            else
                                plat.UnePeriode.Read();
                        }

                        PlatsExistants.Add(plat);
                    }
                }

                dgPlatsExistants.ItemsSource = PlatsExistants;

                System.Diagnostics.Debug.WriteLine($"Chargés {PlatsExistants.Count} plats existants");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des plats existants : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                System.Diagnostics.Debug.WriteLine($"Erreur ChargerPlatsExistants: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"Erreur ViderChamps: {ex.Message}");
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

                // Créer le plat en base de données
                int nouveauNumPlat = nouveauPlat.Create();
                nouveauPlat.NumPlat = nouveauNumPlat;

                // Ajouter à la collection de gestion
                if (LaGestionCommande.LesPlats != null)
                {
                    LaGestionCommande.LesPlats.Add(nouveauPlat);
                }

                // Ajouter à la collection d'affichage
                PlatsExistants.Add(nouveauPlat);

                // Trier la collection par nom de plat
                var platsTries = PlatsExistants.OrderBy(p => p.NomPlat).ToList();
                PlatsExistants.Clear();
                foreach (var plat in platsTries)
                {
                    PlatsExistants.Add(plat);
                }

                MessageBox.Show($"Le plat '{nouveauPlat.NomPlat}' a été créé avec succès !\nNuméro : {nouveauPlat.NumPlat}",
                    "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                ViderChamps();

                // Actualiser l'affichage du DataGrid
                CollectionViewSource.GetDefaultView(dgPlatsExistants.ItemsSource).Refresh();

                System.Diagnostics.Debug.WriteLine($"Plat créé : {nouveauPlat.NomPlat} (N°{nouveauPlat.NumPlat})");
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
                System.Diagnostics.Debug.WriteLine($"Erreur création plat: {ex.Message}");
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            ViderChamps();
        }

        private void btnRafraichir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Recharger les données depuis la base
                LaGestionCommande?.RechargerDonnees();

                // Reconfigurer les ComboBoxes
                ConfigurerComboBoxes();

                // Recharger les plats existants
                ChargerPlatsExistants();

                MessageBox.Show("Données actualisées avec succès !", "Actualisation",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'actualisation : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Erreur rafraîchissement: {ex.Message}");
            }
        }

        /// <summary>
        /// Méthode publique pour rafraîchir les données depuis l'extérieur
        /// </summary>
        public void RafraichirDonnees()
        {
            try
            {
                ConfigurerComboBoxes();
                ChargerPlatsExistants();
                System.Diagnostics.Debug.WriteLine("RafraichirDonnees appelé depuis l'extérieur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur RafraichirDonnees: {ex.Message}");
            }
        }
    }
}