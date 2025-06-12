using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using SIBILIATP11.Classe;

namespace SIBILIATP11.UserControl
{
    public partial class DetailCommande : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private Commande _commandeEnCours;
        private ObservableCollection<Contient> _lignesDeLaCommande;
        private GestionCommande _laGestionCommande;

        public DetailCommande()
        {
            InitializeComponent();
            this.DataContext = this;
            LignesDeLaCommande = new ObservableCollection<Contient>();
        }

        public Commande CommandeEnCours
        {
            get { return _commandeEnCours; }
            set
            {
                if (_commandeEnCours != value)
                {
                    _commandeEnCours = value;
                    OnPropertyChanged(nameof(CommandeEnCours));
                    OnPropertyChanged(nameof(NombreArticles));
                }
            }
        }

        public ObservableCollection<Contient> LignesDeLaCommande
        {
            get { return _lignesDeLaCommande; }
            set
            {
                if (_lignesDeLaCommande != value)
                {
                    _lignesDeLaCommande = value;
                    OnPropertyChanged(nameof(LignesDeLaCommande));
                    OnPropertyChanged(nameof(NombreArticles));
                }
            }
        }

        public GestionCommande LaGestionCommande
        {
            get { return _laGestionCommande; }
            set
            {
                if (_laGestionCommande != value)
                {
                    _laGestionCommande = value;
                    OnPropertyChanged(nameof(LaGestionCommande));
                }
            }
        }

        public int NombreArticles
        {
            get
            {
                return LignesDeLaCommande?.Sum(l => l.Quantite) ?? 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CalculerTotal()
        {
            if (CommandeEnCours != null)
            {
                double total = LignesDeLaCommande.Sum(l => l.UnPlat.PrixUnitaire * l.Quantite);
                CommandeEnCours.PrixTotal = total;
                OnPropertyChanged(nameof(CommandeEnCours));
            }
        }

        private void AugmenterQuantite_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Contient ligne)
            {
                ligne.Quantite++;
                CalculerTotal();
                OnPropertyChanged(nameof(NombreArticles));
            }
        }

        private void DiminuerQuantite_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Contient ligne)
            {
                if (ligne.Quantite > 1)
                {
                    ligne.Quantite--;
                    CalculerTotal();
                    OnPropertyChanged(nameof(NombreArticles));
                }
            }
        }

        private void SupprimerLigne_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Contient ligne)
            {
                LignesDeLaCommande.Remove(ligne);
                CalculerTotal();
                OnPropertyChanged(nameof(NombreArticles));
            }
        }

        private void ViderPanier_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir vider le panier ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LignesDeLaCommande.Clear();
                CalculerTotal();
                OnPropertyChanged(nameof(NombreArticles));
            }
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            LignesDeLaCommande.Clear();
            CommandeEnCours = null;
        }

        private void ValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            if (CommandeEnCours == null || LignesDeLaCommande.Count == 0)
            {
                MessageBox.Show("Impossible de valider une commande vide.", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                CommandeEnCours.Create();

                foreach (var ligne in LignesDeLaCommande)
                {
                    ligne.UneCommande = CommandeEnCours;
                    ligne.Create();
                }

                MessageBox.Show("Commande validée avec succès !", "Succès",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                LignesDeLaCommande.Clear();
                CommandeEnCours = null;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur lors de la validation : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}