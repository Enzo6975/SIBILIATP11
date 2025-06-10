using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class Commande
    {
        private int numCommande;
        private DateTime dateCommande;
        private DateTime dateRetraitPrevue;
        private bool payee;
        private bool retiree;
        private double prixTotal;
        private Employe unEmploye;
        private Client unClient;

        public Commande(int numCommande, DateTime dateCommande, DateTime dateRetraitPrevue, bool payee, bool retiree, double prixTotal, Employe unEmploye, Client unClient)
        {
            this.NumCommande = numCommande;
            this.DateCommande = dateCommande;
            this.DateRetraitPrevue = dateRetraitPrevue;
            this.Payee = payee;
            this.Retiree = retiree;
            this.PrixTotal = prixTotal;
            this.UnEmploye = unEmploye;
            this.UnClient = unClient;
        }

        public int NumCommande
        {
            get
            {
                return this.numCommande;
            }

            set
            {
                this.numCommande = value;
            }
        }

        public DateTime DateCommande
        {
            get
            {
                return this.dateCommande;
            }

            set
            {
                this.dateCommande = value;
            }
        }

        public DateTime DateRetraitPrevue
        {
            get
            {
                return this.dateRetraitPrevue;
            }

            set
            {
                this.dateRetraitPrevue = value;
            }
        }

        public bool Payee
        {
            get
            {
                return this.payee;
            }

            set
            {
                this.payee = value;
            }
        }

        public bool Retiree
        {
            get
            {
                return this.retiree;
            }

            set
            {
                this.retiree = value;
            }
        }

        public double PrixTotal
        {
            get
            {
                return this.prixTotal;
            }

            set
            {
                this.prixTotal = value;
            }
        }

        public Employe UnEmploye
        {
            get
            {
                return this.unEmploye;
            }

            set
            {
                this.unEmploye = value;
            }
        }

        public Client UnClient
        {
            get
            {
                return this.unClient;
            }

            set
            {
                this.unClient = value;
            }
        }
    }
}
