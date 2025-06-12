using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIBILIATP11.Model;
using System.Windows;

namespace SIBILIATP11.Classe
{
    public class Client : ICrud<Client>, INotifyPropertyChanged
    {
        public int numClient;
        public string nomClient;
        public string prenomClient;
        public string tel;
        public string adresseRue;
        public string adresseCP;
        public string adresseVille;

        public Client()
        {

        }

        public Client(int numClient)
        {
            this.numClient = numClient;
        }

        public Client(int numClient, string nomClient, string prenomClient, string tel)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.Tel = tel;
        }

        public Client(int numClient, string nomClient, string prenomClient, string tel, string adresseRue, string adresseCP, string adresseVille)
        {
            this.NumClient = numClient;
            this.NomClient = nomClient;
            this.PrenomClient = prenomClient;
            this.Tel = tel;
            this.AdresseRue = adresseRue;
            this.AdresseCP = adresseCP;
            this.AdresseVille = adresseVille;
        }

        public int NumClient
        {
            get { return this.numClient; }
            set { this.numClient = value; }
        }

        public string NomClient
        {
            get { return this.nomClient; }
            set
            {
                this.nomClient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomClient)));
            }
        }

        public string PrenomClient
        {
            get { return this.prenomClient; }
            set
            {
                this.prenomClient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrenomClient)));
            }
        }

        public string Tel
        {
            get { return this.tel; }
            set
            {
                this.tel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tel)));
            }
        }

        public string AdresseRue
        {
            get { return this.adresseRue; }
            set
            {
                this.adresseRue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdresseRue)));
            }
        }

        public string AdresseCP
        {
            get { return this.adresseCP; }
            set
            {
                this.adresseCP = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdresseCP)));
            }
        }

        public string AdresseVille
        {
            get { return this.adresseVille; }
            set
            {
                this.adresseVille = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdresseVille)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT numclient, nomclient, prenomclient, tel, adresserue, adressecp, adresseville FROM client"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesClients.Add(new Client(
                        (Int32)dr["numclient"],
                        dr["nomclient"]?.ToString() ?? "",
                        dr["prenomclient"]?.ToString() ?? "",
                        dr["tel"]?.ToString() ?? "",
                        dr["adresserue"]?.ToString() ?? "",
                        dr["adressecp"]?.ToString() ?? "",
                        dr["adresseville"]?.ToString() ?? ""
                    ));
                }
            }
            return lesClients;
        }

        public List<Client> FindBySelection(string criteres)
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM client WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesClients.Add(new Client(
                        (Int32)dr["numclient"],
                        dr["nomclient"]?.ToString() ?? "",
                        dr["prenomclient"]?.ToString() ?? "",
                        dr["tel"]?.ToString() ?? "",
                        dr["adresserue"]?.ToString() ?? "",
                        dr["adressecp"]?.ToString() ?? "",
                        dr["adresseville"]?.ToString() ?? ""
                    ));
                }
            }
            return lesClients;
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO client (nomclient, prenomclient, tel, adresserue, adressecp, adresseville) VALUES (@nomClient, @prenomClient, @tel, @adresserue, @adressecp, @adresseville) RETURNING numclient"))
            {
                cmdInsert.Parameters.AddWithValue("@nomClient", this.NomClient ?? "");
                cmdInsert.Parameters.AddWithValue("@prenomClient", this.PrenomClient ?? "");
                cmdInsert.Parameters.AddWithValue("@tel", this.Tel ?? "");
                cmdInsert.Parameters.AddWithValue("@adresserue", this.AdresseRue ?? "");
                cmdInsert.Parameters.AddWithValue("@adressecp", this.AdresseCP ?? "");
                cmdInsert.Parameters.AddWithValue("@adresseville", this.AdresseVille ?? "");
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumClient = nb;
            return nb;
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("UPDATE client SET nomclient = @nomClient, prenomclient = @prenomClient, tel = @tel, adresserue = @adresseRue, adressecp = @adresseCP, adresseville = @adresseVille WHERE numclient = @numClient"))
            {
                cmdUpdate.Parameters.AddWithValue("@nomClient", this.NomClient ?? "");
                cmdUpdate.Parameters.AddWithValue("@prenomClient", this.PrenomClient ?? "");
                cmdUpdate.Parameters.AddWithValue("@tel", this.Tel ?? "");
                cmdUpdate.Parameters.AddWithValue("@adresseRue", this.AdresseRue ?? "");
                cmdUpdate.Parameters.AddWithValue("@adresseCP", this.AdresseCP ?? "");
                cmdUpdate.Parameters.AddWithValue("@adresseVille", this.AdresseVille ?? "");
                cmdUpdate.Parameters.AddWithValue("@numClient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdUpdate = new NpgsqlCommand("DELETE FROM client WHERE numclient = @numClient"))
            {
                cmdUpdate.Parameters.AddWithValue("@numClient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM client WHERE numclient = @numClient"))
            {
                cmdSelect.Parameters.AddWithValue("@numClient", this.NumClient);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomClient = dt.Rows[0]["nomclient"]?.ToString() ?? "";
                    this.PrenomClient = dt.Rows[0]["prenomclient"]?.ToString() ?? "";
                    this.Tel = dt.Rows[0]["tel"]?.ToString() ?? "";
                    this.AdresseRue = dt.Rows[0]["adresserue"]?.ToString() ?? "";
                    this.AdresseCP = dt.Rows[0]["adressecp"]?.ToString() ?? "";
                    this.AdresseVille = dt.Rows[0]["adresseville"]?.ToString() ?? "";
                }
            }
        }
    }
}