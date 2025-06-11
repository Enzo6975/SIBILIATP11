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
    public class Employe
    {
        private int numEmploye;
        private string nomEmploye;
        private string prenomEmploye;
        private string password;
        private string login;
        private Role unRole;

        public Employe()
        {

        }

        public Employe(int numEmploye)
        {
            this.NumEmploye = numEmploye;
        }

        public Employe(int numEmploye, string nomEmploye, string prenomEmploye, string password, string login, Role unRole)
        {
            this.NumEmploye = numEmploye;
            this.NomEmploye = nomEmploye;
            this.PrenomEmploye = prenomEmploye;
            this.Password = password;
            this.Login = login;
            this.UnRole = unRole;
        }

        public int NumEmploye
        {
            get
            {
                return this.numEmploye;
            }

            set
            {
                this.numEmploye = value;
            }
        }

        public string NomEmploye
        {
            get
            {
                return this.nomEmploye;
            }

            set
            {
                this.nomEmploye = value;
            }
        }

        public string PrenomEmploye
        {
            get
            {
                return this.prenomEmploye;
            }

            set
            {
                this.prenomEmploye = value;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;
            }
        }

        public string Login
        {
            get
            {
                return this.login;
            }

            set
            {
                this.login = value;
            }
        }

        public Role UnRole
        {
            get
            {
                return this.unRole;
            }

            set
            {
                this.unRole = value;
            }
        }

        public List<Employe> FindAll()
        {
            List<Employe> lesEmployes = new List<Employe>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from employe ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesEmployes.Add(new Employe((Int32)dr["numemploye"], (String)dr["nomemploye"], (String)dr["prenomemploye"], (String)dr["password"], (String)dr["login"], new Role((Int32)dr["numrole"])));
            }
            return lesEmployes;
        }
    }
}
