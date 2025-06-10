using Npgsql;
using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Data;
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
using SIBILIATP11.Classe;

namespace SIBILIATP11.UserControl
{
    /// <summary>
    /// Logique d'interaction pour CreerCommande.xaml
    /// </summary>
    public partial class CreerCommande : System.Windows.Controls.UserControl
    {
        public Gestionplat gestionplat;
        public CreerCommande()
        {
            InitializeComponent();
            gestionplat = new Gestionplat();
            this.Loaded += CreerCommande_Loaded;
        }

        private void CreerCommande_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlatsIntoDataGrid();
        }

        private void LoadPlatsIntoDataGrid()
        {
            try
            {
                List<Plat> lesPlats = gestionplat.FindAll();

                if (plats != null) // Vérification de sécurité au cas où le DataGrid ne serait pas encore initialisé
                {
                    plats.ItemsSource = lesPlats;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des plats dans le DataGrid : {ex.Message}","Erreur de données", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
