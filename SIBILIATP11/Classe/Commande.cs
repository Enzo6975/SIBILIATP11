using Npgsql;
using SIBILIATP11.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            get { return this.numCommande; }
            set { this.numCommande = value; }
        }

        public DateTime DateCommande
        {
            get { return this.dateCommande; }
            set
            {
                this.dateCommande = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateCommande)));
            }
        }

        public DateTime DateRetraitPrevue
        {
            get { return this.dateRetraitPrevue; }
            set
            {
                this.dateRetraitPrevue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateRetraitPrevue)));
            }
        }

        public bool Payee
        {
            get { return this.payee; }
            set
            {
                this.payee = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Payee)));
            }
        }

        public bool Retiree
        {
            get { return this.retiree; }
            set
            {
                this.retiree = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Retiree)));
            }
        }

        public double PrixTotal
        {
            get { return this.prixTotal; }
            set
            {
                this.prixTotal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrixTotal)));
            }
        }

        public Employe UnEmploye
        {
            get { return this.unEmploye; }
            set
            {
                this.unEmploye = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnEmploye)));
            }
        }

        public Client UnClient
        {
            get { return this.unClient; }
            set
            {
                this.unClient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnClient)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("INSERT INTO commande (datecommande, dateretraitprevue, payee, retiree, prixtotal, numemploye, numclient) VALUES (@dateCommande, @dateRetraitPrevue, @payee, @retiree, @prixTotal, @numEmploye, @numClient) RETURNING numcommande"))
            {
                cmdInsert.Parameters.AddWithValue("@dateCommande", this.DateCommande);
                cmdInsert.Parameters.AddWithValue("@dateRetraitPrevue", this.DateRetraitPrevue);
                cmdInsert.Parameters.AddWithValue("@payee", this.Payee);
                cmdInsert.Parameters.AddWithValue("@retiree", this.Retiree);
                cmdInsert.Parameters.AddWithValue("@prixTotal", this.PrixTotal);
                cmdInsert.Parameters.AddWithValue("@numEmploye", this.UnEmploye.NumEmploye);
                cmdInsert.Parameters.AddWithValue("@numClient", this.UnClient.NumClient);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumCommande = nb;
            return nb;
        }

        public int Update()
        {
            try
            {
                using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(
                    "UPDATE commande SET datecommande = @dateCommande, dateretraitprevue = @dateRetraitPrevue, payee = @payee, retiree = @retiree, prixtotal = @prixTotal, numemploye = @numEmploye, numclient = @numClient WHERE numcommande = @numcommande"))
                {
                    cmdUpdate.Parameters.AddWithValue("@dateCommande", this.DateCommande);
                    cmdUpdate.Parameters.AddWithValue("@dateRetraitPrevue", this.DateRetraitPrevue);
                    cmdUpdate.Parameters.AddWithValue("@payee", this.Payee);
                    cmdUpdate.Parameters.AddWithValue("@retiree", this.Retiree);
                    cmdUpdate.Parameters.AddWithValue("@prixTotal", this.PrixTotal);
                    cmdUpdate.Parameters.AddWithValue("@numEmploye", this.UnEmploye.NumEmploye);
                    cmdUpdate.Parameters.AddWithValue("@numClient", this.UnClient.NumClient);
                    cmdUpdate.Parameters.AddWithValue("@numcommande", this.NumCommande);

                    return DataAccess.Instance.ExecuteSet(cmdUpdate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la mise à jour de la commande en base de données.", ex);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("DELETE FROM commande WHERE numcommande = @numCommande"))
            {
                cmdDelete.Parameters.AddWithValue("@numCommande", this.NumCommande);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Commande> FindAll()
        {
            List<Commande> lesCommandes = new List<Commande>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM commande"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesCommandes.Add(new Commande(
                        (Int32)dr["numcommande"],
                        (DateTime)dr["datecommande"],
                        (DateTime)dr["dateretraitprevue"],
                        (Boolean)dr["payee"],
                        (Boolean)dr["retiree"],
                        Convert.ToDouble(dr["prixtotal"]),
                        new Employe((Int32)dr["numemploye"]),
                        new Client((Int32)dr["numclient"])
                    ));
                }
            }
            return lesCommandes;
        }

        public List<Commande> FindBySelection(string criteres)
        {
            List<Commande> lesCommandes = new List<Commande>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT * FROM commande WHERE {criteres}"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesCommandes.Add(new Commande(
                        (Int32)dr["numcommande"],
                        (DateTime)dr["datecommande"],
                        (DateTime)dr["dateretraitprevue"],
                        (Boolean)dr["payee"],
                        (Boolean)dr["retiree"],
                        Convert.ToDouble(dr["prixtotal"]),
                        new Employe((Int32)dr["numemploye"]),
                        new Client((Int32)dr["numclient"])
                    ));
                }
            }
            return lesCommandes;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("SELECT * FROM commande WHERE numcommande = @numCommande"))
            {
                cmdSelect.Parameters.AddWithValue("@numCommande", this.NumCommande);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.DateCommande = (DateTime)dt.Rows[0]["datecommande"];
                    this.DateRetraitPrevue = (DateTime)dt.Rows[0]["dateretraitprevue"];
                    this.Payee = (Boolean)dt.Rows[0]["payee"];
                    this.Retiree = (Boolean)dt.Rows[0]["retiree"];
                    this.PrixTotal = Convert.ToDouble(dt.Rows[0]["prixtotal"]);
                    this.UnEmploye = new Employe((Int32)dt.Rows[0]["numemploye"]);
                    this.UnClient = new Client((Int32)dt.Rows[0]["numclient"]);
                    this.UnEmploye.Read();
                    this.UnClient.Read();
                }
            }
        }
    }
}