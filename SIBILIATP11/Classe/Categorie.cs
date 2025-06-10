using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class Categorie
    {
        private int numCategorie; 
        private string nomCategorie;

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
    }
}
