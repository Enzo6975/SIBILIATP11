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
    public class Role
    {
        private int numRole;
        private string nomRole;

        public Role() { }

        public Role(int numRole)
        {
            this.numRole = numRole;
        }
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

        public List<Role> FindAll()
        {
            List<Role> lesRoles = new List<Role>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from role ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesRoles.Add(new Role((Int32)dr["numrole"], (String)dr["nomrole"]));
            }
            return lesRoles;
        }
    }
}
