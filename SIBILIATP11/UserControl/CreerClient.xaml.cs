using SIBILIATP11.Classe;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SIBILIATP11.Model;

namespace SIBILIATP11.UserControl
{
    public partial class CreerClient : System.Windows.Controls.UserControl
    {
        public Client ClientEnEdition { get; private set; }

        public CreerClient()
        {
            InitializeComponent();
            ClientEnEdition = new Client();
            this.DataContext = ClientEnEdition;
        }

        private void RetourToSelectionnerClient()
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is MainWindow mainWindow)
            {
                mainWindow.Sibilia.Content = new SelectionnerClient();
            }
            else
            {
                MessageBox.Show("Impossible de trouver la fenêtre principale pour la navigation.");
            }
        }

        private void butValiderClient_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in StackPanelCreerClient.Children)
            {
                if (child is TextBox textBox)
                {
                    BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression?.UpdateSource();
                }
            }
            if (string.IsNullOrWhiteSpace(ClientEnEdition.NomClient) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.PrenomClient) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.Tel) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.AdresseRue) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.AdresseCP) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.AdresseVille))
            {
                MessageBox.Show("Veuillez remplir tous les champs requis.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                int newNumClient = ClientEnEdition.Create();
                ClientEnEdition.NumClient = newNumClient;
                MessageBox.Show($"Client '{ClientEnEdition.NomClient} {ClientEnEdition.PrenomClient}' créé avec succès ! Numéro : {ClientEnEdition.NumClient}", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                RetourToSelectionnerClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création du client : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButRetourClient_Click(object sender, RoutedEventArgs e)
        {
            RetourToSelectionnerClient();
        }
    }
}