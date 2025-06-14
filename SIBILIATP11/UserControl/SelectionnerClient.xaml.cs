﻿using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SIBILIATP11.Model;

namespace SIBILIATP11.UserControl
{
    public partial class SelectionnerClient : System.Windows.Controls.UserControl
    {
        public ObservableCollection<Client> ClientsList { get; set; }

        public SelectionnerClient()
        {
            InitializeComponent();
            ClientsList = new ObservableCollection<Client>();
            LoadClients();
            this.DataContext = ClientsList;
        }

        private void LoadClients()
        {
            try
            {
                List<SIBILIATP11.Classe.Client> clientsFromDb = new SIBILIATP11.Classe.Client().FindAll();
                ClientsList.Clear();
                foreach (SIBILIATP11.Classe.Client client in clientsFromDb)
                {
                    ClientsList.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des clients : " + ex.Message, "Erreur de base de données", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void butCreerClient_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow is MainWindow mainWindow)
            {
                CreerClient creerClientUserControl = new CreerClient();
                mainWindow.Sibilia.Content = creerClientUserControl;
            }
            else
            {
                MessageBox.Show("Impossible de trouver la fenêtre principale ou le conteneur de contenu.");
            }
        }
    }
}