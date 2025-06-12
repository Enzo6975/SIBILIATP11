using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
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

namespace SIBILIATP11.Windows
{
    /// <summary>
    /// Logique d'interaction pour DetailCommande.xaml
    /// </summary>
    public partial class DetailCommande : Window
    {
        private GestionCommande LaGestionCommande { get; set; }
        private Commande CommandeActuelle { get; set; }
        private ObservableCollection<Contient> PlatsCommandes { get; set; }

        // Événement pour notifier les modifications de la commande
        public event EventHandler<Commande> CommandeModifiee;
        public event EventHandler FermetureRequise;

        public DetailCommande()
        {
            InitializeComponent();
            InitialiserGestionCommande();
            PlatsCommandes = new ObservableCollection<Contient>();
            dgPlatsCommandes.ItemsSource = PlatsCommandes;
        }

        public DetailCommande(Commande commande) : this()
        {
            AfficherCommande(commande);
        }

        private void InitialiserGestionCommande()
        {
            try
            {
                // Essayer de récupérer depuis MainWindow
                if (App.Current.MainWindow?.DataContext is GestionCommande gestionDC)
                {
                    LaGestionCommande = gestionDC;
                }
                else if (App.Current.MainWindow is MainWindow mainWin && mainWin.LaGestion != null)
                {
                    LaGestionCommande = mainWin.LaGestion;
                }
                else
                {
                    // Créer une nouvelle instance
                    LaGestionCommande = new GestionCommande("Gestion Commandes");
                }
            }
            catch (Exception ex)
            {
                LaGestionCommande = new GestionCommande("Gestion Commandes");
                MessageBox.Show($"Erreur lors de l'initialisation : {ex.Message}", "Erreur",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Affiche les détails d'une commande
        /// </summary>
        /// <param name="commande">La commande à afficher</param>
        public void AfficherCommande(Commande commande)
        {
            if (commande == null)
            {
                MessageBox.Show("Aucune commande sélectionnée.", "Information",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                CommandeActuelle = commande;

                // Charger les données complètes de la commande si nécessaire
                if (CommandeActuelle.UnClient?.NomClient == null || CommandeActuelle.UnEmploye?.NomEmploye == null)
                {
                    CommandeActuelle.Read();
                }

                // Afficher les informations de base
                AfficherInformationsBase();

                // Charger et afficher les plats de la commande
                ChargerPlatsCommande();

                // Calculer et afficher les totaux
                CalculerTotaux();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'affichage de la commande : {ex.Message}",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AfficherInformationsBase()
        {
            // Informations de base
            txtNumCommande.Text = CommandeActuelle.NumCommande.ToString();
            txtDateCommande.Text = CommandeActuelle.DateCommande.ToString("dd/MM/yyyy HH:mm");
            txtDateRetrait.Text = CommandeActuelle.DateRetraitPrevue.ToString("dd/MM/yyyy");

            // Statuts avec couleurs
            txtStatutPaiement.Text = CommandeActuelle.Payee ? "Payée" : "Non payée";
            txtStatutPaiement.Foreground = CommandeActuelle.Payee ?
                new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            txtStatutRetrait.Text = CommandeActuelle.Retiree ? "Retirée" : "En attente";
            txtStatutRetrait.Foreground = CommandeActuelle.Retiree ?
                new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Orange);

            // Prix total
            txtPrixTotal.Text = CommandeActuelle.PrixTotal.ToString("C", CultureInfo.CurrentCulture);

            // Client et employé
            txtClient.Text = CommandeActuelle.UnClient != null ?
                $"{CommandeActuelle.UnClient.NomClient} {CommandeActuelle.UnClient.PrenomClient}" : "Non défini";
            txtEmploye.Text = CommandeActuelle.UnEmploye != null ?
                $"{CommandeActuelle.UnEmploye.NomEmploye} {CommandeActuelle.UnEmploye.PrenomEmploye}" : "Non défini";
        }

        private void ChargerPlatsCommande()
        {
            try
            {
                PlatsCommandes.Clear();

                // Récupérer les plats de la commande depuis la base de données
                var contients = new Contient().FindByCommande(CommandeActuelle.NumCommande);

                // Charger les informations complètes des plats
                foreach (var contient in contients)
                {
                    // S'assurer que les données du plat sont chargées
                    if (contient.UnPlat != null)
                    {
                        contient.UnPlat.Read();
                    }

                    // Ajouter à la collection observable
                    PlatsCommandes.Add(contient);
                }

                // Si aucun plat trouvé dans la base, essayer depuis la gestion en mémoire
                if (PlatsCommandes.Count == 0 && LaGestionCommande?.LesContients != null)
                {
                    var contientsMémoire = LaGestionCommande.LesContients
                        .Where(c => c.UneCommande.NumCommande == CommandeActuelle.NumCommande);

                    foreach (var contient in contientsMémoire)
                    {
                        PlatsCommandes.Add(contient);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des plats : {ex.Message}",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculerTotaux()
        {
            try
            {
                if (PlatsCommandes.Count > 0)
                {
                    // Nombre total d'articles
                    int nombreArticles = PlatsCommandes.Sum(c => c.Quantite);
                    txtNombreArticles.Text = nombreArticles.ToString();

                    // Nombre total de personnes (somme des quantités × nombre de personnes par plat)
                    int nombrePersonnes = PlatsCommandes.Sum(c => c.Quantite * c.UnPlat.NbPersonnes);
                    txtNombrePersonnes.Text = nombrePersonnes.ToString();

                    // Vérifier la cohérence du prix total
                    double prixCalculé = PlatsCommandes.Sum(c => c.CalculerTotal);
                    if (Math.Abs(prixCalculé - CommandeActuelle.PrixTotal) > 0.01) // Tolérance pour les erreurs d'arrondi
                    {
                        txtPrixTotal.Text = $"{CommandeActuelle.PrixTotal:C} (Calc: {prixCalculé:C})";
                        txtPrixTotal.Foreground = new SolidColorBrush(Colors.Orange);
                    }
                }
                else
                {
                    txtNombreArticles.Text = "0";
                    txtNombrePersonnes.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du calcul des totaux : {ex.Message}",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommandeActuelle == null)
                {
                    MessageBox.Show("Aucune commande à modifier.", "Information",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Vérifier si la commande peut être modifiée
                if (CommandeActuelle.Retiree)
                {
                    MessageBox.Show("Impossible de modifier une commande déjà retirée.",
                                  "Modification impossible", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Ouvrir une fenêtre de modification ou déclencher un événement
                CommandeModifiee?.Invoke(this, CommandeActuelle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnImprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommandeActuelle == null)
                {
                    MessageBox.Show("Aucune commande à imprimer.", "Information",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Créer un document d'impression
                ImprimerCommande();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'impression : {ex.Message}",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImprimerCommande()
        {
            try
            {
                // Créer un FlowDocument pour l'impression
                FlowDocument doc = new FlowDocument();
                doc.PageWidth = 8.5 * 96; // 8.5 pouces en pixels
                doc.PageHeight = 11 * 96; // 11 pouces en pixels
                doc.PagePadding = new Thickness(50);

                // Titre
                Paragraph titre = new Paragraph(new Run("DÉTAIL DE COMMANDE"))
                {
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                doc.Blocks.Add(titre);

                // Informations de la commande
                Table tableInfo = new Table();
                tableInfo.Columns.Add(new TableColumn() { Width = new GridLength(200) });
                tableInfo.Columns.Add(new TableColumn() { Width = new GridLength(200) });

                TableRowGroup groupeInfo = new TableRowGroup();

                // Ligne 1
                TableRow ligne1 = new TableRow();
                ligne1.Cells.Add(new TableCell(new Paragraph(new Run($"N° Commande: {CommandeActuelle.NumCommande}"))));
                ligne1.Cells.Add(new TableCell(new Paragraph(new Run($"Date: {CommandeActuelle.DateCommande:dd/MM/yyyy}"))));
                groupeInfo.Rows.Add(ligne1);

                // Ligne 2
                TableRow ligne2 = new TableRow();
                ligne2.Cells.Add(new TableCell(new Paragraph(new Run($"Date Retrait: {CommandeActuelle.DateRetraitPrevue:dd/MM/yyyy}"))));
                ligne2.Cells.Add(new TableCell(new Paragraph(new Run($"Prix Total: {CommandeActuelle.PrixTotal:C}"))));
                groupeInfo.Rows.Add(ligne2);

                // Ligne 3
                TableRow ligne3 = new TableRow();
                ligne3.Cells.Add(new TableCell(new Paragraph(new Run($"Client: {txtClient.Text}"))));
                ligne3.Cells.Add(new TableCell(new Paragraph(new Run($"Employé: {txtEmploye.Text}"))));
                groupeInfo.Rows.Add(ligne3);

                tableInfo.RowGroups.Add(groupeInfo);
                doc.Blocks.Add(tableInfo);

                // Espacement
                doc.Blocks.Add(new Paragraph(new Run("")) { Margin = new Thickness(0, 20, 0, 0) });

                // Titre des plats
                Paragraph titrePlats = new Paragraph(new Run("PLATS COMMANDÉS"))
                {
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                doc.Blocks.Add(titrePlats);

                // Table des plats
                if (PlatsCommandes.Count > 0)
                {
                    Table tablePlats = new Table();
                    tablePlats.Columns.Add(new TableColumn() { Width = new GridLength(200) }); // Plat
                    tablePlats.Columns.Add(new TableColumn() { Width = new GridLength(80) });  // Prix
                    tablePlats.Columns.Add(new TableColumn() { Width = new GridLength(60) });  // Qté
                    tablePlats.Columns.Add(new TableColumn() { Width = new GridLength(80) });  // Total

                    // En-tête
                    TableRowGroup entete = new TableRowGroup();
                    TableRow ligneEntete = new TableRow();
                    ligneEntete.Cells.Add(new TableCell(new Paragraph(new Run("Plat")) { FontWeight = FontWeights.Bold }));
                    ligneEntete.Cells.Add(new TableCell(new Paragraph(new Run("Prix")) { FontWeight = FontWeights.Bold }));
                    ligneEntete.Cells.Add(new TableCell(new Paragraph(new Run("Qté")) { FontWeight = FontWeights.Bold }));
                    ligneEntete.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { FontWeight = FontWeights.Bold }));
                    entete.Rows.Add(ligneEntete);
                    tablePlats.RowGroups.Add(entete);

                    // Données
                    TableRowGroup donnees = new TableRowGroup();
                    foreach (var plat in PlatsCommandes)
                    {
                        TableRow lignePlat = new TableRow();
                        lignePlat.Cells.Add(new TableCell(new Paragraph(new Run(plat.UnPlat.NomPlat))));
                        lignePlat.Cells.Add(new TableCell(new Paragraph(new Run(plat.Prix.ToString("F2")))));
                        lignePlat.Cells.Add(new TableCell(new Paragraph(new Run(plat.Quantite.ToString()))));
                        lignePlat.Cells.Add(new TableCell(new Paragraph(new Run(plat.CalculerTotal.ToString("F2")))));
                        donnees.Rows.Add(lignePlat);
                    }
                    tablePlats.RowGroups.Add(donnees);
                    doc.Blocks.Add(tablePlats);
                }

                // Lancer l'impression
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator,
                                            $"Commande_{CommandeActuelle.NumCommande}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du document d'impression : {ex.Message}",
                              "Erreur d'impression", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            FermetureRequise?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Rafraîchit l'affichage de la commande actuelle
        /// </summary>
        public void RafraichirAffichage()
        {
            if (CommandeActuelle != null)
            {
                AfficherCommande(CommandeActuelle);
            }
        }

        /// <summary>
        /// Met à jour le statut de paiement de la commande
        /// </summary>
        public void MarquerCommePaye()
        {
            if (CommandeActuelle != null && !CommandeActuelle.Payee)
            {
                try
                {
                    CommandeActuelle.Payee = true;
                    CommandeActuelle.Update();

                    // Rafraîchir l'affichage
                    txtStatutPaiement.Text = "Payée";
                    txtStatutPaiement.Foreground = new SolidColorBrush(Colors.Green);

                    MessageBox.Show("Commande marquée comme payée.", "Succès",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour du paiement : {ex.Message}",
                                  "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Met à jour le statut de retrait de la commande
        /// </summary>
        public void MarquerCommeRetiree()
        {
            if (CommandeActuelle != null && !CommandeActuelle.Retiree)
            {
                try
                {
                    CommandeActuelle.Retiree = true;
                    CommandeActuelle.Update();

                    // Rafraîchir l'affichage
                    txtStatutRetrait.Text = "Retirée";
                    txtStatutRetrait.Foreground = new SolidColorBrush(Colors.Green);

                    MessageBox.Show("Commande marquée comme retirée.", "Succès",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour du retrait : {ex.Message}",
                                  "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}