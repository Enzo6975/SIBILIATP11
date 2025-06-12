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
    public class SousCategorieTests
    {
        [TestClass]
        public class SousCategorieTest
        {
            [TestMethod]
            public void SousCategorieTests()
            {
                // Arrange
                Categorie uneCategorie = new Categorie(1, "Entrées");
                SousCategorie sousCategorie = new SousCategorie(1, "Salades", uneCategorie);

                // Assert
                Assert.AreEqual(1, sousCategorie.NumSousCategorie, "Numéro sous-catégorie égal");
                Assert.AreEqual("Salades", sousCategorie.NomSousCategorie, "Nom sous-catégorie égal");
                Assert.AreEqual(uneCategorie, sousCategorie.UneCategorie, "Catégorie égale");
            }

            [TestMethod]
            public void SousCategorieTests_ConstructeurVide()
            {
                // Arrange & Act
                SousCategorie sousCategorie = new SousCategorie();

                // Assert
                Assert.AreEqual(0, sousCategorie.NumSousCategorie, "Numéro sous-catégorie par défaut");
                Assert.IsNull(sousCategorie.NomSousCategorie, "Nom sous-catégorie null par défaut");
                Assert.IsNull(sousCategorie.UneCategorie, "Catégorie null par défaut");
            }

            [TestMethod]
            public void SousCategorieTests_ConstructeurAvecNum()
            {
                // Arrange & Act
                SousCategorie sousCategorie = new SousCategorie(5);

                // Assert
                Assert.AreEqual(5, sousCategorie.NumSousCategorie, "Numéro sous-catégorie initialisé");
                Assert.IsNull(sousCategorie.NomSousCategorie, "Nom sous-catégorie null");
                Assert.IsNull(sousCategorie.UneCategorie, "Catégorie null");
            }

            
            [TestMethod]
            public void SousCategorieTests_ModificationProprietes()
            {
                // Arrange
                Categorie categorie1 = new Categorie(1, "Entrées");
                Categorie categorie2 = new Categorie(2, "Plats");
                SousCategorie sousCategorie = new SousCategorie();
                bool propertyChanged = false;

                sousCategorie.PropertyChanged += (sender, e) => propertyChanged = true;

                // Act & Assert
                sousCategorie.NumSousCategorie = 10;
                Assert.AreEqual(10, sousCategorie.NumSousCategorie, "Modification numéro");
                Assert.IsTrue(propertyChanged, "PropertyChanged déclenché pour NumSousCategorie");

                propertyChanged = false;
                sousCategorie.NomSousCategorie = "Soupes";
                Assert.AreEqual("Soupes", sousCategorie.NomSousCategorie, "Modification nom");
                Assert.IsTrue(propertyChanged, "PropertyChanged déclenché pour NomSousCategorie");

                propertyChanged = false;
                sousCategorie.UneCategorie = categorie1;
                Assert.AreEqual(categorie1, sousCategorie.UneCategorie, "Modification catégorie");
                Assert.IsTrue(propertyChanged, "PropertyChanged déclenché pour UneCategorie");
            }

            [TestMethod]
            public void SousCategorieTests_NomVide()
            {
                // Arrange
                Categorie uneCategorie = new Categorie(1, "Entrées");

                // Act
                SousCategorie sousCategorie = new SousCategorie(1, "", uneCategorie);

                // Assert
                Assert.AreEqual("", sousCategorie.NomSousCategorie, "Nom vide accepté");
            }

            [TestMethod]
            public void SousCategorieTests_NumeroNegatif()
            {
                // Arrange
                Categorie uneCategorie = new Categorie(1, "Entrées");

                // Act
                SousCategorie sousCategorie = new SousCategorie(-1, "Test", uneCategorie);

                // Assert
                Assert.AreEqual(-1, sousCategorie.NumSousCategorie, "Numéro négatif accepté");
            }
        }
    }
}