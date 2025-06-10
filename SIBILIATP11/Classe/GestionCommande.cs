using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    class GestionCommande
    {
        private string nom;
        private ObservableCollection<Commande> lesCommandes;
        private ObservableCollection<Client> lesClients;
        private ObservableCollection<Plat> lesPlats;

        public GestionCommande(string nom)
        {
            this.Nom = nom;
            this.LesClients = new ObservableCollection<Client>(new Client().FindAll());
            this.LesCommandes = new ObservableCollection<Commande>(new Commande().FindAll());
            this.LesPlats = new ObservableCollection<Plat>(new Plat().FindAll());
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
