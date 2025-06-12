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
    public class PlatTests
    {
        [TestClass]
        public class PlatTest
        {
            [TestMethod]
            public void PlatTests()
            {
                // Arrange
                Categorie uneCategorie = new Categorie(1, "Plats principaux");
                SousCategorie uneSousCategorie = new SousCategorie(1, "Viandes", uneCategorie);
                Periode unePeriode = new Periode(1, "Été");
                Plat plat = new Plat(1, "Boeuf bourguignon", 25.50, 45, 4, uneSousCategorie, unePeriode);

                // Assert
                Assert.AreEqual(1, plat.NumPlat, "Numéro plat égal");
                Assert.AreEqual("Boeuf bourguignon", plat.NomPlat, "Nom plat égal");
                Assert.AreEqual(25.50, plat.PrixUnitaire, 0.01, "Prix unitaire égal");
                Assert.AreEqual(45, plat.DelaiPreparation, "Délai préparation égal");
                Assert.AreEqual(4, plat.NbPersonnes, "Nombre personnes égal");
                Assert.AreEqual(uneSousCategorie, plat.UneSousCategorie, "Sous-catégorie égale");
                Assert.AreEqual(unePeriode, plat.UnePeriode, "Période égale");
            }

            [TestMethod]
            public void PlatTests_ConstructeurVide()
            {
                // Arrange & Act
                Plat plat = new Plat();

                // Assert
                Assert.AreEqual(0, plat.NumPlat, "Numéro plat par défaut");
                Assert.IsNull(plat.NomPlat, "Nom plat null par défaut");
                Assert.AreEqual(0.0, plat.PrixUnitaire, 0.01, "Prix unitaire par défaut");
                Assert.AreEqual(0, plat.DelaiPreparation, "Délai préparation par défaut");
                Assert.AreEqual(0, plat.NbPersonnes, "Nombre personnes par défaut");
                Assert.IsNull(plat.UneSousCategorie, "Sous-catégorie null par défaut");
                Assert.IsNull(plat.UnePeriode, "Période null par défaut");
            }

            [TestMethod]
            public void PlatTests_ConstructeurAvecNum()
            {
                // Arrange & Act
                Plat plat = new Plat(5);

                // Assert
                Assert.AreEqual(5, plat.NumPlat, "Numéro plat initialisé");
                Assert.IsNull(plat.NomPlat, "Nom plat null");
                Assert.AreEqual(0.0, plat.PrixUnitaire, 0.01, "Prix unitaire par défaut");
                Assert.AreEqual(0, plat.DelaiPreparation, "Délai préparation par défaut");
                Assert.AreEqual(0, plat.NbPersonnes, "Nombre personnes par défaut");
                Assert.IsNull(plat.UneSousCategorie, "Sous-catégorie null");
                Assert.IsNull(plat.UnePeriode, "Période null");
            }


            [TestMethod]
            public void PlatTests_ModificationProprietes()
            {
                // Arrange
                Plat plat = new Plat();
                bool propertyChanged = false;

                plat.PropertyChanged += (sender, e) => propertyChanged = true;

                // Act & Assert
                plat.NomPlat = "Coq au vin";
                Assert.AreEqual("Coq au vin", plat.NomPlat, "Modification nom");
                Assert.IsTrue(propertyChanged, "PropertyChanged déclenché pour NomPlat");

                propertyChanged = false;
                plat.PrixUnitaire = 18.75;
                Assert.AreEqual(18.75, plat.PrixUnitaire, 0.01, "Modification prix");
                Assert.IsTrue(propertyChanged, "PropertyChanged déclenché pour PrixUnitaire");

                propertyChanged = false;
                plat.DelaiPreparation = 60;
                Assert.AreEqual(60, plat.DelaiPreparation, "Modification délai");
                Assert.IsTrue(propertyChanged, "PropertyChanged déclenché pour DelaiPreparation");

                propertyChanged = false;
                plat.NbPersonnes = 6;
                Assert.AreEqual(6, plat.NbPersonnes, "Modification nombre personnes");
                Assert.IsTrue(propertyChanged, "PropertyChanged déclenché pour NbPersonnes");
            }

            [TestMethod]
            public void PlatTests_ValeursBordure()
            {
                // Arrange
                Categorie uneCategorie = new Categorie(1, "Plats principaux");
                SousCategorie uneSousCategorie = new SousCategorie(1, "Viandes", uneCategorie);
                Periode unePeriode = new Periode(1, "Été");

                // Act
                Plat plat = new Plat(1, "", 0.01, 1, 1, uneSousCategorie, unePeriode);

                // Assert
                Assert.AreEqual("", plat.NomPlat, "Nom vide accepté");
                Assert.AreEqual(0.01, plat.PrixUnitaire, 0.001, "Prix minimum accepté");
                Assert.AreEqual(1, plat.DelaiPreparation, "Délai minimum accepté");
                Assert.AreEqual(1, plat.NbPersonnes, "Une personne acceptée");
            }

            [TestMethod]
            public void PlatTests_ValeursElevees()
            {
                // Arrange
                Categorie uneCategorie = new Categorie(1, "Plats principaux");
                SousCategorie uneSousCategorie = new SousCategorie(1, "Viandes", uneCategorie);
                Periode unePeriode = new Periode(1, "Été");

                // Act
                Plat plat = new Plat(1, "Plat très long nom pour tester la limite", 999.99, 480, 20, uneSousCategorie, unePeriode);

                // Assert
                Assert.AreEqual("Plat très long nom pour tester la limite", plat.NomPlat, "Nom long accepté");
                Assert.AreEqual(999.99, plat.PrixUnitaire, 0.01, "Prix élevé accepté");
                Assert.AreEqual(480, plat.DelaiPreparation, "Délai long accepté");
                Assert.AreEqual(20, plat.NbPersonnes, "Nombre élevé de personnes accepté");
            }
        }
    }
}