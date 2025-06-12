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

                // Configurer la ComboBox des catégories si nécessaire
                cbCategorie.ItemsSource = LaGestionCommande.LesCategories;
                cbCategorie.DisplayMemberPath = "NomCategorie";
                cbCategorie.SelectedValuePath = "NumCategorie";
            }

            // Ajouter l'événement TextChanged pour le filtrage en temps réel
            recherche.TextChanged += Recherche_TextChanged;
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

        // Méthode de filtrage
        private bool RechercheMotClefPlat(object obj)
        {
            if (String.IsNullOrEmpty(recherche.Text))
                return true;

            Plat unPlat = obj as Plat;
            if (unPlat == null)
                return false;

            return (unPlat.NomPlat.StartsWith(recherche.Text, StringComparison.OrdinalIgnoreCase));
        }

        // Event handler pour le filtrage en temps réel
        private void Recherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (plats.ItemsSource != null)
            {
                CollectionViewSource.GetDefaultView(plats.ItemsSource).Refresh();
            }
        }
    }
}