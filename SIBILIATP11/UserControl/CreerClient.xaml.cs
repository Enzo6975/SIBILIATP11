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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SIBILIATP11.Model;

namespace SIBILIATP11.UserControl
{
    /// <summary>
    /// Logique d'interaction pour CreerClient.xaml
    /// </summary>
    public partial class CreerClient : System.Windows.Controls.UserControl
    {

        public Client ClientEnEdition { get; private set; } 

        public CreerClient()
        {
            InitializeComponent();
            ClientEnEdition = new Client(); 
            this.DataContext = ClientEnEdition; 
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

            // Validation des champs (vérifie si les propriétés de ClientEnEdition sont remplies)
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

            // Si la validation passe, tenter de créer le client
            try
            {
                int newNumClient = ClientEnEdition.Create(); // Appelle la méthode Create de l'objet ClientEnEdition
                ClientEnEdition.NumClient = newNumClient; // Met à jour le NumClient de l'objet ClientEnEdition

                MessageBox.Show($"Client '{ClientEnEdition.NomClient} {ClientEnEdition.PrenomClient}' créé avec succès ! Numéro : {ClientEnEdition.NumClient}", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Après la création, revenir à la page de sélection des clients
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

        private void RetourToSelectionnerClient()
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is MainWindow mainWindow) // Assurez-vous que votre fenêtre principale est bien nommée MainWindow
            {
                mainWindow.Sibilia.Content = new SelectionnerClient();
            }
            else
            {
                MessageBox.Show("Impossible de trouver la fenêtre principale pour la navigation.");
            }
        }
    }
}
