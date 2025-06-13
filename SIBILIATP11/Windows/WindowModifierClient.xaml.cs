using SIBILIATP11.Classe;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace SIBILIATP11.Windows
{
    public partial class WindowModifierClient : Window
    {
        public Client ClientEnEdition { get; private set; }
        private Client clientOriginal;

        public WindowModifierClient(Client clientAModifier)
        {
            InitializeComponent();

            if (clientAModifier == null)
            {
                throw new ArgumentNullException(nameof(clientAModifier), "Le client à modifier ne peut pas être null");
            }

            ClientEnEdition = new Client
            {
                NumClient = clientAModifier.NumClient,
                NomClient = clientAModifier.NomClient,
                PrenomClient = clientAModifier.PrenomClient,
                Tel = clientAModifier.Tel,
                AdresseRue = clientAModifier.AdresseRue,
                AdresseCP = clientAModifier.AdresseCP,
                AdresseVille = clientAModifier.AdresseVille
            };

            clientOriginal = new Client
            {
                NumClient = clientAModifier.NumClient,
                NomClient = clientAModifier.NomClient,
                PrenomClient = clientAModifier.PrenomClient,
                Tel = clientAModifier.Tel,
                AdresseRue = clientAModifier.AdresseRue,
                AdresseCP = clientAModifier.AdresseCP,
                AdresseVille = clientAModifier.AdresseVille
            };

            this.DataContext = ClientEnEdition;
        }

        private void ButValiderModification_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in StackPanelModifierClient.Children)
            {
                if (child is TextBox textBox && !textBox.IsReadOnly)
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

            if (ClientEnEdition.NomClient == clientOriginal.NomClient &&
                ClientEnEdition.PrenomClient == clientOriginal.PrenomClient &&
                ClientEnEdition.Tel == clientOriginal.Tel &&
                ClientEnEdition.AdresseRue == clientOriginal.AdresseRue &&
                ClientEnEdition.AdresseCP == clientOriginal.AdresseCP &&
                ClientEnEdition.AdresseVille == clientOriginal.AdresseVille)
            {
                MessageBox.Show("Aucune modification n'a été apportée.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Confirmer la modification du client :\n\n{ClientEnEdition.NomClient} {ClientEnEdition.PrenomClient}",
                "Confirmation de modification",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ClientEnEdition.Update();

                    MessageBox.Show($"Le client '{ClientEnEdition.NomClient} {ClientEnEdition.PrenomClient}' a été modifié avec succès.",
                        "Modification réussie", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification du client : " + ex.Message,
                        "Erreur de base de données", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButAnnulerModification_Click(object sender, RoutedEventArgs e)
        {
            if (ClientEnEdition.NomClient != clientOriginal.NomClient ||
                ClientEnEdition.PrenomClient != clientOriginal.PrenomClient ||
                ClientEnEdition.Tel != clientOriginal.Tel ||
                ClientEnEdition.AdresseRue != clientOriginal.AdresseRue ||
                ClientEnEdition.AdresseCP != clientOriginal.AdresseCP ||
                ClientEnEdition.AdresseVille != clientOriginal.AdresseVille)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Des modifications non sauvegardées seront perdues.\n\nÊtes-vous sûr de vouloir annuler ?",
                    "Confirmation d'annulation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            this.DialogResult = false;
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (ClientEnEdition.NomClient != clientOriginal.NomClient ||
                ClientEnEdition.PrenomClient != clientOriginal.PrenomClient ||
                ClientEnEdition.Tel != clientOriginal.Tel ||
                ClientEnEdition.AdresseRue != clientOriginal.AdresseRue ||
                ClientEnEdition.AdresseCP != clientOriginal.AdresseCP ||
                ClientEnEdition.AdresseVille != clientOriginal.AdresseVille)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Des modifications non sauvegardées seront perdues.\n\nÊtes-vous sûr de vouloir fermer ?",
                    "Confirmation de fermeture",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnClosing(e);
        }
    }
}