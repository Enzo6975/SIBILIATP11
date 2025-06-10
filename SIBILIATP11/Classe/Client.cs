using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe
{
    public class Client
    {
        public int numClient { get; set; } 
        public string nomClient { get; set; }
        public string prenomClient { get; set; }
        public string tel { get; set; }
        public string adresseRue { get; set; }
        public string adresseCP { get; set; }
        public string adresseVille { get; set; }

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

        public Client()
        { }

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
                this.nomClient = value;
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
                this.prenomClient = value;
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
                this.tel = value;
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
                this.adresseRue = value;
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
                this.adresseCP = value;
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
                this.adresseVille = value;
            }
        }
    }
}
