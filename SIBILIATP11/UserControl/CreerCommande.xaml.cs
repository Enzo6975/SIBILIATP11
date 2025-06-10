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
using TD3_BindingBDPension.Model;

namespace SIBILIATP11.UserControl
{
    /// <summary>
    /// Logique d'interaction pour CreerCommande.xaml
    /// </summary>
    public partial class CreerCommande : System.Windows.Controls.UserControl
    {
        public CreerCommande()
        {
            InitializeComponent();
            this.Loaded += CreerCommande_Loaded;
        }
        private void CreerCommande_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDataGrid(); // Appelez la méthode spécifique pour les plats
        }

        private void LoadDataGrid()
        {
            string tablePlat = "plat";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT NomPlat, PrixUnitaire, DelaiPreparation, NbPersonnes FROM {tablePlat}"))
                {
                    DataTable platsData = DataAccess.Instance.ExecuteSelect(cmd);
                    if (plats != null)
                    {
                        plats.ItemsSource = platsData.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des plats : {ex.Message}", "Erreur de base de données", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
