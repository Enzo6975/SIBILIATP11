using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIBILIATP11.Classe;
using System;

namespace SIBILIATP11.Classe.Tests
{
    [TestClass()]
    public class CommandeTests
    {


        [TestMethod]
        public void ProprieteDateCommande_PropertyChanged_Test()
        {
            Commande commande = new Commande();
            bool propertyChangedRaised = false;
            DateTime date = new DateTime(2024, 5, 1);
            commande.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "DateCommande")
                    propertyChangedRaised = true;
            };
            commande.DateCommande = date;
            Assert.AreEqual(date, commande.DateCommande);
            Assert.IsTrue(propertyChangedRaised);
        }
        [TestMethod]
        public void Commande_CalculMontantTotal_Test()
        {
            var plats = new List<Plat>
    {
        new Plat { PrixUnitaire = 12.5 },
        new Plat { PrixUnitaire = 18.0 },
        new Plat { PrixUnitaire = 9.5 }
    };

            double montantTotal = plats.Sum(p => p.PrixUnitaire);

            Assert.AreEqual(40.0, montantTotal, 0.01);
        }


    }
}
