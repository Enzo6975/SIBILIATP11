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

namespace SIBILIATP11.UserControl
{
    /// <summary>
    /// Logique d'interaction pour VoirClient.xaml
    /// </summary>
    public partial class VoirClient : System.Windows.Controls.UserControl
    {
        public ObservableCollection<Client> ClientsList { get; set; }

        public VoirClient()
        {
            // Initialisez votre ObservableCollection
            ClientsList = new ObservableCollection<Client>();
            InitializeComponent();
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
                List<SIBILIATP11.Classe.Client> clientsFromDb = new SIBILIATP11.Classe.Client().FindAll();

                // Efface la liste actuelle pour éviter les doublons lors du rechargement
                ClientsList.Clear();

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
    }
}
