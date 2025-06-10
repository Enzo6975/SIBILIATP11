using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class Contient
    {
        private int quantite;
        private double prix; 
        private Commande uneCommande;
        private Plat unPlat;

        public Contient(int quantite, double prix, Commande uneCommande, Plat unPlat)
        {
            this.Quantite = quantite;
            this.Prix = prix;
            this.UneCommande = uneCommande;
            this.UnPlat = unPlat;
        }

        public int Quantite
        {
            get
            {
                return this.quantite;
            }

            set
            {
                this.quantite = value;
            }
        }

        public double Prix
        {
            get
            {
                return this.prix;
            }

            set
            {
                this.prix = value;
            }
        }

        public Commande UneCommande
        {
            get
            {
                return this.uneCommande;
            }

            set
            {
                this.uneCommande = value;
            }
        }

        public Plat UnPlat
        {
            get
            {
                return this.unPlat;
            }

            set
            {
                this.unPlat = value;
            }
        }
    }
}
