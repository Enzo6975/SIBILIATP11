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
    public class CategorieTests
    {
        [TestClass]
        public class CategorieTest
        {
            [TestMethod]
            public void CategorieConstructeurVide_Test()
            {
                // Arrange & Act
                Categorie categorie = new Categorie();

                // Assert
                Assert.AreEqual(0, categorie.NumCategorie, "Numéro catégorie doit être 0 par défaut");
                Assert.IsNull(categorie.NomCategorie, "Nom catégorie doit être null par défaut");
            }

            [TestMethod]
            public void CategorieConstructeurAvecNumero_Test()
            {
                // Arrange & Act
                Categorie categorie = new Categorie(5);

                // Assert
                Assert.AreEqual(5, categorie.NumCategorie, "Numéro catégorie doit être 5");
            }

            [TestMethod]
            public void CategorieConstructeurComplet_Test()
            {
                // Arrange & Act
                Categorie categorie = new Categorie(10, "Electronique");

                // Assert
                Assert.AreEqual(10, categorie.NumCategorie, "Numéro catégorie doit être 10");
                Assert.AreEqual("Electronique", categorie.NomCategorie, "Nom catégorie doit être Electronique");
            }

            [TestMethod]
            public void ProprieteNumCategorie_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();
                bool propertyChangedRaised = false;
                categorie.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "NumCategorie")
                        propertyChangedRaised = true;
                };

                // Act
                categorie.NumCategorie = 15;

                // Assert
                Assert.AreEqual(15, categorie.NumCategorie, "Le numéro catégorie doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprieteNomCategorie_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();
                bool propertyChangedRaised = false;
                categorie.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "NomCategorie")
                        propertyChangedRaised = true;
                };

                // Act
                categorie.NomCategorie = "informatique";

                // Assert
                Assert.AreEqual("Informatique", categorie.NomCategorie, "Le nom catégorie doit être formaté avec première lettre majuscule");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void NomCategorie_FormatageMinuscules_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NomCategorie = "automobile";

                // Assert
                Assert.AreEqual("Automobile", categorie.NomCategorie, "Le nom doit être formaté : première lettre majuscule, reste minuscule");
            }

            [TestMethod]
            public void NomCategorie_FormatageMajuscules_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NomCategorie = "TEXTILE";

                // Assert
                Assert.AreEqual("Textile", categorie.NomCategorie, "Le nom doit être formaté : première lettre majuscule, reste minuscule");
            }

            [TestMethod]
            public void NomCategorie_FormatageMixte_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NomCategorie = "aLiMeNtAiRe";

                // Assert
                Assert.AreEqual("Alimentaire", categorie.NomCategorie, "Le nom doit être formaté : première lettre majuscule, reste minuscule");
            }

            [TestMethod]
            public void NomCategorie_FormatageAvecEspaces_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NomCategorie = "jeux et jouets";

                // Assert
                Assert.AreEqual("Jeux et jouets", categorie.NomCategorie, "Le nom doit être formaté : première lettre majuscule, reste minuscule");
            }

            [TestMethod]
            public void NomCategorie_FormatageAvecChiffres_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NomCategorie = "high-TECH 2024";

                // Assert
                Assert.AreEqual("High-tech 2024", categorie.NomCategorie, "Le nom doit être formaté : première lettre majuscule, reste minuscule");
            }

            [TestMethod]
            public void NomCategorie_DejaFormate_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NomCategorie = "Sport";

                // Assert
                Assert.AreEqual("Sport", categorie.NomCategorie, "Le nom correctement formaté doit rester inchangé");
            }

            [TestMethod]
            public void PropertyChangedEvent_MultipleProperties_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();
                int propertyChangedCount = 0;
                categorie.PropertyChanged += (sender, e) => propertyChangedCount++;

                // Act
                categorie.NumCategorie = 20;
                categorie.NomCategorie = "mode";

                // Assert
                Assert.AreEqual(2, propertyChangedCount, "PropertyChanged doit être déclenché 2 fois");
            }

            [TestMethod]
            public void CategorieComplet_Test()
            {
                // Arrange & Act
                Categorie categorie = new Categorie(25, "BRICOLAGE");

                // Assert
                Assert.AreEqual(25, categorie.NumCategorie, "Numéro catégorie incorrect");
                Assert.AreEqual("Bricolage", categorie.NomCategorie, "Nom catégorie doit être formaté correctement");
            }

            [TestMethod]
            public void ConstructeurComplet_AvecFormatage_Test()
            {
                // Arrange & Act
                Categorie categorie = new Categorie(30, "maison et jardin");

                // Assert
                Assert.AreEqual(30, categorie.NumCategorie, "Numéro catégorie incorrect");
                Assert.AreEqual("Maison et jardin", categorie.NomCategorie, "Le nom doit être formaté lors de la construction");
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            public void NomCategorie_ChaineVide_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act - Ceci devrait lever une exception car on ne peut pas accéder à value[0] sur une chaîne vide
                categorie.NomCategorie = "";
            }

            [TestMethod]
            public void NomCategorie_UnSeulCaractere_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NomCategorie = "a";

                // Assert
                Assert.AreEqual("A", categorie.NomCategorie, "Un seul caractère doit être mis en majuscule");
            }

            [TestMethod]
            public void MultipleModifications_Test()
            {
                // Arrange
                Categorie categorie = new Categorie();

                // Act
                categorie.NumCategorie = 1;
                categorie.NomCategorie = "test1";
                categorie.NumCategorie = 2;
                categorie.NomCategorie = "TEST2";

                // Assert
                Assert.AreEqual(2, categorie.NumCategorie, "Le numéro final doit être 2");
                Assert.AreEqual("Test2", categorie.NomCategorie, "Le nom final doit être Test2");
            }

        }
    }
}

        