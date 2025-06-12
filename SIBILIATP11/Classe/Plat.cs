using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIBILIATP11.Model;

namespace SIBILIATP11.Classe
{
    public class Plat : ICrud<Plat>, INotifyPropertyChanged
    {
        private int numPlat;
        private string nomPlat;
        private double prixUnitaire;
        private int delaiPreparation;
        private int nbPersonnes;
        private SousCategorie uneSousCategorie;
        private Periode unePeriode;

        public Plat()
        {
        }

        public Plat(int numPlat)
        {
            this.NumPlat = numPlat;
        }

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
            get { return this.numPlat; }
            set { this.numPlat = value; }
        }

        public string NomPlat
        {
            get { return this.nomPlat; }
            set
            {
                this.nomPlat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomPlat)));
            }
        }

        public double PrixUnitaire
        {
            get { return this.prixUnitaire; }
            set
            {
                this.prixUnitaire = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrixUnitaire)));
            }
        }

        public int DelaiPreparation
        {
            get { return this.delaiPreparation; }
            set
            {
                this.delaiPreparation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DelaiPreparation)));
            }
        }

        public int NbPersonnes
        {
            get { return this.nbPersonnes; }
            set
            {
                this.nbPersonnes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NbPersonnes)));
            }
        }

        public SousCategorie UneSousCategorie
        {
            get { return this.uneSousCategorie; }
            set
            {
                this.uneSousCategorie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UneSousCategorie)));
            }
        }

        public Periode UnePeriode
        {
            get { return this.unePeriode; }
            set
            {
                this.unePeriode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnePeriode)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO plat (nomplat, prixunitaire, delaipreparation, nbpersonnes, numsouscategorie, numperiode) VALUES (@nomPlat, @prixUnitaire, @delaiPreparation, @nbPersonnes, @numSousCategorie, @numPeriode) RETURNING numplat"))
            {
                cmdInsert.Parameters.AddWithValue("@nomPlat", this.NomPlat);
                cmdInsert.Parameters.AddWithValue("@prixUnitaire", this.PrixUnitaire);
                cmdInsert.Parameters.AddWithValue("@delaiPreparation", this.DelaiPreparation);
                cmdInsert.Parameters.AddWithValue("@nbPersonnes", this.NbPersonnes);
                cmdInsert.Parameters.AddWithValue("@numSousCategorie", this.UneSousCategorie.NumSousCategorie);
                cmdInsert.Parameters.AddWithValue("@numPeriode", this.UnePeriode.NumPeriode);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumPlat = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM plat WHERE numplat = @numPlat"))
            {
                cmdSelect.Parameters.AddWithValue("@numPlat", this.NumPlat);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomPlat = dt.Rows[0]["nomplat"]?.ToString() ?? "";
                    this.PrixUnitaire = Convert.ToDouble(dt.Rows[0]["prixunitaire"]);
                    this.DelaiPreparation = (Int32)dt.Rows[0]["delaipreparation"];
                    this.NbPersonnes = (Int32)dt.Rows[0]["nbpersonnes"];
                    this.UneSousCategorie = new SousCategorie((Int32)dt.Rows[0]["numsouscategorie"]);
                    this.UnePeriode = new Periode((Int32)dt.Rows[0]["numperiode"]);
                    this.UneSousCategorie.Read();
                    this.UnePeriode.Read();
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE plat SET nomplat = @nomPlat, prixunitaire = @prixUnitaire, delaipreparation = @delaiPreparation, nbpersonnes = @nbPersonnes, numsouscategorie = @numSousCategorie, numperiode = @numPeriode WHERE numplat = @numPlat"))
            {
                cmdUpdate.Parameters.AddWithValue("@nomPlat", this.NomPlat);
                cmdUpdate.Parameters.AddWithValue("@prixUnitaire", this.PrixUnitaire);
                cmdUpdate.Parameters.AddWithValue("@delaiPreparation", this.DelaiPreparation);
                cmdUpdate.Parameters.AddWithValue("@nbPersonnes", this.NbPersonnes);
                cmdUpdate.Parameters.AddWithValue("@numSousCategorie", this.UneSousCategorie.NumSousCategorie);
                cmdUpdate.Parameters.AddWithValue("@numPeriode", this.UnePeriode.NumPeriode);
                cmdUpdate.Parameters.AddWithValue("@numPlat", this.NumPlat);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("DELETE FROM plat WHERE numplat = @numPlat"))
            {
                cmdDelete.Parameters.AddWithValue("@numPlat", this.NumPlat);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Plat> FindAll()
        {
            List<Plat> lesPlats = new List<Plat>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM plat"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesPlats.Add(new Plat(
                        (Int32)dr["numplat"],
                        dr["nomplat"]?.ToString() ?? "",
                        Convert.ToDouble(dr["prixunitaire"]),
                        (Int32)dr["delaipreparation"],
                        (Int32)dr["nbpersonnes"],
                        new SousCategorie((Int32)dr["numsouscategorie"]),
                        new Periode((Int32)dr["numperiode"])
                    ));
                }
            }
            return lesPlats;
        }

        public List<Plat> FindBySelection(string criteres)
        {
            List<Plat> lesPlats = new List<Plat>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM plat WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesPlats.Add(new Plat(
                        (Int32)dr["numplat"],
                        dr["nomplat"]?.ToString() ?? "",
                        Convert.ToDouble(dr["prixunitaire"]),
                        (Int32)dr["delaipreparation"],
                        (Int32)dr["nbpersonnes"],
                        new SousCategorie((Int32)dr["numsouscategorie"]),
                        new Periode((Int32)dr["numperiode"])
                    ));
                }
            }
            return lesPlats;
        }
    }
}