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
    public class Role : ICrud<Role>, INotifyPropertyChanged
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
            get { return this.numRole; }
            set
            {
                this.numRole = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumRole)));
            }
        }

        public string NomRole
        {
            get { return this.nomRole; }
            set
            {
                this.nomRole = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomRole)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO role (nomrole) VALUES (@nomRole) RETURNING numrole"))
            {
                cmdInsert.Parameters.AddWithValue("@nomRole", this.NomRole);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumRole = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM role WHERE numrole = @numRole"))
            {
                cmdSelect.Parameters.AddWithValue("@numRole", this.NumRole);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomRole = dt.Rows[0]["nomrole"]?.ToString() ?? "";
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE role SET nomrole = @nomRole WHERE numrole = @numRole"))
            {
                cmdUpdate.Parameters.AddWithValue("@nomRole", this.NomRole);
                cmdUpdate.Parameters.AddWithValue("@numRole", this.NumRole);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("DELETE FROM role WHERE numrole = @numRole"))
            {
                cmdDelete.Parameters.AddWithValue("@numRole", this.NumRole);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Role> FindAll()
        {
            List<Role> lesRoles = new List<Role>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM role"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesRoles.Add(new Role(
                        (Int32)dr["numrole"],
                        dr["nomrole"]?.ToString() ?? ""
                    ));
                }
            }
            return lesRoles;
        }

        public List<Role> FindBySelection(string criteres)
        {
            List<Role> lesRoles = new List<Role>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM role WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesRoles.Add(new Role(
                        (Int32)dr["numrole"],
                        dr["nomrole"]?.ToString() ?? ""
                    ));
                }
            }
            return lesRoles;
        }
    }
}
