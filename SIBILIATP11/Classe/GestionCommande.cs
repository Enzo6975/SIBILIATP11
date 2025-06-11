using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIBILIATP11.Classe
{
    class GestionCommande
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
            this.LesClients = new ObservableCollection<Client>(new Client().FindAll());
            this.LesEmploye = new List<Employe>(new Employe().FindAll());
            this.LesCategories = new List<Categorie>(new Categorie().FindAll());
            this.LesPeriodes = new List<Periode>(new Periode().FindAll());
            this.LesRoles = new List<Role>(new Role().FindAll());
            this.LesSousCategories = new List<SousCategorie>(new SousCategorie().FindAll());

            ObservableCollection<Commande> commandes = new ObservableCollection<Commande>(new Commande().FindAll());
            foreach (Commande commande in commandes)
            {
                Client client = LesClients.SingleOrDefault(c => c.NumClient == commande.UnClient.NumClient);
                Employe employe = LesEmploye.SingleOrDefault(b => b.NumEmploye == commande.UnEmploye.NumEmploye);

                commande.UnClient = client;
                commande.UnEmploye = employe;
            }
            this.LesCommandes = commandes;

            ObservableCollection<Plat> plats = new ObservableCollection<Plat>(new Plat().FindAll());
            foreach (Plat plat in plats)
            {
                SousCategorie souscategorie = LesSousCategories.SingleOrDefault(c => c.NumSousCategorie == plat.UneSousCategorie.NumSousCategorie);
                Periode periode = LesPeriodes.SingleOrDefault(b => b.NumPeriode == plat.UnePeriode.NumPeriode);

                plat.UneSousCategorie = souscategorie;
                plat.UnePeriode = periode;
            }
            this.LesPlats = plats;

            ObservableCollection<Contient> contients = new ObservableCollection<Contient>(new Contient().FindAll());
            foreach (Contient contient in contients)
            {
                Commande commande = LesCommandes.SingleOrDefault(c => c.NumCommande == contient.UneCommande.NumCommande);
                Plat plat = LesPlats.SingleOrDefault(b => b.NumPlat == contient.UnPlat.NumPlat);

                contient.UneCommande = commande;
                contient.UnPlat = plat;
            }
            this.LesPlats = plats;
        }

        public string Nom
        {
            get
            {
                return this.nom;
            }

            set
            {
                this.nom = value;
            }
        }

        public ObservableCollection<Commande> LesCommandes
        {
            get
            {
                return this.lesCommandes;
            }

            set
            {
                this.lesCommandes = value;
            }
        }

        public ObservableCollection<Client> LesClients
        {
            get
            {
                return this.lesClients;
            }

            set
            {
                this.lesClients = value;
            }
        }

        public ObservableCollection<Plat> LesPlats
        {
            get
            {
                return this.lesPlats;
            }

            set
            {
                this.lesPlats = value;
            }
        }

        public List<Employe> LesEmploye
        {
            get
            {
                return this.lesEmploye;
            }

            set
            {
                this.lesEmploye = value;
            }
        }

        public List<Categorie> LesCategories
        {
            get
            {
                return this.lesCategories;
            }

            set
            {
                this.lesCategories = value;
            }
        }

        public List<Periode> LesPeriodes
        {
            get
            {
                return this.lesPeriodes;
            }

            set
            {
                this.lesPeriodes = value;
            }
        }

        public List<Role> LesRoles
        {
            get
            {
                return this.lesRoles;
            }

            set
            {
                this.lesRoles = value;
            }
        }

        public List<SousCategorie> LesSousCategories
        {
            get
            {
                return this.lesSousCategories;
            }

            set
            {
                this.lesSousCategories = value;
            }
        }

        public ObservableCollection<Contient> LesContients
        {
            get
            {
                return this.lesContients;
            }

            set
            {
                this.lesContients = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
