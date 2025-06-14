﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIBILIATP11.Model;
using System.ComponentModel;

namespace SIBILIATP11.Classe
{
    public class Contient : ICrud<Contient>, INotifyPropertyChanged
    {
        private int quantite;
        private double prix;
        private Commande uneCommande;
        private Plat unPlat;

        public Contient()
        {
        }

        public Contient(int quantite, double prix, Commande uneCommande, Plat unPlat)
        {
            this.Quantite = quantite;
            this.Prix = prix;
            this.UneCommande = uneCommande;
            this.UnPlat = unPlat;
        }

        public int Quantite
        {
            get { return this.quantite; }
            set
            {
                if (value >= 1 && value <= 100) // Validation de la plage
                {
                    this.quantite = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantite)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculerTotal)));
                    
                    // Mettre à jour automatiquement le prix basé sur le prix unitaire du plat
                    if (UnPlat != null)
                    {
                        this.Prix = UnPlat.PrixUnitaire;
                    }
                }
            }
        }

        public double Prix
        {
            get { return this.prix; }
            set
            {
                this.prix = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Prix)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculerTotal)));
            }
        }

        public Commande UneCommande
        {
            get { return this.uneCommande; }
            set
            {
                this.uneCommande = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UneCommande)));
            }
        }

        public Plat UnPlat
        {
            get { return this.unPlat; }
            set
            {
                this.unPlat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnPlat)));
                
                // Mettre à jour automatiquement le prix quand le plat change
                if (value != null)
                {
                    this.Prix = value.PrixUnitaire;
                }
            }
        }

        public double CalculerTotal
        {
            get { return this.Quantite * this.Prix; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO platcommande (quantite, prix, numcommande, numplat) VALUES (@quantite, @prix, @numCommande, @numPlat)"))
            {
                cmdInsert.Parameters.AddWithValue("@quantite", this.Quantite);
                cmdInsert.Parameters.AddWithValue("@prix", this.Prix);
                cmdInsert.Parameters.AddWithValue("@numCommande", this.UneCommande.NumCommande);
                cmdInsert.Parameters.AddWithValue("@numPlat", this.UnPlat.NumPlat);
                return DataAccess.Instance.ExecuteSet(cmdInsert);
            }
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM platcommande WHERE numcommande = @numCommande AND numplat = @numPlat"))
            {
                cmdSelect.Parameters.AddWithValue("@numCommande", this.UneCommande.NumCommande);
                cmdSelect.Parameters.AddWithValue("@numPlat", this.UnPlat.NumPlat);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.Quantite = (Int32)dt.Rows[0]["quantite"];
                    this.Prix = Convert.ToDouble(dt.Rows[0]["prix"]);
                    this.UneCommande.Read();
                    this.UnPlat.Read();
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE platcommande SET quantite = @quantite, prix = @prix WHERE numcommande = @numCommande AND numplat = @numPlat"))
            {
                cmdUpdate.Parameters.AddWithValue("@quantite", this.Quantite);
                cmdUpdate.Parameters.AddWithValue("@prix", this.Prix);
                cmdUpdate.Parameters.AddWithValue("@numCommande", this.UneCommande.NumCommande);
                cmdUpdate.Parameters.AddWithValue("@numPlat", this.UnPlat.NumPlat);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("DELETE FROM platcommande WHERE numcommande = @numCommande AND numplat = @numPlat"))
            {
                cmdDelete.Parameters.AddWithValue("@numCommande", this.UneCommande.NumCommande);
                cmdDelete.Parameters.AddWithValue("@numPlat", this.UnPlat.NumPlat);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Contient> FindAll()
        {
            List<Contient> lesContients = new List<Contient>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM platcommande"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesContients.Add(new Contient(
                        (Int32)dr["quantite"],
                        Convert.ToDouble(dr["prix"]),
                        new Commande((Int32)dr["numcommande"]),
                        new Plat((Int32)dr["numplat"])
                    ));
                }
            }
            return lesContients;
        }

        public List<Contient> FindBySelection(string criteres)
        {
            List<Contient> lesContients = new List<Contient>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM platcommande WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesContients.Add(new Contient(
                        (Int32)dr["quantite"],
                        Convert.ToDouble(dr["prix"]),
                        new Commande((Int32)dr["numcommande"]),
                        new Plat((Int32)dr["numplat"])
                    ));
                }
            }
            return lesContients;
        }

        public List<Contient> FindByCommande(int numCommande)
        {
            return FindBySelection($"numcommande = {numCommande}");
        }

        public List<Contient> FindByPlat(int numPlat)
        {
            return FindBySelection($"numplat = {numPlat}");
        }
    }
}