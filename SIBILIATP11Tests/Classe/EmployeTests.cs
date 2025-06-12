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
    public class EmployeTests
    {
        [TestClass]
        public class EmployeTest
        {
            [TestMethod]
            public void EmployeConstructeurVide_Test()
            {
                // Arrange & Act
                Employe employe = new Employe();

                // Assert
                Assert.AreEqual(0, employe.NumEmploye, "Numéro employé doit être 0 par défaut");
                Assert.IsNull(employe.NomEmploye, "Nom employé doit être null par défaut");
                Assert.IsNull(employe.PrenomEmploye, "Prénom employé doit être null par défaut");
                Assert.IsNull(employe.Password, "Password doit être null par défaut");
                Assert.IsNull(employe.Login, "Login doit être null par défaut");
                Assert.IsNull(employe.UnRole, "Role doit être null par défaut");
            }

            [TestMethod]
            public void EmployeConstructeurAvecNumero_Test()
            {
                // Arrange & Act
                Employe employe = new Employe(25);

                // Assert
                Assert.AreEqual(25, employe.NumEmploye, "Numéro employé doit être 25");
            }

            [TestMethod]
            public void EmployeConstructeurComplet_Test()
            {
                // Arrange
                Role role = new Role(1, "Administrateur");

                // Act
                Employe employe = new Employe(10, "Dupont", "Jean", "motdepasse123", "jdupont", role);

                // Assert
                Assert.AreEqual(10, employe.NumEmploye, "Numéro employé doit être 10");
                Assert.AreEqual("Dupont", employe.NomEmploye, "Nom employé doit être Dupont");
                Assert.AreEqual("Jean", employe.PrenomEmploye, "Prénom employé doit être Jean");
                Assert.AreEqual("motdepasse123", employe.Password, "Password doit être motdepasse123");
                Assert.AreEqual("jdupont", employe.Login, "Login doit être jdupont");
                Assert.AreEqual(role, employe.UnRole, "Role doit être celui passé");
            }

            [TestMethod]
            public void ProprieteNumEmploye_Test()
            {
                // Arrange
                Employe employe = new Employe();

                // Act
                employe.NumEmploye = 50;

                // Assert
                Assert.AreEqual(50, employe.NumEmploye, "Le numéro employé doit être mis à jour");
            }

            [TestMethod]
            public void ProprieteNomEmploye_Test()
            {
                // Arrange
                Employe employe = new Employe();
                bool propertyChangedRaised = false;
                employe.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "NomEmploye")
                        propertyChangedRaised = true;
                };

                // Act
                employe.NomEmploye = "Martin";

                // Assert
                Assert.AreEqual("Martin", employe.NomEmploye, "Le nom employé doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprietePrenomEmploye_Test()
            {
                // Arrange
                Employe employe = new Employe();
                bool propertyChangedRaised = false;
                employe.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "PrenomEmploye")
                        propertyChangedRaised = true;
                };

                // Act
                employe.PrenomEmploye = "Marie";

                // Assert
                Assert.AreEqual("Marie", employe.PrenomEmploye, "Le prénom employé doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprietePassword_Test()
            {
                // Arrange
                Employe employe = new Employe();
                bool propertyChangedRaised = false;
                employe.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Password")
                        propertyChangedRaised = true;
                };

                // Act
                employe.Password = "nouveauMotDePasse";

                // Assert
                Assert.AreEqual("nouveauMotDePasse", employe.Password, "Le password doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprieteLogin_Test()
            {
                // Arrange
                Employe employe = new Employe();
                bool propertyChangedRaised = false;
                employe.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Login")
                        propertyChangedRaised = true;
                };

                // Act
                employe.Login = "utilisateur1";

                // Assert
                Assert.AreEqual("utilisateur1", employe.Login, "Le login doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void ProprieteUnRole_Test()
            {
                // Arrange
                Employe employe = new Employe();
                Role role = new Role(5, "Manager");
                bool propertyChangedRaised = false;
                employe.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "UnRole")
                        propertyChangedRaised = true;
                };

                // Act
                employe.UnRole = role;

                // Assert
                Assert.AreEqual(role, employe.UnRole, "Le role doit être mis à jour");
                Assert.IsTrue(propertyChangedRaised, "L'événement PropertyChanged doit être déclenché");
            }

            [TestMethod]
            public void PropertyChangedEvent_MultipleProperties_Test()
            {
                // Arrange
                Employe employe = new Employe();
                Role role = new Role(1, "Employé");
                int propertyChangedCount = 0;
                employe.PropertyChanged += (sender, e) => propertyChangedCount++;

                // Act
                employe.NomEmploye = "Durand";
                employe.PrenomEmploye = "Pierre";
                employe.Password = "password123";
                employe.Login = "pdurand";
                employe.UnRole = role;

                // Assert
                Assert.AreEqual(5, propertyChangedCount, "PropertyChanged doit être déclenché 5 fois");
            }

            [TestMethod]
            public void PropertyChangedEvent_NumEmployeNonDeclenche_Test()
            {
                // Arrange
                Employe employe = new Employe();
                bool propertyChangedRaised = false;
                employe.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "NumEmploye")
                        propertyChangedRaised = true;
                };

                // Act
                employe.NumEmploye = 100;

                // Assert
                Assert.IsFalse(propertyChangedRaised, "PropertyChanged ne doit PAS être déclenché pour NumEmploye");
            }

            [TestMethod]
            public void EmployeComplet_TodosLesChamps_Test()
            {
                // Arrange
                Role roleAdmin = new Role(2, "Administrateur");

                // Act
                Employe employe = new Employe(15, "Leblanc", "Sophie", "securePass456", "sleblanc", roleAdmin);

                // Assert
                Assert.AreEqual(15, employe.NumEmploye, "Numéro employé incorrect");
                Assert.AreEqual("Leblanc", employe.NomEmploye, "Nom employé incorrect");
                Assert.AreEqual("Sophie", employe.PrenomEmploye, "Prénom employé incorrect");
                Assert.AreEqual("securePass456", employe.Password, "Password incorrect");
                Assert.AreEqual("sleblanc", employe.Login, "Login incorrect");
                Assert.AreEqual(roleAdmin, employe.UnRole, "Role incorrect");
            }

            [TestMethod]
            public void ModificationRole_Test()
            {
                // Arrange
                Role roleInitial = new Role(1, "Employé");
                Role roleNouveau = new Role(2, "Manager");
                Employe employe = new Employe(20, "Test", "Test", "test", "test", roleInitial);

                // Act
                employe.UnRole = roleNouveau;

                // Assert
                Assert.AreEqual(roleNouveau, employe.UnRole, "Le nouveau role doit être assigné");
                Assert.AreNotEqual(roleInitial, employe.UnRole, "L'ancien role ne doit plus être assigné");
            }

            [TestMethod]
            public void ModificationMotDePasse_Test()
            {
                // Arrange
                Role role = new Role(1, "Employé");
                Employe employe = new Employe(30, "Nom", "Prenom", "ancienMDP", "login", role);

                // Act
                employe.Password = "nouveauMDP";

                // Assert
                Assert.AreEqual("nouveauMDP", employe.Password, "Le nouveau mot de passe doit être assigné");
                Assert.AreNotEqual("ancienMDP", employe.Password, "L'ancien mot de passe ne doit plus être présent");
            }

            [TestMethod]
            public void ModificationLogin_Test()
            {
                // Arrange
                Role role = new Role(1, "Employé");
                Employe employe = new Employe(40, "Nom", "Prenom", "mdp", "ancienLogin", role);

                // Act
                employe.Login = "nouveauLogin";

                // Assert
                Assert.AreEqual("nouveauLogin", employe.Login, "Le nouveau login doit être assigné");
                Assert.AreNotEqual("ancienLogin", employe.Login, "L'ancien login ne doit plus être présent");
            }

            [TestMethod]
            public void ModificationsSuccessives_Test()
            {
                // Arrange
                Employe employe = new Employe();
                Role role1 = new Role(1, "Employé");
                Role role2 = new Role(2, "Manager");

                // Act
                employe.NumEmploye = 1;
                employe.NomEmploye = "Nom1";
                employe.PrenomEmploye = "Prenom1";
                employe.Password = "pass1";
                employe.Login = "login1";
                employe.UnRole = role1;

                employe.NumEmploye = 2;
                employe.NomEmploye = "Nom2";
                employe.PrenomEmploye = "Prenom2";
                employe.Password = "pass2";
                employe.Login = "login2";
                employe.UnRole = role2;

                // Assert
                Assert.AreEqual(2, employe.NumEmploye, "Numéro final incorrect");
                Assert.AreEqual("Nom2", employe.NomEmploye, "Nom final incorrect");
                Assert.AreEqual("Prenom2", employe.PrenomEmploye, "Prénom final incorrect");
                Assert.AreEqual("pass2", employe.Password, "Password final incorrect");
                Assert.AreEqual("login2", employe.Login, "Login final incorrect");
                Assert.AreEqual(role2, employe.UnRole, "Role final incorrect");
            }
        }
    }
}