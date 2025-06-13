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

    }
}
