using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SIBILIATP11.Classe
{
    public class Categorie
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

    }
}
