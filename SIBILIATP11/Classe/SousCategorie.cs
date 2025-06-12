using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIBILIATP11.Model;
using System.ComponentModel;

namespace SIBILIATP11.Classe
{
    public class SousCategorie : ICrud<SousCategorie>, INotifyPropertyChanged
    {
        private int numSousCategorie;
        private string nomSousCategorie;
        private Categorie uneCategorie;

        public SousCategorie()
        {
        }

        public SousCategorie(int numSousCategorie)
        {
            this.NumSousCategorie = numSousCategorie;
        }

        public SousCategorie(int numSousCategorie, string nomSousCategorie, Categorie uneCategorie)
        {
            this.NumSousCategorie = numSousCategorie;
            this.NomSousCategorie = nomSousCategorie;
            this.UneCategorie = uneCategorie;
        }

        public int NumSousCategorie
        {
            get { return this.numSousCategorie; }
            set
            {
                this.numSousCategorie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumSousCategorie)));
            }
        }

        public string NomSousCategorie
        {
            get { return this.nomSousCategorie; }
            set
            {
                this.nomSousCategorie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomSousCategorie)));
            }
        }

        public Categorie UneCategorie
        {
            get { return this.uneCategorie; }
            set
            {
                this.uneCategorie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UneCategorie)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO souscategorie (nomsouscategorie, numcategorie) VALUES (@nomSousCategorie, @numCategorie) RETURNING numsouscategorie"))
            {
                cmdInsert.Parameters.AddWithValue("@nomSousCategorie", this.NomSousCategorie);
                cmdInsert.Parameters.AddWithValue("@numCategorie", this.UneCategorie.NumCategorie);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumSousCategorie = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM souscategorie WHERE numsouscategorie = @numSousCategorie"))
            {
                cmdSelect.Parameters.AddWithValue("@numSousCategorie", this.NumSousCategorie);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomSousCategorie = dt.Rows[0]["nomsouscategorie"]?.ToString() ?? "";
                    this.UneCategorie = new Categorie((Int32)dt.Rows[0]["numcategorie"]);
                    this.UneCategorie.Read();
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE souscategorie SET nomsouscategorie = @nomSousCategorie, numcategorie = @numCategorie WHERE numsouscategorie = @numSousCategorie"))
            {
                cmdUpdate.Parameters.AddWithValue("@nomSousCategorie", this.NomSousCategorie);
                cmdUpdate.Parameters.AddWithValue("@numCategorie", this.UneCategorie.NumCategorie);
                cmdUpdate.Parameters.AddWithValue("@numSousCategorie", this.NumSousCategorie);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("DELETE FROM souscategorie WHERE numsouscategorie = @numSousCategorie"))
            {
                cmdDelete.Parameters.AddWithValue("@numSousCategorie", this.NumSousCategorie);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<SousCategorie> FindAll()
        {
            List<SousCategorie> lesSousCategories = new List<SousCategorie>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM souscategorie"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesSousCategories.Add(new SousCategorie(
                        (Int32)dr["numsouscategorie"],
                        dr["nomsouscategorie"]?.ToString() ?? "",
                        new Categorie((Int32)dr["numcategorie"])
                    ));
                }
            }
            return lesSousCategories;
        }

        public List<SousCategorie> FindBySelection(string criteres)
        {
            List<SousCategorie> lesSousCategories = new List<SousCategorie>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM souscategorie WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesSousCategories.Add(new SousCategorie(
                        (Int32)dr["numsouscategorie"],
                        dr["nomsouscategorie"]?.ToString() ?? "",
                        new Categorie((Int32)dr["numcategorie"])
                    ));
                }
            }
            return lesSousCategories;
        }
    }
}