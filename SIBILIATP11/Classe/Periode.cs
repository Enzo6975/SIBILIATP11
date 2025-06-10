using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class Periode
    {
        private int numPeriode;
        private string libellePeriode;

        public Periode(int numPeriode, string libellePeriode)
        {
            this.NumPeriode = numPeriode;
            this.LibellePeriode = libellePeriode;
        }

        public int NumPeriode
        {
            get
            {
                return this.numPeriode;
            }

            set
            {
                this.numPeriode = value;
            }
        }

        public string LibellePeriode
        {
            get
            {
                return this.libellePeriode;
            }

            set
            {
                this.libellePeriode = value;
            }
        }
    }
}
