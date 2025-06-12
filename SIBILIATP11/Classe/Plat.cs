using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        public Plat(int numPlat)
        {
            this.NumPlat = numPlat;
        }

        public Plat(int numPlat, string nomPlat, double prixUnitaire, int delaiPreparation, int nbPersonnes)
        {
            this.NumPlat = numPlat;
            this.NomPlat = nomPlat;
            this.PrixUnitaire = prixUnitaire;
            this.DelaiPreparation = delaiPreparation;
            this.NbPersonnes = nbPersonnes;
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

        // using System.Diagnostics; // Utile pour logger l'erreur pour le développeur

        public List<Plat> FindAll()
        {
            // La liste est retournée uniquement en cas de succès.
            try
            {
                List<Plat> lesPlats = new List<Plat>();
                using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from plat"))
                {
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        lesPlats.Add(new Plat(
                            (Int32)dr["numplat"],
                            (String)dr["nomplat"],
                            Double.Parse(dr["prixunitaire"].ToString()),
                            (Int32)dr["delaipreparation"],
                            (Int32)dr["nbpersonnes"]
                        ));
                    }
                    return lesPlats;
                }
            }
            catch (Exception ex)
            {
                // Option 1 : Log l'erreur pour le débogage (recommandé)
                // Ceci écrit dans la fenêtre "Sortie" de Visual Studio.
                Debug.WriteLine("ERREUR DAL: " + ex.Message);

                // Option 2 : Relancer l'exception pour que la couche UI puisse la gérer.
                // On encapsule l'exception originale pour ne pas perdre d'information.
                throw new Exception("Impossible de charger la liste des plats depuis la base de données.", ex);
            }
        }

        public List<Plat> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from  plat where numplat =@numplat;"))
            {
                cmdSelect.Parameters.AddWithValue("numplat", this.numPlat);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                this.NumPlat = (Int32)dt.Rows[0]["numplat"];
                this.NomPlat = (String)dt.Rows[0]["nomplat"];
                this.PrixUnitaire = (double)dt.Rows[0]["poids"];
                this.DelaiPreparation = (Int32)dt.Rows[0]["delaipreparation"];
                this.NbPersonnes = (Int32)dt.Rows[0]["nbpersonnes"];

            }

        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
