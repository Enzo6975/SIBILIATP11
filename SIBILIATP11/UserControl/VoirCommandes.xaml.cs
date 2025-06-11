using SIBILIATP11.Classe;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIBILIATP11.UserControl
{
    public partial class VoirCommandes : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private Commande commandeSelectionnee;
        private ObservableCollection<Contient> platsCommandeSelectionnee;
        private ObservableCollection<Commande> commandesOriginales;
        private ObservableCollection<Commande> commandesFiltrees;
        private string filtreClient;

        public VoirCommandes()
        {
            InitializeComponent();
            this.DataContext = this;
            PlatsCommandeSelectionnee = new ObservableCollection<Contient>();
            CommandesFiltrees = new ObservableCollection<Commande>();
            CommandesOriginales = new ObservableCollection<Commande>();

            // Charger les données au démarrage
            ChargerCommandes();
        }

        // Propriété pour la commande sélectionnée
        public Commande CommandeSelectionnee
        {
            get { return commandeSelectionnee; }
            set
            {
                commandeSelectionnee = value;
                OnPropertyChanged(nameof(CommandeSelectionnee));
                ChargerPlatsCommande();
            }
        }

        // Propriété pour les plats de la commande sélectionnée
        public ObservableCollection<Contient> PlatsCommandeSelectionnee
        {
            get { return platsCommandeSelectionnee; }
            set
            {
                platsCommandeSelectionnee = value;
                OnPropertyChanged(nameof(PlatsCommandeSelectionnee));
            }
        }

        // Propriété pour les commandes filtrées (affichées dans le DataGrid)
        public ObservableCollection<Commande> CommandesFiltrees
        {
            get { return commandesFiltrees; }
            set
            {
                commandesFiltrees = value;
                OnPropertyChanged(nameof(CommandesFiltrees));
            }
        }

        public ObservableCollection<Commande> CommandesOriginales { get; private set; }

        // Propriété pour le filtre par client
        public string FiltreClient
        {
            get { return filtreClient; }
            set
            {
                filtreClient = value;
                OnPropertyChanged(nameof(FiltreClient));
                AppliquerFiltres();
            }
        }

        // Méthode pour charger toutes les commandes
        public void ChargerCommandes()
        {
            try
            {
                CommandesOriginales.Clear();

                // Charger directement depuis la base de données
                Commande commandeTemp = new Commande();
                List<Commande> toutesLesCommandes = commandeTemp.FindAll();

                // Charger les données complètes des clients pour chaque commande
                Client clientTemp = new Client();
                List<Client> tousLesClients = clientTemp.FindAll();

                foreach (var commande in toutesLesCommandes)
                {
                    // Associer le client complet à la commande
                    var clientComplet = tousLesClients.FirstOrDefault(c => c.NumClient == commande.UnClient.NumClient);
                    if (clientComplet != null)
                    {
                        commande.UnClient = clientComplet;
                    }

                    CommandesOriginales.Add(commande);
                }

                AppliquerFiltres();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des commandes: {ex.Message}");
            }
        }

        // Méthode pour appliquer les filtres
        private void AppliquerFiltres()
        {
            CommandesFiltrees.Clear();

            var commandesFiltrees = CommandesOriginales.AsEnumerable();

            // Filtre par client
            if (!string.IsNullOrWhiteSpace(FiltreClient))
            {
                commandesFiltrees = commandesFiltrees.Where(c =>
                    c.UnClient != null &&
                    (c.UnClient.NomClient.ToLower().Contains(FiltreClient.ToLower()) ||
                     c.UnClient.PrenomClient.ToLower().Contains(FiltreClient.ToLower())));
            }

            foreach (var commande in commandesFiltrees)
            {
                CommandesFiltrees.Add(commande);
            }
        }

        // Méthode pour charger les plats de la commande sélectionnée
        private void ChargerPlatsCommande()
        {
            PlatsCommandeSelectionnee.Clear();
            if (CommandeSelectionnee != null)
            {
                try
                {
                    // Charger directement depuis la base de données
                    Contient contientTemp = new Contient();
                    List<Contient> tousLesContients = contientTemp.FindAll();

                    // Filtrer pour la commande sélectionnée
                    var platsCommande = tousLesContients
                        .Where(c => c.UneCommande.NumCommande == CommandeSelectionnee.NumCommande)
                        .ToList();

                    foreach (var contient in platsCommande)
                    {
                        PlatsCommandeSelectionnee.Add(contient);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement des plats: {ex.Message}");
                }
            }
        }

        // Méthode appelée quand la case à cocher "Commande retirée" est modifiée
        private void OnCommandeRetireeChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Commande commande)
            {
                try
                {
                    // Ici vous pouvez ajouter la logique pour sauvegarder en base
                    // Par exemple, appeler une méthode Update() si elle est implémentée
                    // commande.Update();

                    // Optionnel : afficher un message de confirmation
                    string status = commande.Retiree ? "retirée" : "en attente";
                    MessageBox.Show($"Commande #{commande.NumCommande} marquée comme {status}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour: {ex.Message}");
                    // Annuler le changement en cas d'erreur
                    commande.Retiree = !commande.Retiree;
                }
            }
        }

        // Command pour effacer les filtres
        public ICommand EffacerFiltresCommand => new RelayCommand(EffacerFiltres);

        private void EffacerFiltres()
        {
            FiltreClient = string.Empty;
        }

        // Implémentation INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe helper pour les commandes
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => execute();
    }
}