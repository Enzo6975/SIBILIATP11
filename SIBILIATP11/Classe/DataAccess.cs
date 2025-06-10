

using System.Collections.Generic;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Logging;
using Npgsql;
using SIBILIATP11.Classe;


namespace TD3_BindingBDPension.Model
{

    public  class DataAccess
    {
        private static readonly DataAccess instance = new DataAccess();
        private readonly string connectionString = "Host=srv-peda-new;Port=5433;Username=renauale;Password=rMD0EE;Database=SAE201_sibilia;Options='-c search_path=sibilia'";
        private NpgsqlConnection connection;

        public static DataAccess Instance
        {
            get
            {
                return instance;
            }
        }

        //  Constructeur privé pour empêcher l'instanciation multiple
        private DataAccess()
        {
            
            try
            {
                connection = new NpgsqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                throw;
            }
        }


        // pour récupérer la connexion (et l'ouvrir si nécessaire)
        public NpgsqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb de connexion GetConnection \n" + connectionString);
                    throw;                
                }
            }

        
            return connection;
        }

        //  pour requêtes SELECT et retourne un DataTable ( table de données en mémoire)
        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL");
                throw;
            }
            return dataTable;
        }

        //   pour requêtes INSERT et renvoie l'ID généré

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex) { 
                LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                throw; }
            return nb;
        }




        //  pour requêtes UPDATE, DELETE
        public int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex) {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
            return nb;

        }

        // pour requêtes avec une seule valeur retour  (ex : COUNT, SUM) 
        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                cmd.Connection = GetConnection();
                res = cmd.ExecuteScalar();
            }
            catch (Exception ex) { 
                LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                throw;
            }
            return res;

        }

        //  Fermer la connexion 
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public int AddClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Le client à ajouter ne peut pas être null.");
            }

            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {

                cmd.CommandText = "INSERT INTO client (nomClient, prenomClient, tel, adresseRue, adresseCP, adresseVille) " +
                                  "VALUES (@NomClient, @PrenomClient, @Tel, @AdresseRue, @AdresseCP, @AdresseVille) " +
                                  "RETURNING numClient;"; 

                cmd.Parameters.AddWithValue("@NomClient", client.NomClient ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PrenomClient", client.PrenomClient ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Tel", client.Tel ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AdresseRue", client.AdresseRue ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AdresseCP", client.AdresseCP ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AdresseVille", client.AdresseVille ?? (object)DBNull.Value);


                return ExecuteInsert(cmd);
            }
        }
    }
}



