using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SIBILIATP11.Classe
{
    public class Plat: ICrud<Plat>, INotifyPropertyChanged
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public List<Plat> FindAll()
        {
            List<Plat> lesPlats = new List<Plat>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from plat ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesPlats.Add(new Plat((Int32)dr["numplat"], (String)dr["nomplat"], (Double)dr["prixunitaire"], (Int32)dr["delaipreparation"], (Int32)dr["nbpersonnes"], (SousCategorie)dr["numsouscategorie"], (Periode)dr["numperiode"]));
            }
            return lesPlats;
        }

        public List<Plat> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
