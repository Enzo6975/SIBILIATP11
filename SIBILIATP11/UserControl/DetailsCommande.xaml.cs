using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using SIBILIATP11.Classe; // Make sure this namespace is correct

namespace SIBILIATP11.UserControl
{
    public partial class RecapCommande : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        private Commande _uneCommande;
        private ObservableCollection<Contient> _lignesDeLaCommande;

        public RecapCommande()
        {
            InitializeComponent();
            this.DataContext = this; // Set DataContext to itself for binding
        }

        public Commande UneCommande
        {
            get { return _uneCommande; }
            set
            {
                if (_uneCommande != value)
                {
                    _uneCommande = value;
                    OnPropertyChanged(nameof(UneCommande));
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
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}