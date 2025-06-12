using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Logique d'interaction pour CreerCommande.xaml
    /// </summary>
    public partial class CreerCommande : System.Windows.Controls.UserControl
    {
        private GestionCommande LaGestionCommande { get; set; }

        public CreerCommande()
        {
            InitializeComponent();
            // Initialiser la gestion
            InitialiserGestionCommande();
            // Différer l'initialisation après le chargement
            this.Loaded += CreerCommande_Loaded;
        }

        private void CreerCommande_Loaded(object sender, RoutedEventArgs e)
        {
            // Se désabonner pour éviter les appels multiples
            this.Loaded -= CreerCommande_Loaded;

            // Configurer le DataGrid et le filtrage
            if (LaGestionCommande != null)
            {
                plats.ItemsSource = LaGestionCommande.LesPlats;

                // Configurer le filtre
                plats.Items.Filter = RechercheMotClefPlat;

                // Configurer la ComboBox des catégories
                ConfigurerComboBoxCategories();

                // Configurer le DatePicker avec la date d'aujourd'hui par défaut
                ConfigurerDatePicker();
            }

            // Ajouter les événements pour le filtrage en temps réel
            recherche.TextChanged += Recherche_TextChanged;
            cbCategorie.SelectionChanged += CbCategorie_SelectionChanged;
            
            // Ajouter l'événement pour le filtrage par date
            if (dpDateCommande != null)
                dpDateCommande.SelectedDateChanged += DpDateCommande_SelectedDateChanged;

            // Ajouter les événements pour le filtrage par prix
            if (txtPrixMin != null)
                txtPrixMin.TextChanged += TxtPrix_TextChanged;
            if (txtPrixMax != null)
                txtPrixMax.TextChanged += TxtPrix_TextChanged;
        }

        private void ConfigurerComboBoxCategories()
        {
            // Créer une liste avec "Toutes les catégories" en premier
            var categoriesAvecTous = new List<object>();

            // Ajouter l'option "Toutes les catégories"
            categoriesAvecTous.Add(new { NumCategorie = -1, NomCategorie = "Toutes les catégories" });

            // Ajouter toutes les catégories existantes
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

            // Sélectionner "Toutes les catégories" par défaut
            cbCategorie.SelectedIndex = 0;
        }

        private void ConfigurerDatePicker()
        {
            // Configurer la date par défaut (aujourd'hui + 1 jour pour permettre la préparation)
            if (dpDateCommande != null)
            {
                dpDateCommande.SelectedDate = DateTime.Today.AddDays(1);
                dpDateCommande.DisplayDateStart = DateTime.Today.AddDays(1); // Empêcher la sélection de dates passées
            }
        }

        private void InitialiserGestionCommande()
        {
            try
            {
                // Essayer de récupérer depuis MainWindow
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
                    // Créer une nouvelle instance
                    LaGestionCommande = new GestionCommande("Gestion Commandes");
                }
            }
            catch (Exception ex)
            {
                LaGestionCommande = new GestionCommande("Gestion Commandes");
            }
        }

        // Méthode de filtrage combinée (recherche + catégorie + date + prix)
        private bool RechercheMotClefPlat(object obj)
        {
            Plat unPlat = obj as Plat;
            if (unPlat == null)
                return false;

            // Filtrage par texte de recherche
            bool correspondTexte = true;
            if (!String.IsNullOrEmpty(recherche.Text))
            {
                correspondTexte = unPlat.NomPlat.StartsWith(recherche.Text, StringComparison.OrdinalIgnoreCase);
            }

            // Filtrage par catégorie
            bool correspondCategorie = true;
            if (cbCategorie.SelectedValue != null)
            {
                int categorieSelectionnee = Convert.ToInt32(cbCategorie.SelectedValue);
                if (categorieSelectionnee != -1) // -1 = "Toutes les catégories"
                {
                    // Vérifier si le plat appartient à la catégorie sélectionnée
                    // Il faut passer par la SousCategorie pour accéder à la Catégorie
                    if (unPlat.UneSousCategorie != null && unPlat.UneSousCategorie.UneCategorie != null)
                    {
                        correspondCategorie = unPlat.UneSousCategorie.UneCategorie.NumCategorie == categorieSelectionnee;
                    }
                    else
                    {
                        // Si les données de catégorie ne sont pas chargées, charger la sous-catégorie
                        if (unPlat.UneSousCategorie != null)
                        {
                            unPlat.UneSousCategorie.Read(); // Cela va charger la catégorie associée
                            correspondCategorie = unPlat.UneSousCategorie.UneCategorie?.NumCategorie == categorieSelectionnee;
                        }
                        else
                        {
                            correspondCategorie = false;
                        }
                    }
                }
            }

            // Filtrage par date et délai de préparation
            bool correspondDate = true;
            if (dpDateCommande != null && dpDateCommande.SelectedDate.HasValue)
            {
                DateTime dateCommande = dpDateCommande.SelectedDate.Value;
                DateTime dateAujourdhui = DateTime.Today;

                // Calculer le nombre de jours entre aujourd'hui et la date de commande
                int joursDisponibles = (dateCommande - dateAujourdhui).Days;

                // Le plat est disponible si le délai de préparation est inférieur ou égal aux jours disponibles
                correspondDate = unPlat.DelaiPreparation <= joursDisponibles;
            }

            // Filtrage par prix
            bool correspondPrix = true;
            
            // Vérifier le prix minimum
            if (txtPrixMin != null && !string.IsNullOrEmpty(txtPrixMin.Text))
            {
                if (double.TryParse(txtPrixMin.Text, out double prixMin))
                {
                    correspondPrix = correspondPrix && unPlat.PrixUnitaire >= prixMin;
                }
            }

            // Vérifier le prix maximum
            if (txtPrixMax != null && !string.IsNullOrEmpty(txtPrixMax.Text))
            {
                if (double.TryParse(txtPrixMax.Text, out double prixMax))
                {
                    correspondPrix = correspondPrix && unPlat.PrixUnitaire <= prixMax;
                }
            }

            return correspondTexte && correspondCategorie && correspondDate && correspondPrix;
        }

        // Event handler pour le filtrage en temps réel par texte
        private void Recherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshFilter();
        }

        // Event handler pour le changement de catégorie
        private void CbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshFilter();
        }

        // Event handler pour le changement de date
        private void DpDateCommande_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshFilter();
        }

        // Event handler pour le changement de prix (min et max)
        private void TxtPrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Valider que la saisie est numérique
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string texte = textBox.Text;
                if (!string.IsNullOrEmpty(texte))
                {
                    // Vérifier si c'est un nombre double valide
                    if (!double.TryParse(texte, out _))
                    {
                        // Optionnel : changer la couleur du texte pour indiquer une erreur
                        textBox.Foreground = System.Windows.Media.Brushes.Red;
                        return;
                    }
                    else
                    {
                        // Remettre la couleur normale
                        textBox.Foreground = System.Windows.Media.Brushes.Black;
                    }
                }
                else
                {
                    // Remettre la couleur normale si le champ est vide
                    textBox.Foreground = System.Windows.Media.Brushes.Black;
                }
            }

            RefreshFilter();
        }

        // Méthode helper pour rafraîchir le filtre
        private void RefreshFilter()
        {
            if (plats.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(plats.ItemsSource).Refresh();
            }
        }

        // Méthode pour réinitialiser les filtres (optionnelle)
        private void ReinitialiserFiltres()
        {
            recherche.Text = "";
            cbCategorie.SelectedIndex = 0; // "Toutes les catégories"
            dpDateCommande.SelectedDate = DateTime.Today.AddDays(1); // Demain par défaut
            txtPrixMin.Text = ""; // Réinitialiser prix minimum
            txtPrixMax.Text = ""; // Réinitialiser prix maximum
        }

        // Méthode pour obtenir les plats disponibles pour une date donnée
        private List<Plat> ObtenirPlatsDisponibles(DateTime dateCommande)
        {
            var platsDisponibles = new List<Plat>();
            int joursDisponibles = (dateCommande - DateTime.Today).Days;

            if (LaGestionCommande?.LesPlats != null)
            {
                platsDisponibles = LaGestionCommande.LesPlats
                    .Where(p => p.DelaiPreparation <= joursDisponibles)
                    .ToList();
            }

            return platsDisponibles;
        }

        // Méthode pour obtenir les plats dans une fourchette de prix
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

        // Méthode pour afficher des informations sur la disponibilité (optionnelle)
        private void AfficherInfoDisponibilite()
        {
            if (dpDateCommande?.SelectedDate.HasValue == true)
            {
                var platsDisponibles = ObtenirPlatsDisponibles(dpDateCommande.SelectedDate.Value);
                var totalPlats = LaGestionCommande?.LesPlats?.Count ?? 0;

                // Vous pouvez afficher ces informations dans un Label ou StatusBar
                // lblInfo.Content = $"{platsDisponibles.Count} plats disponibles sur {totalPlats} pour le {dpDateCommande.SelectedDate.Value:dd/MM/yyyy}";
            }
        }

        // Méthode pour afficher des statistiques de prix (optionnelle)
        private void AfficherStatistiquesPrix()
        {
            if (LaGestionCommande?.LesPlats != null && LaGestionCommande.LesPlats.Any())
            {
                double prixMin = LaGestionCommande.LesPlats.Min(p => p.PrixUnitaire);
                double prixMax = LaGestionCommande.LesPlats.Max(p => p.PrixUnitaire);
                double prixMoyen = LaGestionCommande.LesPlats.Average(p => p.PrixUnitaire);

                // Vous pouvez afficher ces informations dans un Label
                // lblStatsPrix.Content = $"Prix: Min {prixMin:C}, Max {prixMax:C}, Moyen {prixMoyen:C}";
            }
        }

        // Méthode pour valider la cohérence des prix min/max
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
                        // Optionnel : afficher un message d'erreur
                        MessageBox.Show("Le prix minimum ne peut pas être supérieur au prix maximum.", 
                                      "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
            }
            return true;
        }

        // Méthode pour optimiser le chargement des données (optionnelle)
        private void PreChargerDonneesCategories()
        {
            // Cette méthode peut être appelée pour pré-charger les données de catégorie
            // afin d'éviter les appels répétés à Read() dans le filtrage
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
    }
}