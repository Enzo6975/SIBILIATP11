using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
