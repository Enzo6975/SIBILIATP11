using SIBILIATP11.Classe;
using SIBILIATP11.UserControl;
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
using System.Windows.Shapes;

namespace SIBILIATP11.Windows
{
    public partial class WindowSelectionnerClient : Window
    {
        public ObservableCollection<Client> ClientsList { get; set; }

        public WindowSelectionnerClient()
        {
            InitializeComponent();
            ClientsList = new ObservableCollection<Client>();
            LoadClients();
            this.DataContext = ClientsList;
        }

        private void LoadClients()
        {
            try
            {
                List<Client> clientsFromDb = new Client().FindAll();
                ClientsList.Clear();
                foreach (Client client in clientsFromDb)
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
            CreerClient creerClientUserControl = new CreerClient();
            Window creerClientWindow = new Window
            {
                Title = "Créer un Client",
                Width = 800,
                Height = 500,
                Content = creerClientUserControl,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            creerClientWindow.ShowDialog();
            LoadClients();
        }
    }
}