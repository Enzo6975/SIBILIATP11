using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class Plat
    {
        private int numPlat;
        private string nomPlat;
        private double prixUnitaire;
        private int delaiPreparation;
        private int nbPersonnes;
        private SousCategorie uneSousCategorie;
        private Periode unePeriode;

        public Plat(int numPlat, string nomPlat, double prixUnitaire, int delaiPreparation, int nbPersonnes, SousCategorie uneSousCategorie, Periode unePeriode)
        {
            this.NumPlat = numPlat;
            this.NomPlat = nomPlat;
            this.PrixUnitaire = prixUnitaire;
            this.DelaiPreparation = delaiPreparation;
            this.NbPersonnes = nbPersonnes;
            this.UneSousCategorie = uneSousCategorie;
            this.UnePeriode = unePeriode;
        }

        public int NumPlat
        {
            get
            {
                return this.numPlat;
            }

            set
            {
                this.numPlat = value;
            }
        }

        public string NomPlat
        {
            get
            {
                return this.nomPlat;
            }

            set
            {
                this.nomPlat = value;
            }
        }

        public double PrixUnitaire
        {
            get
            {
                return this.prixUnitaire;
            }

            set
            {
                this.prixUnitaire = value;
            }
        }

        public int DelaiPreparation
        {
            get
            {
                return this.delaiPreparation;
            }

            set
            {
                this.delaiPreparation = value;
            }
        }

        public int NbPersonnes
        {
            get
            {
                return this.nbPersonnes;
            }

            set
            {
                this.nbPersonnes = value;
            }
        }

        public SousCategorie UneSousCategorie
        {
            get
            {
                return this.uneSousCategorie;
            }

            set
            {
                this.uneSousCategorie = value;
            }
        }

        public Periode UnePeriode
        {
            get
            {
                return this.unePeriode;
            }

            set
            {
                this.unePeriode = value;
            }
        }
    }
}
