using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SIBILIATP11.Classe
{
    public class Contient
    {
        private int quantite;
        private double prix; 
        private Commande uneCommande;
        private Plat unPlat;

        public Contient() { }
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

        public List<Contient> FindAll()
        {
            List<Contient> lesContients = new List<Contient>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from platcommande ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesContients.Add(new Contient((Int32)dr["quantite"], (Int32)dr["prix"], (Commande)dr["numcommande"], (Plat)dr["numplat"]));
            }
            return lesContients;
        }
    }
}
