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
    public partial class VoirClient : System.Windows.Controls.UserControl
    {
        public ObservableCollection<Client> ClientsList { get; set; }
        public ObservableCollection<Client> ClientsListFiltered { get; set; }
        private List<Client> clientsOriginals; // Liste originale pour la recherche

        public VoirClient()
        {
            ClientsList = new ObservableCollection<Client>();
            ClientsListFiltered = new ObservableCollection<Client>();
            InitializeComponent();
            LoadClients();
            this.DataContext = ClientsListFiltered;
        }

        private void LoadClients()
        {
            try
            {
                List<Client> clientsFromDb = new Client().FindAll();
                clientsOriginals = clientsFromDb; // Sauvegarder la liste originale

                ClientsList.Clear();
                ClientsListFiltered.Clear();

                foreach (var client in clientsFromDb)
                {
                    ClientsList.Add(client);
                    ClientsListFiltered.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des clients : " + ex.Message, "Erreur de base de données", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string critereRecherche = txtRecherche.Text.ToLower().Trim();

            ClientsListFiltered.Clear();

            if (string.IsNullOrEmpty(critereRecherche))
            {
                // Si la recherche est vide, afficher tous les clients
                foreach (var client in clientsOriginals)
                {
                    ClientsListFiltered.Add(client);
                }
            }
            else
            {
                // Filtrer les clients selon les critères de recherche
                var clientsFiltres = clientsOriginals.Where(client =>
                    (client.NomClient?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.PrenomClient?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.Tel?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.AdresseRue?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.AdresseCP?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.AdresseVille?.ToLower().Contains(critereRecherche) ?? false)
                ).ToList();

                foreach (var client in clientsFiltres)
                {
                    ClientsListFiltered.Add(client);
                }
            }
        }

        private void BtnEffacerRecherche_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
            txtRecherche.Focus();
        }

        // Méthode publique pour recharger les données (utile si les données sont modifiées ailleurs)
        public void RafraichirDonnees()
        {
            LoadClients();
        }
    }
}