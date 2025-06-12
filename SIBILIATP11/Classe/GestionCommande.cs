using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIBILIATP11.Classe
{
    public class GestionCommande
    {
        private string nom;
        private ObservableCollection<Commande> lesCommandes;
        private ObservableCollection<Client> lesClients;
        private ObservableCollection<Plat> lesPlats;
        private ObservableCollection<Contient> lesContients;
        private List<Employe> lesEmploye;
        private List<Categorie> lesCategories;
        private List<Periode> lesPeriodes;
        private List<Role> lesRoles;
        private List<SousCategorie> lesSousCategories;

        public GestionCommande(string nom)
        {
            this.Nom = nom;

            try
            {
                this.LesClients = new ObservableCollection<Client>(new Client().FindAll());
                this.LesEmploye = new List<Employe>(new Employe().FindAll());
                this.LesCategories = new List<Categorie>(new Categorie().FindAll());
                this.LesPeriodes = new List<Periode>(new Periode().FindAll());
                this.LesRoles = new List<Role>(new Role().FindAll());
                this.LesSousCategories = new List<SousCategorie>(new SousCategorie().FindAll());

                ObservableCollection<Commande> commandes = new ObservableCollection<Commande>(new Commande().FindAll());
                foreach (Commande commande in commandes)
                {
                    Client clientComplet = LesClients.FirstOrDefault(c => c.NumClient == commande.UnClient.NumClient);
                    Employe employeComplet = LesEmploye.FirstOrDefault(e => e.NumEmploye == commande.UnEmploye.NumEmploye);

                    if (clientComplet != null)
                        commande.UnClient = clientComplet;
                    if (employeComplet != null)
                        commande.UnEmploye = employeComplet;
                }
                this.LesCommandes = commandes;

                ObservableCollection<Plat> plats = new ObservableCollection<Plat>(new Plat().FindAll());
                foreach (Plat plat in plats)
                {
                    SousCategorie souscategorieComplete = LesSousCategories.FirstOrDefault(c => c.NumSousCategorie == plat.UneSousCategorie.NumSousCategorie);
                    Periode periodeComplete = LesPeriodes.FirstOrDefault(p => p.NumPeriode == plat.UnePeriode.NumPeriode);

                    if (souscategorieComplete != null)
                        plat.UneSousCategorie = souscategorieComplete;
                    if (periodeComplete != null)
                        plat.UnePeriode = periodeComplete;
                }
                this.LesPlats = plats;

                ObservableCollection<Contient> contients = new ObservableCollection<Contient>(new Contient().FindAll());
                foreach (Contient contient in contients)
                {
                    Commande commandeComplete = LesCommandes.FirstOrDefault(c => c.NumCommande == contient.UneCommande.NumCommande);
                    Plat platComplet = LesPlats.FirstOrDefault(p => p.NumPlat == contient.UnPlat.NumPlat);

                    if (commandeComplete != null)
                        contient.UneCommande = commandeComplete;
                    if (platComplet != null)
                        contient.UnPlat = platComplet;
                }
                this.LesContients = contients;
            }
            catch (Exception ex)
            {
                this.LesClients = new ObservableCollection<Client>();
                this.LesCommandes = new ObservableCollection<Commande>();
                this.LesPlats = new ObservableCollection<Plat>();
                this.LesContients = new ObservableCollection<Contient>();
                this.LesEmploye = new List<Employe>();
                this.LesCategories = new List<Categorie>();
                this.LesPeriodes = new List<Periode>();
                this.LesRoles = new List<Role>();
                this.LesSousCategories = new List<SousCategorie>();

                throw;
            }
        }

        public GestionCommande() : this("Application Sibilia")
        {
        }

        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }

        public ObservableCollection<Commande> LesCommandes
        {
            get { return this.lesCommandes; }
            set { this.lesCommandes = value; }
        }

        public ObservableCollection<Client> LesClients
        {
            get { return this.lesClients; }
            set { this.lesClients = value; }
        }

        public ObservableCollection<Plat> LesPlats
        {
            get { return this.lesPlats; }
            set { this.lesPlats = value; }
        }

        public List<Employe> LesEmploye
        {
            get { return this.lesEmploye; }
            set { this.lesEmploye = value; }
        }

        public List<Categorie> LesCategories
        {
            get { return this.lesCategories; }
            set { this.lesCategories = value; }
        }

        public List<Periode> LesPeriodes
        {
            get { return this.lesPeriodes; }
            set { this.lesPeriodes = value; }
        }

        public List<Role> LesRoles
        {
            get { return this.lesRoles; }
            set { this.lesRoles = value; }
        }

        public List<SousCategorie> LesSousCategories
        {
            get { return this.lesSousCategories; }
            set { this.lesSousCategories = value; }
        }

        public ObservableCollection<Contient> LesContients
        {
            get { return this.lesContients; }
            set { this.lesContients = value; }
        }

        public void RechargerDonnees()
        {
            try
            {
                string nomActuel = this.Nom;
                var nouvellGestion = new GestionCommande(nomActuel);

                this.LesClients = nouvellGestion.LesClients;
                this.LesCommandes = nouvellGestion.LesCommandes;
                this.LesPlats = nouvellGestion.LesPlats;
                this.LesContients = nouvellGestion.LesContients;
                this.LesEmploye = nouvellGestion.LesEmploye;
                this.LesCategories = nouvellGestion.LesCategories;
                this.LesPeriodes = nouvellGestion.LesPeriodes;
                this.LesRoles = nouvellGestion.LesRoles;
                this.LesSousCategories = nouvellGestion.LesSousCategories;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AjouterCommande(Commande nouvelleCommande)
        {
            if (nouvelleCommande != null)
            {
                if (nouvelleCommande.UnClient != null)
                {
                    var clientComplet = LesClients.FirstOrDefault(c => c.NumClient == nouvelleCommande.UnClient.NumClient);
                    if (clientComplet != null)
                        nouvelleCommande.UnClient = clientComplet;
                }

                if (nouvelleCommande.UnEmploye != null)
                {
                    var employeComplet = LesEmploye.FirstOrDefault(e => e.NumEmploye == nouvelleCommande.UnEmploye.NumEmploye);
                    if (employeComplet != null)
                        nouvelleCommande.UnEmploye = employeComplet;
                }

                LesCommandes.Add(nouvelleCommande);
            }
        }

        public void SupprimerCommande(Commande commande)
        {
            if (commande != null && LesCommandes.Contains(commande))
            {
                var contentsASupprimer = LesContients.Where(c => c.UneCommande.NumCommande == commande.NumCommande).ToList();
                foreach (var contient in contentsASupprimer)
                {
                    LesContients.Remove(contient);
                }

                LesCommandes.Remove(commande);
            }
        }

        public override string ToString()
        {
            return $"GestionCommande: {Nom} - {LesCommandes?.Count ?? 0} commandes";
        }
    }
}