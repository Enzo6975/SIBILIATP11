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
    public class ContientTests
    {
        [TestClass]
        public class ContientTest
        {
            [TestMethod]
            public void ContientConstructeurVide_Test()
            {
                // Arrange & Act
                Contient contient = new Contient();

                // Assert
                Assert.AreEqual(0, contient.Quantite, "Quantité doit être 0 par défaut");
                Assert.AreEqual(0.0, contient.Prix, "Prix doit être 0.0 par défaut");
                Assert.IsNull(contient.UneCommande, "Commande doit être null par défaut");
                Assert.IsNull(contient.UnPlat, "Plat doit être null par défaut");
            }

            [TestMethod]
            public void ContientConstructeurComplet_Test()
            {
                // Arrange
                Commande commande = new Commande(1);
                Plat plat = new Plat(5);

                // Act
                Contient contient = new Contient(3, 15.50, commande, plat);

                // Assert
                Assert.AreEqual(3, contient.Quantite, "Quantité doit être 3");
                Assert.AreEqual(15.50, contient.Prix, "Prix doit être 15.50");
                Assert.AreEqual(commande, contient.UneCommande, "Commande doit être celle passée");
                Assert.AreEqual(plat, contient.UnPlat, "Plat doit être celui passé");
            }

            [TestMethod]
            public void ProprieteQuantite_Test()
            {
                // Arrange
                Contient contient = new Contient();
                bool propertyChangedRaised = false;
                contient.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Quantite")
                        propertyChangedRaised = true;
                };

                // Act
                contient.Quantite = 5;

                // Assert
                Assert.AreEqual(5, contient.Quantite, "La quantité doit être mise à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprietePrix_Test()
            {
                // Arrange
                Contient contient = new Contient();
                bool propertyChangedRaised = false;
                contient.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Prix")
                        propertyChangedRaised = true;
                };

                // Act
                contient.Prix = 25.75;

                // Assert
                Assert.AreEqual(25.75, contient.Prix, "Le prix doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprieteUneCommande_Test()
            {
                // Arrange
                Contient contient = new Contient();
                Commande commande = new Commande(10);
                bool propertyChangedRaised = false;
                contient.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "UneCommande")
                        propertyChangedRaised = true;
                };

                // Act
                contient.UneCommande = commande;

                // Assert
                Assert.AreEqual(commande, contient.UneCommande, "La commande doit être mise à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprieteUnPlat_Test()
            {
                // Arrange
                Contient contient = new Contient();
                Plat plat = new Plat(15);
                bool propertyChangedRaised = false;
                contient.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "UnPlat")
                        propertyChangedRaised = true;
                };

                // Act
                contient.UnPlat = plat;

                // Assert
                Assert.AreEqual(plat, contient.UnPlat, "Le plat doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void CalculerTotal_Test()
            {
                // Arrange
                Commande commande = new Commande(1);
                Plat plat = new Plat(1);
                Contient contient = new Contient(4, 12.50, commande, plat);

                // Act
                double total = contient.CalculerTotal;

                // Assert
                Assert.AreEqual(50.0, total, "Le total doit être 4 * 12.50 = 50.0");
            }

            [TestMethod]
            public void CalculerTotal_QuantiteZero_Test()
            {
                // Arrange
                Commande commande = new Commande(1);
                Plat plat = new Plat(1);
                Contient contient = new Contient(0, 10.0, commande, plat);

                // Act
                double total = contient.CalculerTotal;

                // Assert
                Assert.AreEqual(0.0, total, "Le total doit être 0 si quantité = 0");
            }

            [TestMethod]
            public void CalculerTotal_PrixZero_Test()
            {
                // Arrange
                Commande commande = new Commande(1);
                Plat plat = new Plat(1);
                Contient contient = new Contient(5, 0.0, commande, plat);

                // Act
                double total = contient.CalculerTotal;

                // Assert
                Assert.AreEqual(0.0, total, "Le total doit être 0 si prix = 0");
            }

            [TestMethod]
            public void CalculerTotal_ValeursDécimales_Test()
            {
                // Arrange
                Commande commande = new Commande(1);
                Plat plat = new Plat(1);
                Contient contient = new Contient(3, 7.33, commande, plat);

                // Act
                double total = contient.CalculerTotal;

                // Assert
                Assert.AreEqual(21.99, total, 0.01, "Le total doit être 3 * 7.33 = 21.99");
            }

            [TestMethod]
            public void CalculerTotal_MiseAJourDynamique_Test()
            {
                // Arrange
                Commande commande = new Commande(1);
                Plat plat = new Plat(1);
                Contient contient = new Contient(2, 10.0, commande, plat);

                // Act & Assert - Premier calcul
                Assert.AreEqual(20.0, contient.CalculerTotal, "Total initial doit être 20.0");

                // Modification de la quantité
                contient.Quantite = 5;
                Assert.AreEqual(50.0, contient.CalculerTotal, "Total après modification quantité doit être 50.0");

                // Modification du prix
                contient.Prix = 8.0;
                Assert.AreEqual(40.0, contient.CalculerTotal, "Total après modification prix doit être 40.0");
            }

            [TestMethod]
            public void PropertyChangedEvent_MultipleProperties_Test()
            {
                // Arrange
                Contient contient = new Contient();
                Commande commande = new Commande(1);
                Plat plat = new Plat(1);
                int propertyChangedCount = 0;
                contient.PropertyChanged += (sender, e) => propertyChangedCount++;

                // Act
                contient.Quantite = 3;
                contient.Prix = 15.0;
                contient.UneCommande = commande;
                contient.UnPlat = plat;

                // Assert
                Assert.AreEqual(4, propertyChangedCount, "PropertyChanged doit être déclenché 4 fois");
            }

            [TestMethod]
            public void ContientComplet_AvecCalcul_Test()
            {
                // Arrange
                Commande commande = new Commande(100);
                Plat plat = new Plat(200);

                // Act
                Contient contient = new Contient(6, 9.99, commande, plat);

                // Assert
                Assert.AreEqual(6, contient.Quantite, "Quantité incorrecte");
                Assert.AreEqual(9.99, contient.Prix, "Prix incorrect");
                Assert.AreEqual(commande, contient.UneCommande, "Commande incorrecte");
                Assert.AreEqual(plat, contient.UnPlat, "Plat incorrect");
                Assert.AreEqual(59.94, contient.CalculerTotal, 0.01, "Total calculé incorrect");
            }

            [TestMethod]
            public void ModificationsSuccessives_Test()
            {
                // Arrange
                Contient contient = new Contient();
                Commande commande1 = new Commande(1);
                Commande commande2 = new Commande(2);
                Plat plat1 = new Plat(10);
                Plat plat2 = new Plat(20);

                // Act
                contient.Quantite = 2;
                contient.Prix = 5.0;
                contient.UneCommande = commande1;
                contient.UnPlat = plat1;

                double total1 = contient.CalculerTotal;

                contient.Quantite = 3;
                contient.Prix = 8.0;
                contient.UneCommande = commande2;
                contient.UnPlat = plat2;

                double total2 = contient.CalculerTotal;

                // Assert
                Assert.AreEqual(10.0, total1, "Premier total doit être 10.0");
                Assert.AreEqual(24.0, total2, "Deuxième total doit être 24.0");
                Assert.AreEqual(commande2, contient.UneCommande, "Commande finale incorrecte");
                Assert.AreEqual(plat2, contient.UnPlat, "Plat final incorrect");
            }
        }
    }
}