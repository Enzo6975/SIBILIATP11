using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
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
    public partial class WindowCreerClient : Window
    {
        public Client ClientEnEdition { get; private set; }

        public WindowCreerClient()
        {
            InitializeComponent();
            ClientEnEdition = new Client();
            this.DataContext = ClientEnEdition;
        }

        private void RetourToSelectionnerClient()
        {
            this.DialogResult = false;
            this.Close();
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
                this.DialogResult = true;
                this.Close();
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