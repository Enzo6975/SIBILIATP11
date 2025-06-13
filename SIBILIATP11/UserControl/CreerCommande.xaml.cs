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
using SIBILIATP11.Model;

namespace SIBILIATP11.UserControl
{
    public partial class CreerCommande : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private GestionCommande LaGestionCommande { get; set; }

        private Commande _commandeEnCours;

        private Client clientSelectionne;
        public Commande CommandeEnCours
        {
            get { return _commandeEnCours; }
            set
            {
                _commandeEnCours = value;
                OnPropertyChanged(nameof(CommandeEnCours));
                OnPropertyChanged(nameof(NomClientAffiche));
            }
        }

        public ObservableCollection<Contient> LignesDeLaCommande { get; set; }

        public string NomClientAffiche
        {
            get
            {
                if (CommandeEnCours?.UnClient != null)
                {
                    return $"{CommandeEnCours.UnClient.PrenomClient} {CommandeEnCours.UnClient.NomClient}";
                }
                return "Aucun client sélectionné";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CreerCommande()
        {
            InitializeComponent();
            InitialiserGestionCommande();

            CommandeEnCours = new Commande
            {
                DateCommande = DateTime.Now,
                UnClient = null,
                UnEmploye = LaGestionCommande?.LesEmploye?.FirstOrDefault()
            };

            LignesDeLaCommande = new ObservableCollection<Contient>();
            this.DataContext = this;
            this.Loaded += CreerCommande_Loaded;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        // Configuration du ComboBox des catégories (inchangé)
        private void ConfigurerComboBoxCategories()
        {
            var categoriesAvecTous = new List<object>();
            categoriesAvecTous.Add(new { NumCategorie = -1, NomCategorie = "Toutes les catégories" });
            if (LaGestionCommande.LesCategories != null)
            {
                foreach (var categorie in LaGestionCommande.LesCategories)
                {
                    categoriesAvecTous.Add(categorie);
                }
            }

            cbCategorie.ItemsSource = categoriesAvecTous;
            cbCategorie.DisplayMemberPath = "NomCategorie";
            cbCategorie.SelectedValuePath = "NumCategorie";
            cbCategorie.SelectedIndex = 0;

            System.Diagnostics.Debug.WriteLine($"Catégories configurées : {categoriesAvecTous.Count - 1}");
        }

        // NOUVEAU : Configuration du ComboBox des sous-catégories
        private void ConfigurerComboBoxSousCategories()
        {
            var sousCategoriesAvecTous = new List<object>();
            sousCategoriesAvecTous.Add(new { NumSousCategorie = -1, NomSousCategorie = "Toutes les sous-catégories" });

            if (LaGestionCommande.LesSousCategories != null)
            {
                foreach (var sousCategorie in LaGestionCommande.LesSousCategories)
                {
                    sousCategoriesAvecTous.Add(sousCategorie);
                }
            }

            cbSousCategorie.ItemsSource = sousCategoriesAvecTous;
            cbSousCategorie.DisplayMemberPath = "NomSousCategorie";
            cbSousCategorie.SelectedValuePath = "NumSousCategorie";
            cbSousCategorie.SelectedIndex = 0;

            System.Diagnostics.Debug.WriteLine($"Sous-catégories configurées : {sousCategoriesAvecTous.Count - 1}");
        }

        // NOUVEAU : Mise à jour des sous-catégories selon la catégorie sélectionnée
        private void MettreAJourSousCategories()
        {
            if (cbCategorie.SelectedValue == null)
                return;

            int categorieSelectionnee = Convert.ToInt32(cbCategorie.SelectedValue);
            var sousCategoriesFiltrées = new List<object>();
            sousCategoriesFiltrées.Add(new { NumSousCategorie = -1, NomSousCategorie = "Toutes les sous-catégories" });

            if (categorieSelectionnee == -1)
            {
                // Si "Toutes les catégories" est sélectionné, afficher toutes les sous-catégories
                if (LaGestionCommande.LesSousCategories != null)
                {
                    foreach (var sousCategorie in LaGestionCommande.LesSousCategories)
                    {
                        sousCategoriesFiltrées.Add(sousCategorie);
                    }
                }
            }
            else
            {
                // Filtrer les sous-catégories selon la catégorie sélectionnée
                if (LaGestionCommande.LesSousCategories != null)
                {
                    var sousCategoriesDeLaCategorie = LaGestionCommande.LesSousCategories
                        .Where(sc => sc.UneCategorie != null && sc.UneCategorie.NumCategorie == categorieSelectionnee)
                        .ToList();

                    foreach (var sousCategorie in sousCategoriesDeLaCategorie)
                    {
                        sousCategoriesFiltrées.Add(sousCategorie);
                    }
                }
            }

            cbSousCategorie.ItemsSource = sousCategoriesFiltrées;
            cbSousCategorie.SelectedIndex = 0;

            System.Diagnostics.Debug.WriteLine($"Sous-catégories filtrées : {sousCategoriesFiltrées.Count - 1} pour catégorie {categorieSelectionnee}");
        }

        private void ConfigurerDatePicker()
        {
            if (dpDateCommande != null)
            {
                dpDateCommande.SelectedDate = DateTime.Today.AddDays(1);
                dpDateCommande.DisplayDateStart = DateTime.Today.AddDays(1);
            }
        }

        private void ConfigurerDatePickerRetrait()
        {
            if (dateRetrait != null)
            {
                DateTime dateMinimale = DateTime.Today.AddDays(1);

                int delaiMaxPreparation = CalculerDelaiPreparationMax();

                if (delaiMaxPreparation > 0)
                {
                    dateMinimale = DateTime.Today.AddDays(delaiMaxPreparation);
                }

                dateRetrait.DisplayDateStart = dateMinimale;

                if (!dateRetrait.SelectedDate.HasValue || dateRetrait.SelectedDate.Value < dateMinimale)
                {
                    dateRetrait.SelectedDate = dateMinimale;
                }
            }
        }

        private int CalculerDelaiPreparationMax()
        {
            if (LignesDeLaCommande == null || !LignesDeLaCommande.Any())
                return 0;

            return LignesDeLaCommande.Max(ligne => ligne.UnPlat?.DelaiPreparation ?? 0);
        }

        private void CalculerPrixTotal()
        {
            double total = 0;
            foreach (var ligne in LignesDeLaCommande)
            {
                total += ligne.UnPlat.PrixUnitaire * ligne.Quantite;
            }
            CommandeEnCours.PrixTotal = total;
            OnPropertyChanged(nameof(CommandeEnCours));
        }

        private void ReinitialiserCommande()
        {
            LignesDeLaCommande.Clear();

            CommandeEnCours = new Commande
            {
                DateCommande = DateTime.Now,
                UnClient = null,
                UnEmploye = LaGestionCommande?.LesEmploye?.FirstOrDefault()
            };

            OnPropertyChanged(nameof(CommandeEnCours));
            OnPropertyChanged(nameof(LignesDeLaCommande));
            OnPropertyChanged(nameof(NomClientAffiche));

            ConfigurerDatePickerRetrait();
        }

        // MODIFIÉ : Filtre prenant en compte catégorie ET sous-catégorie
        private bool RechercheMotClefPlat(object obj)
        {
            Plat unPlat = obj as Plat;
            if (unPlat == null)
                return false;

            // Filtre par texte (nom du plat)
            bool correspondTexte = true;
            if (!String.IsNullOrEmpty(recherche.Text))
            {
                correspondTexte = unPlat.NomPlat.StartsWith(recherche.Text, StringComparison.OrdinalIgnoreCase);
            }

            // Filtre par catégorie
            bool correspondCategorie = true;
            if (cbCategorie.SelectedValue != null)
            {
                int categorieSelectionnee = Convert.ToInt32(cbCategorie.SelectedValue);
                if (categorieSelectionnee != -1)
                {
                    if (unPlat.UneSousCategorie != null && unPlat.UneSousCategorie.UneCategorie != null)
                    {
                        correspondCategorie = unPlat.UneSousCategorie.UneCategorie.NumCategorie == categorieSelectionnee;
                    }
                    else
                    {
                        if (unPlat.UneSousCategorie != null)
                        {
                            unPlat.UneSousCategorie.Read();
                            correspondCategorie = unPlat.UneSousCategorie.UneCategorie?.NumCategorie == categorieSelectionnee;
                        }
                        else
                        {
                            correspondCategorie = false;
                        }
                    }
                }
            }

            // NOUVEAU : Filtre par sous-catégorie
            bool correspondSousCategorie = true;
            if (cbSousCategorie.SelectedValue != null)
            {
                int sousCategorieSelectionnee = Convert.ToInt32(cbSousCategorie.SelectedValue);
                if (sousCategorieSelectionnee != -1)
                {
                    if (unPlat.UneSousCategorie != null)
                    {
                        correspondSousCategorie = unPlat.UneSousCategorie.NumSousCategorie == sousCategorieSelectionnee;
                    }
                    else
                    {
                        correspondSousCategorie = false;
                    }
                }
            }

            // Filtre par date de disponibilité
            bool correspondDate = true;
            if (dpDateCommande != null && dpDateCommande.SelectedDate.HasValue)
            {
                DateTime dateCommande = dpDateCommande.SelectedDate.Value;
                DateTime dateAujourdhui = DateTime.Today;
                int joursDisponibles = (dateCommande - dateAujourdhui).Days;
                correspondDate = unPlat.DelaiPreparation <= joursDisponibles;
            }

            // Filtre par fourchette de prix
            bool correspondPrix = true;
            if (txtPrixMin != null && !string.IsNullOrEmpty(txtPrixMin.Text))
            {
                if (double.TryParse(txtPrixMin.Text, out double prixMin))
                {
                    correspondPrix = correspondPrix && unPlat.PrixUnitaire >= prixMin;
                }
            }

            if (txtPrixMax != null && !string.IsNullOrEmpty(txtPrixMax.Text))
            {
                if (double.TryParse(txtPrixMax.Text, out double prixMax))
                {
                    correspondPrix = correspondPrix && unPlat.PrixUnitaire <= prixMax;
                }
            }

            // MODIFIÉ : Toutes les conditions doivent être vraies, y compris la sous-catégorie
            return correspondTexte && correspondCategorie && correspondSousCategorie && correspondDate && correspondPrix;
        }

        private void RefreshFilter()
        {
            if (plats.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(plats.ItemsSource).Refresh();
            }
        }

        private void ReinitialiserFiltres()
        {
            recherche.Text = "";
            cbCategorie.SelectedIndex = 0;
            cbSousCategorie.SelectedIndex = 0; // NOUVEAU
            dpDateCommande.SelectedDate = DateTime.Today.AddDays(1);
            txtPrixMin.Text = "";
            txtPrixMax.Text = "";
        }

        private List<Plat> ObtenirPlatsDisponibles(DateTime dateCommande)
        {
            var platsDisponibles = new List<Plat>();
            int joursDisponibles = (dateCommande - DateTime.Today).Days;

            if (LaGestionCommande?.LesPlats != null)
            {
                platsDisponibles = LaGestionCommande.LesPlats.Where(p => p.DelaiPreparation <= joursDisponibles).ToList();
            }
            return platsDisponibles;
        }

        private List<Plat> ObtenirPlatsDansFourchettePrix(double? prixMin, double? prixMax)
        {
            var platsFiltrés = new List<Plat>();

            if (LaGestionCommande?.LesPlats != null)
            {
                platsFiltrés = LaGestionCommande.LesPlats
                    .Where(p =>
                        (!prixMin.HasValue || p.PrixUnitaire >= prixMin.Value) &&
                        (!prixMax.HasValue || p.PrixUnitaire <= prixMax.Value))
                    .ToList();
            }
            return platsFiltrés;
        }

        private void AfficherInfoDisponibilite()
        {
            if (dpDateCommande?.SelectedDate.HasValue == true)
            {
                var platsDisponibles = ObtenirPlatsDisponibles(dpDateCommande.SelectedDate.Value);
                var totalPlats = LaGestionCommande?.LesPlats?.Count ?? 0;
            }
        }

        private void AfficherStatistiquesPrix()
        {
            if (LaGestionCommande?.LesPlats != null && LaGestionCommande.LesPlats.Any())
            {
                double prixMin = LaGestionCommande.LesPlats.Min(p => p.PrixUnitaire);
                double prixMax = LaGestionCommande.LesPlats.Max(p => p.PrixUnitaire);
                double prixMoyen = LaGestionCommande.LesPlats.Average(p => p.PrixUnitaire);
            }
        }

        private bool ValiderFourchettePrix()
        {
            if (txtPrixMin != null && txtPrixMax != null &&
                !string.IsNullOrEmpty(txtPrixMin.Text) && !string.IsNullOrEmpty(txtPrixMax.Text))
            {
                if (double.TryParse(txtPrixMin.Text, out double prixMin) &&
                    double.TryParse(txtPrixMax.Text, out double prixMax))
                {
                    if (prixMin > prixMax)
                    {
                        MessageBox.Show("Le prix minimum ne peut pas être supérieur au prix maximum.",
                                        "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
            }
            return true;
        }

        private void PreChargerDonneesCategories()
        {
            if (LaGestionCommande?.LesPlats != null)
            {
                foreach (var plat in LaGestionCommande.LesPlats)
                {
                    if (plat.UneSousCategorie != null && plat.UneSousCategorie.UneCategorie == null)
                    {
                        plat.UneSousCategorie.Read();
                    }
                }
            }
        }

        private void CreerCommande_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= CreerCommande_Loaded;

            if (LaGestionCommande != null)
            {
                PreChargerDonneesCategories();

                plats.ItemsSource = LaGestionCommande.LesPlats;
                plats.Items.Filter = RechercheMotClefPlat;
                ConfigurerComboBoxCategories();
                ConfigurerComboBoxSousCategories(); // NOUVEAU
                ConfigurerDatePicker();
                ConfigurerDatePickerRetrait();

                System.Diagnostics.Debug.WriteLine($"Plats chargés : {LaGestionCommande.LesPlats?.Count ?? 0}");
                System.Diagnostics.Debug.WriteLine($"Catégories disponibles : {LaGestionCommande.LesCategories?.Count ?? 0}");
                System.Diagnostics.Debug.WriteLine($"Sous-catégories disponibles : {LaGestionCommande.LesSousCategories?.Count ?? 0}");
            }

            recherche.TextChanged += Recherche_TextChanged;
            cbCategorie.SelectionChanged += CbCategorie_SelectionChanged;
            cbSousCategorie.SelectionChanged += CbSousCategorie_SelectionChanged; // NOUVEAU

            if (dpDateCommande != null)
                dpDateCommande.SelectedDateChanged += DpDateCommande_SelectedDateChanged;

            if (txtPrixMin != null)
                txtPrixMin.TextChanged += TxtPrix_TextChanged;
            if (txtPrixMax != null)
                txtPrixMax.TextChanged += TxtPrix_TextChanged;
        }

        private void Recherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshFilter();
        }

        private void CbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MettreAJourSousCategories(); // NOUVEAU : Met à jour les sous-catégories
            RefreshFilter();
        }

        // NOUVEAU : Gestionnaire pour le changement de sous-catégorie
        private void CbSousCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSousCategorie.SelectedValue != null)
            {
                int sousCategorieSelectionnee = Convert.ToInt32(cbSousCategorie.SelectedValue);
                if (sousCategorieSelectionnee != -1)
                {
                    var sousCategorie = LaGestionCommande.LesSousCategories?.FirstOrDefault(sc => sc.NumSousCategorie == sousCategorieSelectionnee);
                    System.Diagnostics.Debug.WriteLine($"Sous-catégorie sélectionnée : {sousCategorie?.NomSousCategorie ?? "Inconnue"}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Toutes les sous-catégories sélectionnées");
                }
            }
            RefreshFilter();
        }

        private void DpDateCommande_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshFilter();
        }

