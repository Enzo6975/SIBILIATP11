using SIBILIATP11.Classe;
using SIBILIATP11.UserControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private List<Client> _allClients; // Liste complète pour la recherche

        public WindowSelectionnerClient()
        {
            InitializeComponent();
            ClientsList = new ObservableCollection<Client>();
            _allClients = new List<Client>();

            LoadClients();

            // Le DataContext est lié à ClientsList (comme dans le code original)
            this.DataContext = ClientsList;
        }

        private void LoadClients()
        {
            try
            {
                List<SIBILIATP11.Classe.Client> clientsFromDb = new SIBILIATP11.Classe.Client().FindAll();

                ClientsList.Clear();
                _allClients.Clear();

                foreach (var client in clientsFromDb)
                {
                    ClientsList.Add(client);
                    _allClients.Add(client);
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
                // Si la recherche est vide, afficher tous les clients
                foreach (var client in _allClients)
                {
                    ClientsList.Add(client);
                }
            }
            else
            {
                // Filtrer les clients selon les critères de recherche
                var filteredClients = _allClients.Where(client =>
                    (!string.IsNullOrEmpty(client.NomClient) && client.NomClient.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.PrenomClient) && client.PrenomClient.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.Tel) && client.Tel.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.AdresseRue) && client.AdresseRue.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.AdresseVille) && client.AdresseVille.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(client.AdresseCP) && client.AdresseCP.ToLower().Contains(searchText))
                ).ToList();

                foreach (var client in filteredClients)
                {
                    ClientsList.Add(client);
                }
            }
        }

        private void BtnEffacerRecherche_Click(object sender, RoutedEventArgs e)
        {
            // Effacer le texte de recherche
            txtRecherche.Text = string.Empty;

            // Remettre le focus sur la zone de recherche
            txtRecherche.Focus();
        }

        private void BtnAjouterClient_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de création de client
            WindowCreerClient creerClientWindow = new WindowCreerClient
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            // Utiliser ShowDialog pour attendre la fermeture de la fenêtre
            bool? result = creerClientWindow.ShowDialog();

            // Si le client a été créé avec succès, recharger la liste
            if (result == true)
            {
                LoadClients();

                // Optionnel : effacer la recherche pour voir le nouveau client
                txtRecherche.Text = string.Empty;

                // Optionnel : sélectionner le nouveau client créé
                if (creerClientWindow.ClientEnEdition != null && creerClientWindow.ClientEnEdition.NumClient > 0)
                {
                    var newClient = ClientsList.FirstOrDefault(c => c.NumClient == creerClientWindow.ClientEnEdition.NumClient);
                    if (newClient != null)
                    {
                        clients.SelectedItem = newClient;
                        clients.ScrollIntoView(newClient);
                    }
                }
            }
        }

        // Méthode obsolète à supprimer si elle existe encore
        private void butCreerClient_Click(object sender, RoutedEventArgs e)
        {
            // Cette méthode peut être supprimée car elle est remplacée par BtnAjouterClient_Click
            BtnAjouterClient_Click(sender, e);
        }
    }
}