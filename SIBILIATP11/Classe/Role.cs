using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class Role
    {
        private int numRole;
        private string nomRole;

        public Role(int numRole, string nomRole)
        {
            this.NumRole = numRole;
            this.NomRole = nomRole;
        }

        public int NumRole
        {
            get
            {
                return this.numRole;
            }

            set
            {
                this.numRole = value;
            }
        }

        public string NomRole
        {
            get
            {
                return this.nomRole;
            }

            set
            {
                this.nomRole = value;
            }
        }
    }
}
