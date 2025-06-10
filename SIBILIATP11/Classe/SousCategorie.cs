using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class SousCategorie
    {
        private int numSousCategorie;
        private string nomSousCategorie;
        private Categorie uneCategorie;

        public SousCategorie(int numSousCategorie, string nomSousCategorie, Categorie uneCategorie)
        {
            this.NumSousCategorie = numSousCategorie;
            this.NomSousCategorie = nomSousCategorie;
            this.UneCategorie = uneCategorie;
        }

        public int NumSousCategorie
        {
            get
            {
                return this.numSousCategorie;
            }

            set
            {
                this.numSousCategorie = value;
            }
        }

        public string NomSousCategorie
        {
            get
            {
                return this.nomSousCategorie;
            }

            set
            {
                this.nomSousCategorie = value;
            }
        }

        public Categorie UneCategorie
        {
            get
            {
                return this.uneCategorie;
            }

            set
            {
                this.uneCategorie = value;
            }
        }
    }
}
