using SIBILIATP11.Classe;
using SIBILIATP11.UserControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIBILIATP11.Windows
{
    public partial class WindowSelectionnerClient : Window
    {
        public ObservableCollection<Client> ClientsList { get; set; }
        private List<Client> allClients;
        public Client ClientSelectionne { get; private set; }

        public WindowSelectionnerClient()
        {
            InitializeComponent();
            ClientsList = new ObservableCollection<Client>();
            allClients = new List<Client>();
            LoadClients();
            this.DataContext = ClientsList;
            clients.MouseDoubleClick += Clients_MouseDoubleClick;
        }

        private void LoadClients()
        {
            try
            {
                List<SIBILIATP11.Classe.Client> clientsFromDb = new SIBILIATP11.Classe.Client().FindAll();
                ClientsList.Clear();
                allClients.Clear();
                foreach (Client client in clientsFromDb)
                {
                    ClientsList.Add(client);
                    allClients.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des clients : " + ex.Message, "Erreur de base de données", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtRecherche.Text.ToLower().Trim();
            ClientsList.Clear();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (Client client in allClients)
                {
                    ClientsList.Add(client);
                }
            }
            else
            {
                List<Client> filteredClients = allClients.Where(client =>
                    (!string.IsNullOrEmpty(client.NomClient) && client.NomClient.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.PrenomClient) && client.PrenomClient.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.Tel) && client.Tel.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.AdresseRue) && client.AdresseRue.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.AdresseVille) && client.AdresseVille.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.AdresseCP) && client.AdresseCP.ToLower().Contains(searchText))
                ).ToList();
                foreach (Client client in filteredClients)
                {
                    ClientsList.Add(client);
                }
            }
        }

        private void BtnEffacerRecherche_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = string.Empty;
            txtRecherche.Focus();
        }

        private void BtnAjouterClient_Click(object sender, RoutedEventArgs e)
        {
            WindowCreerClient creerClientWindow = new WindowCreerClient
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            bool? result = creerClientWindow.ShowDialog();
            if (result == true)
            {
                LoadClients();
                txtRecherche.Text = string.Empty;
                if (creerClientWindow.ClientEnEdition != null && creerClientWindow.ClientEnEdition.NumClient > 0)
                {
                    Client newClient = ClientsList.FirstOrDefault(c => c.NumClient == creerClientWindow.ClientEnEdition.NumClient);
                    if (newClient != null)
                    {
                        clients.SelectedItem = newClient;
                        clients.ScrollIntoView(newClient);
                    }
                }
            }
        }

        private void ValiderSelection(object sender, RoutedEventArgs e)
        {
            if (clients.SelectedItem is Client clientSelectionne)
            {
                ClientSelectionne = clientSelectionne;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Aucune sélection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AnnulerSelection(object sender, RoutedEventArgs e)
        {
            ClientSelectionne = null;
            this.DialogResult = false;
            this.Close();
        }

        private void Clients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (clients.SelectedItem is Client)
            {
                ValiderSelection(sender, e);
            }
        }
    }
}