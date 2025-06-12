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
    public class Periode : ICrud<Periode>, INotifyPropertyChanged
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
                    lesPeriodes.Add(new Periode((Int32)dr["numperiode"], (String)dr["libelleperiode"]));
            }
            return lesPeriodes;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO periode (libelleperiode) VALUES (@libellePeriode) RETURNING numperiode"))
            {
                cmdInsert.Parameters.AddWithValue("@libellePeriode", this.LibellePeriode);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumPeriode = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM periode WHERE numperiode = @numPeriode"))
            {
                cmdSelect.Parameters.AddWithValue("@numPeriode", this.NumPeriode);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.LibellePeriode = (String)dt.Rows[0]["libelleperiode"];
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE periode SET libelleperiode = @libellePeriode WHERE numperiode = @numPeriode"))
            {
                cmdUpdate.Parameters.AddWithValue("@libellePeriode", this.LibellePeriode);
                cmdUpdate.Parameters.AddWithValue("@numPeriode", this.NumPeriode);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("DELETE FROM periode WHERE numPeriode = @numPeriode"))
            {
                cmdDelete.Parameters.AddWithValue("@numPeriode", this.NumPeriode);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Periode> FindBySelection(string criteres)
        {
            List<Periode> lesPeriodes = new List<Periode>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM periode WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesPeriodes.Add(new Periode((Int32)dr["numperiode"], (String)dr["libelleperiode"]));
            }
            return lesPeriodes;
        }
    }
}
