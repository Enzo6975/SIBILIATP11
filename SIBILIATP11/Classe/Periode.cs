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
    public class Periode
    {
        private int numPeriode;
        private string libellePeriode;

        public Periode() { }

        public Periode(int numPeriode)
        {
            this.NumPeriode = numPeriode;
        }

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

        public List<Periode> FindAll()
        {
            List<Periode> lesPeriodes = new List<Periode>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from periode ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesPeriodes.Add(new Periode((Int32)dr["numperiode"], (String)dr["nomperiode"]));
            }
            return lesPeriodes;
        }
    }
}
