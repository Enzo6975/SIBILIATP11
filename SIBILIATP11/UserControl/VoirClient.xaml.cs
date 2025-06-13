using SIBILIATP11.Classe;
using SIBILIATP11.Windows;
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

        private void BtnCreerClient_Click(object sender, RoutedEventArgs e)
        {
            WindowCreerClient windowCreerClient = new WindowCreerClient();
            bool? result = windowCreerClient.ShowDialog();

            if (result == true)
            {
                // Recharger la liste des clients après création
                RafraichirDonnees();
            }
        }

        private void BtnModifierClient_Click(object sender, RoutedEventArgs e)
        {
            if (clients.SelectedItem is Client clientSelectionne)
            {
                WindowModifierClient windowModifierClient = new WindowModifierClient(clientSelectionne);
                bool? result = windowModifierClient.ShowDialog();

                if (result == true)
                {
                    // Recharger la liste des clients après modification
                    RafraichirDonnees();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à modifier.", "Aucun client sélectionné", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSupprimerClient_Click(object sender, RoutedEventArgs e)
        {
            if (clients.SelectedItem is Client clientSelectionne)
            {
                // Premier message de confirmation
                MessageBoxResult result1 = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer le client :\n\n{clientSelectionne.NomClient} {clientSelectionne.PrenomClient}\nTéléphone : {clientSelectionne.Tel}",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result1 == MessageBoxResult.Yes)
                {
                    // Deuxième message de confirmation
                    MessageBoxResult result2 = MessageBox.Show(
                        "ATTENTION : Cette action est irréversible !\n\nConfirmez-vous définitivement la suppression de ce client ?",
                        "Confirmation finale",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result2 == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Supposons que la classe Client a une méthode Delete()
                            clientSelectionne.Delete();

                            MessageBox.Show($"Le client '{clientSelectionne.NomClient} {clientSelectionne.PrenomClient}' a été supprimé avec succès.",
                                "Suppression réussie", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Recharger la liste des clients après suppression
                            RafraichirDonnees();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur lors de la suppression du client : " + ex.Message,
                                "Erreur de base de données", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.", "Aucun client sélectionné", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Activer/désactiver les boutons selon la sélection
            bool isClientSelected = clients.SelectedItem != null;
            btnModifierClient.IsEnabled = isClientSelected;
            btnSupprimerClient.IsEnabled = isClientSelected;
        }

        // Méthode publique pour recharger les données (utile si les données sont modifiées ailleurs)
        public void RafraichirDonnees()
        {
            LoadClients();
        }
    }
}