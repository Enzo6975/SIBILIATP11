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

            if (string.IsNullOrWhiteSpace(ClientEnEdition.NomClient) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.PrenomClient) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.Tel) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.AdresseRue) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.AdresseCP) ||
                string.IsNullOrWhiteSpace(ClientEnEdition.AdresseVille))
            {
                MessageBox.Show(" Veuillez remplir tous les champs requis.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }


        }
    }
}
