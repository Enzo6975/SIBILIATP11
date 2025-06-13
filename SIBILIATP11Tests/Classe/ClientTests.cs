using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIBILIATP11.Classe;
using System;

namespace SIBILIATP11.Classe.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod]
        public void ProprietesNomClient_PropertyChanged_Test()
        {
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "NomClient")
                    propertyChangedRaised = true;
            };
            client.NomClient = "Nouveau Nom";
            Assert.AreEqual("Nouveau Nom", client.NomClient);
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void ProprietsPrenomClient_PropertyChanged_Test()
        {
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "PrenomClient")
                    propertyChangedRaised = true;
            };
            client.PrenomClient = "Nouveau Prénom";
            Assert.AreEqual("Nouveau Prénom", client.PrenomClient);
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void ProprieteTel_PropertyChanged_Test()
        {
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Tel")
                    propertyChangedRaised = true;
            };
            client.Tel = "0555666777";
            Assert.AreEqual("0555666777", client.Tel);
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void PropertyChangedEvent_MultipleProperties_Test()
        {
            Client client = new Client();
            int propertyChangedCount = 0;
            client.PropertyChanged += (sender, e) => propertyChangedCount++;
            client.NomClient = "Test Nom";
            client.PrenomClient = "Test Prénom";
            client.Tel = "0123456789";
            client.AdresseRue = "Test Rue";
            client.AdresseCP = "12345";
            client.AdresseVille = "Test Ville";
            Assert.AreEqual(6, propertyChangedCount);
        }
    }
}
