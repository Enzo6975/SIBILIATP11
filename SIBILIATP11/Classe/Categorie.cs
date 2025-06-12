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
    public class Categorie : ICrud<Categorie>, INotifyPropertyChanged
    {
        private int numCategorie; 
        private string nomCategorie;
        public Categorie() { }

        public Categorie(int numCategorie)
        {
            this.NumCategorie = numCategorie;
        }
        public Categorie(int numCategorie, string nomCategorie)
        {
            this.NumCategorie = numCategorie;
            this.NomCategorie = nomCategorie;
        }

        public int NumCategorie
        {
            get
            {
                return this.numCategorie;
            }

            set
            {
                this.numCategorie = value;
            }
        }

        public string NomCategorie
        {
            get
            {
                return this.nomCategorie;
            }

            set
            {
                value = char.ToUpper(value[0]) + value.Substring(1).ToLower();
                this.nomCategorie = value;
            }
        }
        public List<Categorie> FindAll()
        {
            List<Categorie> lesCategories = new List<Categorie>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from categorie ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesCategories.Add(new Categorie((Int32)dr["numcategorie"], (String)dr["nomcategorie"]));
            }
            return lesCategories;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO categorie (nomcategorie) VALUES (@nomCategorie) RETURNING numcategorie"))
            {
                cmdInsert.Parameters.AddWithValue("@nomCategorie", this.NomCategorie);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumCategorie = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM categorie WHERE numcategorie = @numCategorie"))
            {
                cmdSelect.Parameters.AddWithValue("@numCategorie", this.NumCategorie);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomCategorie = (String)dt.Rows[0]["nomcategorie"];
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE categorie SET nomcategorie = @nomCategorie WHERE numcategorie = @numCategorie"))
            {
                cmdUpdate.Parameters.AddWithValue("@nomCategorie", this.NomCategorie);
                cmdUpdate.Parameters.AddWithValue("@numCategorie", this.NumCategorie);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("DELETE FROM categorie WHERE numcategorie = @numCategorie"))
            {
                cmdDelete.Parameters.AddWithValue("@numCategorie", this.NumCategorie);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Categorie> FindBySelection(string criteres)
        {
            List<Categorie> lesCategories = new List<Categorie>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM categorie WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesCategories.Add(new Categorie((Int32)dr["numcategorie"], (String)dr["nomcategorie"]));
            }
            return lesCategories;
        }
    }
}
