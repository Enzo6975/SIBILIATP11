using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIBILIATP11.UserControl
{
    public partial class VoirClient : System.Windows.Controls.UserControl
    {
        public ObservableCollection<Client> ClientsList { get; set; }
        public ObservableCollection<Client> ClientsListFiltered { get; set; }
        private List<Client> clientsOriginals;

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
                clientsOriginals = clientsFromDb;
                ClientsList.Clear();
                ClientsListFiltered.Clear();
                foreach (Client client in clientsFromDb)
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
                foreach (Client client in clientsOriginals)
                {
                    ClientsListFiltered.Add(client);
                }
            }
            else
            {
                List<Client> clientsFiltres = clientsOriginals.Where(client =>
                    (client.NomClient?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.PrenomClient?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.Tel?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.AdresseRue?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.AdresseCP?.ToLower().Contains(critereRecherche) ?? false) ||
                    (client.AdresseVille?.ToLower().Contains(critereRecherche) ?? false)
                ).ToList();
                foreach (Client client in clientsFiltres)
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

        public void RafraichirDonnees()
        {
            LoadClients();
        }
    }
}