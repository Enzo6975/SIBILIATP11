using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TD3_BindingBDPension.Model;

namespace SIBILIATP11.UserControl
{
    /// <summary>
    /// Logique d'interaction pour CreerCommande.xaml
    /// </summary>
    public partial class CreerCommande : System.Windows.Controls.UserControl
    {
        public CreerCommande()
        {
            InitializeComponent();
            plats.Items.Filter = RechercheMotClefPlat;
        }

        private bool RechercheMotClefPlat(object obj)
        {
            if (String.IsNullOrEmpty(recherche.Text))
                return true;
            Plat unPlat = obj as Plat;
            return (unPlat.NomPlat.StartsWith(recherche.Text, StringComparison.OrdinalIgnoreCase));
        }
    }
}