        private void TxtPrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string texte = textBox.Text;
                if (!string.IsNullOrEmpty(texte))
                {
                    if (!double.TryParse(texte, out _))
                    {
                        textBox.Foreground = System.Windows.Media.Brushes.Red;
                        return;
                    }
                    else
                    {
                        textBox.Foreground = System.Windows.Media.Brushes.Black;
                    }
                }
                else
                {
                    textBox.Foreground = System.Windows.Media.Brushes.Black;
                }
            }
            RefreshFilter();
        }

        private void AjouterPlat_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Plat platSelectionne)
            {
                Contient ligneExistante = LignesDeLaCommande.FirstOrDefault(c => c.UnPlat.NumPlat == platSelectionne.NumPlat);

                if (ligneExistante != null)
                {
                    ligneExistante.Quantite++;
                }
                else
                {
                    Contient nouvelleLigne = new Contient
                    {
                        UnPlat = platSelectionne,
                        UneCommande = this.CommandeEnCours,
                        Quantite = 1
                    };
                    LignesDeLaCommande.Add(nouvelleLigne);
                }

                CalculerPrixTotal();
                ConfigurerDatePickerRetrait();
            }
        }

        private void ValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LignesDeLaCommande.Count == 0)
                {
                    MessageBox.Show("Veuillez ajouter au moins un plat à la commande.", "Commande vide",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (CommandeEnCours.UnClient == null)
                {
                    MessageBox.Show("Veuillez sélectionner un client.", "Client manquant",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!dateRetrait.SelectedDate.HasValue)
                {
                    MessageBox.Show("Veuillez sélectionner une date de retrait.", "Date de retrait manquante",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                CommandeEnCours.DateRetraitPrevue = dateRetrait.SelectedDate.Value;

                CommandeEnCours.Create();
                LaGestionCommande.LesCommandes.Add(CommandeEnCours);

                foreach (var ligne in LignesDeLaCommande)
                {
                    ligne.UneCommande = CommandeEnCours;
                    ligne.Create();
                    LaGestionCommande.LesContients.Add(ligne);
                }

                MessageBox.Show("Commande enregistrée avec succès !", "Succès",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                ReinitialiserCommande();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement de la commande : {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void selectionClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowSelectionnerClient fenetreSelection = new WindowSelectionnerClient()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Window.GetWindow(this)
                };

                bool? result = fenetreSelection.ShowDialog();

                if (result == true && fenetreSelection.ClientSelectionne != null)
                {
                    CommandeEnCours.UnClient = fenetreSelection.ClientSelectionne;
                    OnPropertyChanged(nameof(CommandeEnCours));
                    OnPropertyChanged(nameof(NomClientAffiche));

                    MessageBox.Show($"Client sélectionné : {fenetreSelection.ClientSelectionne.PrenomClient} {fenetreSelection.ClientSelectionne.NomClient}",
                        "Client sélectionné", MessageBoxButton.OK, MessageBoxImage.Information);

                    AffichageClient();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sélection du client : {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AffichageClient()
        {
            try
            {
                if (CommandeEnCours.UnClient != null)
                {
                    txtClient.Text = $"{CommandeEnCours.UnClient.NomClient} {CommandeEnCours.UnClient.PrenomClient}";
                }
                else
                {
                    txtClient.Text = "Aucun Client";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur affichage Client: {ex.Message}");
            }
        }

        private void SupprimerPlat_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Contient ligneASupprimer)
            {
                LignesDeLaCommande.Remove(ligneASupprimer);
                CalculerPrixTotal();
                ConfigurerDatePickerRetrait();
            }
        }
    }
}