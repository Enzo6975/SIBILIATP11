using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIBILIATP11.Model;

namespace SIBILIATP11.Classe
{
    public class Client: ICrud<Client>, INotifyPropertyChanged
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

        public Client(int numClient, string nomClient,  string prenomClient, string tel)
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
            get
            {
                return this.numClient;
            }

            set
            {
                this.numClient = value;
            }
        }

        public string NomClient
        {
            get
            {
                return this.nomClient;
            }

            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomClient)));
            }
        }

        public string PrenomClient
        {
            get
            {
                return this.prenomClient;
            }

            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrenomClient)));
            }
        }

        public string Tel
        {
            get
            {
                return this.tel;
            }

            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tel)));
            }
        }

        public string AdresseRue
        {
            get
            {
                return this.adresseRue;
            }

            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdresseRue)));
            }
        }

        public string AdresseCP
        {
            get
            {
                return this.adresseCP;
            }

            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdresseCP)));
            }
        }

        public string AdresseVille
        {
            get
            {
                return this.adresseVille;
            }

            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdresseVille)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {

            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into client (nomclient, prenomclient, tel, adresserue, adressecp, adresseville) values (@nomClient, @prenomClient, @tel, @adresserue, @adressecp, @adresseville) RETURNING numclient"))
            {
                cmdInsert.Parameters.AddWithValue("nomclient", this.NomClient);
                cmdInsert.Parameters.AddWithValue("prenomclient", this.PrenomClient);
                cmdInsert.Parameters.AddWithValue("tel", this.Tel);
                cmdInsert.Parameters.AddWithValue("adresserue", this.AdresseRue);
                cmdInsert.Parameters.AddWithValue("adressecp", this.AdresseCP);
                cmdInsert.Parameters.AddWithValue("adresseville", this.AdresseVille);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumClient = nb;
            return nb;
        }

        public int Delete()
        {
            using (var cmdUpdate = new NpgsqlCommand("delete from client where numclient =@numClient;"))
            {
                cmdUpdate.Parameters.AddWithValue("numclient", this.NumClient);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from client;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesClients.Add(new Client((Int32)dr["numclient"], (String)dr["nomclient"], (String)dr["prenomclient"], (String)dr["tel"], (String)dr["adresserue"], (String)dr["adressecp"], (String)dr["adresseville"]));
            }
            return lesClients;
        }

        public List<Client> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from client where numclient =@numClient;"))
            {
                cmdSelect.Parameters.AddWithValue("numclient", this.NumClient);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                this.NomClient = (String)dt.Rows[0]["nomclient"];
                this.PrenomClient = (String)dt.Rows[0]["prenomclient"];
                this.Tel = (String)dt.Rows[0]["tel"];
                this.AdresseRue = (String)dt.Rows[0]["adresserue"];
                this.AdresseCP = (String)dt.Rows[0]["adressecp"];
                this.AdresseVille = (String)dt.Rows[0]["adresseville"];

            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update client set nomclient =@nomClient, prenomclient = @prenomClient, tel = @tel, adresserue = @adresseRue, adressecp = @adresseCP, adresseville = @adresseVille where numclient =@numClient;"))
            {
                cmdUpdate.Parameters.AddWithValue("nomclient", this.NomClient);
                cmdUpdate.Parameters.AddWithValue("prenomclient", this.PrenomClient);
                cmdUpdate.Parameters.AddWithValue("tel", this.Tel);
                cmdUpdate.Parameters.AddWithValue("adresserue", this.AdresseRue);
                cmdUpdate.Parameters.AddWithValue("adressecp", this.AdresseCP);
                cmdUpdate.Parameters.AddWithValue("adresseville", this.AdresseVille);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }
    }
}
