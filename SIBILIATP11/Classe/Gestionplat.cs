using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIBILIATP11.Model;

namespace SIBILIATP11.Classe
{
    public class Gestionplat
    {
        private readonly DataAccess dataAccess;

        public Gestionplat()
        {
            dataAccess = DataAccess.Instance;
        }

        public List<Plat> FindAll()
        {
            List<Plat> plats = new List<Plat>();
            string query = "SELECT NumPlat, NomPlat, PrixUnitaire, DelaiPreparation, NbPersonnes FROM Plat ORDER BY NumPlat";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query))
                {
                    DataTable dataTable = dataAccess.ExecuteSelect(cmd);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Plat plat = new Plat
                        {
                            NumPlat = (int)row["NumPlat"],
                            NomPlat = (string)row["NomPlat"],
                            PrixUnitaire = (double)Convert.ToDecimal(row["PrixUnitaire"]),
                            DelaiPreparation = (int)row["DelaiPreparation"],
                            NbPersonnes = (int)row["NbPersonnes"]
                        };
                        plats.Add(plat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return plats;
        }
    } 
}
