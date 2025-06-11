using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SIBILIATP11.Classe
{
    public class Commande : ICrud<Commande>, INotifyPropertyChanged
    {
        private int numCommande;
        private DateTime dateCommande;
        private DateTime dateRetraitPrevue;
        private bool payee;
        private bool retiree;
        private double prixTotal;
        private Employe unEmploye;
        private Client unClient;

        public Commande()
        {

        }

        public Commande(int numCommande)
        {
            this.NumCommande = numCommande;
        }

        public Commande(int numCommande, DateTime dateCommande, DateTime dateRetraitPrevue, bool payee, bool retiree, double prixTotal, Employe unEmploye, Client unClient)
        {
            this.NumCommande = numCommande;
            this.DateCommande = dateCommande;
            this.DateRetraitPrevue = dateRetraitPrevue;
            this.Payee = payee;
            this.Retiree = retiree;
            this.PrixTotal = prixTotal;
            this.UnEmploye = unEmploye;
            this.UnClient = unClient;
        }

        public int NumCommande
        {
            get
            {
                return this.numCommande;
            }

            set
            {
                this.numCommande = value;
            }
        }

        public DateTime DateCommande
        {
            get
            {
                return this.dateCommande;
            }

            set
            {
                this.dateCommande = value;
            }
        }

        public DateTime DateRetraitPrevue
        {
            get
            {
                return this.dateRetraitPrevue;
            }

            set
            {
                this.dateRetraitPrevue = value;
            }
        }

        public bool Payee
        {
            get
            {
                return this.payee;
            }

            set
            {
                this.payee = value;
            }
        }

        public bool Retiree
        {
            get
            {
                return this.retiree;
            }

            set
            {
                this.retiree = value;
            }
        }

        public double PrixTotal
        {
            get
            {
                return this.prixTotal;
            }

            set
            {
                this.prixTotal = value;
            }
        }

        public Employe UnEmploye
        {
            get
            {
                return this.unEmploye;
            }

            set
            {
                this.unEmploye = value;
            }
        }

        public Client UnClient
        {
            get
            {
                return this.unClient;
            }

            set
            {
                this.unClient = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public List<Commande> FindAll()
        {
            List<Commande> lesCommandes = new List<Commande>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from commande ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesCommandes.Add(new Commande((Int32)dr["numcommande"], (DateTime)dr["datecommande"], (DateTime)dr["dateretraitprevue"], (Boolean)dr["payee"], (Boolean)dr["retiree"], (Double)dr["prixtotal"], new Employe((Int32)dr["numemploye"]), new Client((Int32)dr["numclient"])));
            }
            return lesCommandes;
        }

        public List<Commande> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
