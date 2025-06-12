using SIBILIATP11.Classe;
using SIBILIATP11.Windows;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIBILIATP11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GestionCommande LaGestion { get; set; }
        public MainWindow()
        {
            ChargeData();
            InitializeComponent();
            Window_Loaded();
        }

        public void Window_Loaded()
        {
            this.Hide();
            WindowConnexion dialogwindowmc = new WindowConnexion();
            bool? resultmc = dialogwindowmc.ShowDialog();
            if (resultmc == true)
                this.Show();
            else if (resultmc == false)
            {
                Application.Current.Shutdown();
            }
        }

        public void ChargeData()
        {
            try
            {
                LaGestion = new GestionCommande();
                this.DataContext = LaGestion;
            }
            catch (Exception ex)
            {
            }
        }

        private void ButDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded();
        }
    }
}