using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILIATP11.Classe.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod]
        public void ClientConstructeurVide_Test()
        {
            // Arrange & Act
            Client client = new Client();

            // Assert
            Assert.AreEqual(0, client.NumClient, "Numéro client doit être 0 par défaut");
            Assert.IsNull(client.NomClient, "Nom client doit être null par défaut");
            Assert.IsNull(client.PrenomClient, "Prénom client doit être null par défaut");
            Assert.IsNull(client.Tel, "Téléphone doit être null par défaut");
        }

        [TestMethod]
        public void ClientConstructeurAvecNumero_Test()
        {
            // Arrange & Act
            Client client = new Client(123);

            // Assert
            Assert.AreEqual(123, client.NumClient, "Numéro client doit être 123");
        }

        [TestMethod]
        public void ClientConstructeurComplet_Test()
        {
            // Arrange & Act
            Client client = new Client(456, "Dupont", "Jean", "0123456789");

            // Assert
            Assert.AreEqual(456, client.NumClient, "Numéro client doit être 456");
            Assert.AreEqual("Dupont", client.NomClient, "Nom client doit être Dupont");
            Assert.AreEqual("Jean", client.PrenomClient, "Prénom client doit être Jean");
            Assert.AreEqual("0123456789", client.Tel, "Téléphone doit être 0123456789");
        }

        [TestMethod]
        public void ClientConstructeurAvecAdresse_Test()
        {
            // Arrange & Act
            Client client = new Client(789, "Martin", "Marie", "0987654321", "123 Rue de la Paix", "75001", "Paris");

            // Assert
            Assert.AreEqual(789, client.NumClient, "Numéro client doit être 789");
            Assert.AreEqual("Martin", client.NomClient, "Nom client doit être Martin");
            Assert.AreEqual("Marie", client.PrenomClient, "Prénom client doit être Marie");
            Assert.AreEqual("0987654321", client.Tel, "Téléphone doit être 0987654321");
            Assert.AreEqual("123 Rue de la Paix", client.AdresseRue, "Adresse rue doit être correcte");
            Assert.AreEqual("75001", client.AdresseCP, "Code postal doit être 75001");
            Assert.AreEqual("Paris", client.AdresseVille, "Ville doit être Paris");
        }

        [TestMethod]
        public void ProprietesNumClient_Test()
        {
            // Arrange
            Client client = new Client();

            // Act
            client.NumClient = 999;

            // Assert
            Assert.AreEqual(999, client.NumClient, "La propriété NumClient doit fonctionner correctement");
        }

        [TestMethod]
        public void ProprietesNomClient_Test()
        {
            // Arrange
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "NomClient")
                    propertyChangedRaised = true;
            };

            // Act
            client.NomClient = "Nouveau Nom";

            // Assert
            Assert.AreEqual("Nouveau Nom", client.NomClient, "Le nom client doit être mis à jour");
            Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
        }

        [TestMethod]
        public void ProprietsPrenomClient_Test()
        {
            // Arrange
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "PrenomClient")
                    propertyChangedRaised = true;
            };

            // Act
            client.PrenomClient = "Nouveau Prénom";

            // Assert
            Assert.AreEqual("Nouveau Prénom", client.PrenomClient, "Le prénom client doit être mis à jour");
            Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
        }

        [TestMethod]
        public void ProprieteTel_Test()
        {
            // Arrange
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Tel")
                    propertyChangedRaised = true;
            };

            // Act
            client.Tel = "0555666777";

            // Assert
            Assert.AreEqual("0555666777", client.Tel, "Le téléphone doit être mis à jour");
            Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
        }

        [TestMethod]
        public void ProprieteAdresseRue_Test()
        {
            // Arrange
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "AdresseRue")
                    propertyChangedRaised = true;
            };

            // Act
            client.AdresseRue = "456 Avenue des Champs";

            // Assert
            Assert.AreEqual("456 Avenue des Champs", client.AdresseRue, "L'adresse rue doit être mise à jour");
            Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
        }

        [TestMethod]
        public void ProprieteAdresseCP_Test()
        {
            // Arrange
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "AdresseCP")
                    propertyChangedRaised = true;
            };

            // Act
            client.AdresseCP = "69000";

            // Assert
            Assert.AreEqual("69000", client.AdresseCP, "Le code postal doit être mis à jour");
            Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
        }

        [TestMethod]
        public void ProprieteAdresseVille_Test()
        {
            // Arrange
            Client client = new Client();
            bool propertyChangedRaised = false;
            client.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "AdresseVille")
                    propertyChangedRaised = true;
            };

            // Act
            client.AdresseVille = "Lyon";

            // Assert
            Assert.AreEqual("Lyon", client.AdresseVille, "La ville doit être mise à jour");
            Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
        }

        [TestMethod]
        public void PropertyChangedEvent_MultipleProperties_Test()
        {
            // Arrange
            Client client = new Client();
            int propertyChangedCount = 0;
            client.PropertyChanged += (sender, e) => propertyChangedCount++;

            // Act
            client.NomClient = "Test Nom";
            client.PrenomClient = "Test Prénom";
            client.Tel = "0123456789";
            client.AdresseRue = "Test Rue";
            client.AdresseCP = "12345";
            client.AdresseVille = "Test Ville";

            // Assert
            Assert.AreEqual(6, propertyChangedCount, "PropertyChanged doit être déclenché 6 fois");
        }

        [TestMethod]
        public void ClientComplet_TodosLesChamps_Test()
        {
            // Arrange & Act
            Client client = new Client(100, "Durand", "Pierre", "0147258369", "789 Boulevard Saint-Michel", "75005", "Paris");

            // Assert - Vérification de tous les champs
            Assert.AreEqual(100, client.NumClient, "Numéro client incorrect");
            Assert.AreEqual("Durand", client.NomClient, "Nom client incorrect");
            Assert.AreEqual("Pierre", client.PrenomClient, "Prénom client incorrect");
            Assert.AreEqual("0147258369", client.Tel, "Téléphone incorrect");
            Assert.AreEqual("789 Boulevard Saint-Michel", client.AdresseRue, "Adresse rue incorrecte");
            Assert.AreEqual("75005", client.AdresseCP, "Code postal incorrect");
            Assert.AreEqual("Paris", client.AdresseVille, "Ville incorrecte");
        }
    }
}