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
    /// Logique d'interaction pour SelectionnerClient.xaml
    /// </summary>
    public partial class SelectionnerClient : System.Windows.Controls.UserControl
    { 

        public ObservableCollection<Client> ClientsList { get; set; }

        public SelectionnerClient()
        {
            InitializeComponent();

            // Initialisez votre ObservableCollection
            ClientsList = new ObservableCollection<Client>();

            // Chargez les clients depuis la base de données
            LoadClients();

            // Définissez le DataContext de ce UserControl à votre collection de clients
            this.DataContext = ClientsList;
        }

        // Méthode pour charger les clients
        private void LoadClients()
        {
            try
            {
                // Utilisez l'instance Singleton de DataAccess pour récupérer les clients
                List<Client> clientsFromDb = DataAccess.Instance.GetAllClients();

                // Ajoutez chaque client récupéré à votre ObservableCollection
                foreach (var client in clientsFromDb)
                {
                    ClientsList.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des clients : " + ex.Message, "Erreur de base de données", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void butCreerClient_Click(object sender, RoutedEventArgs e)
        {
            // Trouver la fenêtre parente qui contient le ContentControl
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow is MainWindow mainWindow)
            {

                CreerClient creerClientUserControl = new CreerClient();


                mainWindow.Sibilia.Content = creerClientUserControl;
            }
            else
            {
                MessageBox.Show("Impossible de trouver la fenêtre principale ou le conteneur de contenu.");
            }
        }


    }
}
