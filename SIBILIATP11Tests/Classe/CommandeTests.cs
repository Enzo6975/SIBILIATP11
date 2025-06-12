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
    public class CommandeTests
    {
        [TestClass]
        public class CommandeTest
        {
            [TestMethod]
            public void CommandeTests()
            {
                // Arrange - Création des objets de test
                Role roleTest = new Role(1, "Manager");
                Employe employeTest = new Employe(1, "Dupont", "Jean", "password123", "jdupont", roleTest);
                Client clientTest = new Client(1, "Martin", "Pierre", "0123456789", "123 Rue Test", "75001", "Paris");

                // Act - Création de la commande
                Commande commandeTest = new Commande(
                    numCommande: 1,
                    dateCommande: new DateTime(2025, 06, 15),
                    dateRetraitPrevue: new DateTime(2025, 06, 20),
                    payee: false,
                    retiree: false,
                    prixTotal: 150.50,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                // Assert - Vérifications
                Assert.AreEqual(1, commandeTest.NumCommande, "numero commande egale");
                Assert.AreEqual(new DateTime(2025, 06, 15), commandeTest.DateCommande, "date commande egale");
                Assert.AreEqual(new DateTime(2025, 06, 20), commandeTest.DateRetraitPrevue, "date retrait egale");
                Assert.AreEqual(false, commandeTest.Payee, "commande non payee");
                Assert.AreEqual(false, commandeTest.Retiree, "commande non retiree");
                Assert.AreEqual(150.50, commandeTest.PrixTotal, "prix total egale");
                Assert.AreEqual(employeTest, commandeTest.UnEmploye, "employe egale");
                Assert.AreEqual(clientTest, commandeTest.UnClient, "client egale");
            }

            [TestMethod]
            [ExpectedException(typeof(NullReferenceException), "employe vide")]
            public void CommandeTest_EmployeVide()
            {
                Client clientTest = new Client(1, "Martin", "Pierre", "0123456789");
                Commande commandeTest = new Commande(
                    numCommande: 1,
                    dateCommande: new DateTime(2025, 06, 15),
                    dateRetraitPrevue: new DateTime(2025, 06, 20),
                    payee: false,
                    retiree: false,
                    prixTotal: 150.50,
                    unEmploye: null,
                    unClient: clientTest
                );

                // Cette ligne devrait lever une exception lors de l'accès à Create()
                commandeTest.Create();
            }

            [TestMethod]
            [ExpectedException(typeof(NullReferenceException), "client vide")]
            public void CommandeTest_ClientVide()
            {
                Role roleTest = new Role(1, "Manager");
                Employe employeTest = new Employe(1, "Dupont", "Jean", "password123", "jdupont", roleTest);

                Commande commandeTest = new Commande(
                    numCommande: 1,
                    dateCommande: new DateTime(2025, 06, 15),
                    dateRetraitPrevue: new DateTime(2025, 06, 20),
                    payee: false,
                    retiree: false,
                    prixTotal: 150.50,
                    unEmploye: employeTest,
                    unClient: null
                );

                // Cette ligne devrait lever une exception lors de l'accès à Create()
                commandeTest.Create();
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException), "erreur mise a jour")]
            public void CommandeTest_UpdateAvecErreur()
            {
                // Commande avec des données qui peuvent causer une erreur de mise à jour
                Role roleTest = new Role(999, "RoleInexistant"); // ID inexistant
                Employe employeTest = new Employe(999, "Inexistant", "User", "pass", "login", roleTest);
                Client clientTest = new Client(999, "ClientInexistant", "Test", "0000000000");

                Commande commandeTest = new Commande(
                    numCommande: 999999, // ID inexistant
                    dateCommande: new DateTime(2025, 06, 15),
                    dateRetraitPrevue: new DateTime(2025, 06, 20),
                    payee: false,
                    retiree: false,
                    prixTotal: -999.99, // Prix négatif pour forcer une erreur potentielle
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                // Cette ligne devrait lever une exception
                commandeTest.Update();
            }

            [TestMethod]
            public void PropertyChangedTest()
            {
                // Arrange
                Role roleTest = new Role(1, "Manager");
                Employe employeTest = new Employe(1, "Dupont", "Jean", "password123", "jdupont", roleTest);
                Client clientTest = new Client(1, "Martin", "Pierre", "0123456789");

                Commande commandeTest = new Commande(
                    numCommande: 1,
                    dateCommande: new DateTime(2025, 06, 15),
                    dateRetraitPrevue: new DateTime(2025, 06, 20),
                    payee: false,
                    retiree: false,
                    prixTotal: 150.50,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                bool eventRaised = false;
                commandeTest.PropertyChanged += (sender, e) => eventRaised = true;

                // Act - Modification d'une propriété
                commandeTest.PrixTotal = 200.75;

                // Assert
                Assert.IsTrue(eventRaised, "evenement PropertyChanged leve");
                Assert.AreEqual(200.75, commandeTest.PrixTotal, "nouveau prix applique");
            }

            [TestMethod]
            public void ConstructeurVideTest()
            {
                // Act
                Commande commandeVide = new Commande();

                // Assert
                Assert.AreEqual(0, commandeVide.NumCommande, "numero commande par defaut");
                Assert.AreEqual(0, commandeVide.PrixTotal, "prix total par defaut");
                Assert.IsFalse(commandeVide.Payee, "payee par defaut");
                Assert.IsFalse(commandeVide.Retiree, "retiree par defaut");
            }

            [TestMethod]
            public void ConstructeurAvecNumeroTest()
            {
                // Act
                Commande commandeNumero = new Commande(123);

                // Assert
                Assert.AreEqual(123, commandeNumero.NumCommande, "numero commande initialise");
            }

            [TestMethod]
            public void StatutCommandeTest()
            {
                // Test des différents statuts d'une commande
                Role roleTest = new Role(1, "Vendeur");
                Employe employeTest = new Employe(1, "Durand", "Marie", "pass456", "mdurand", roleTest);
                Client clientTest = new Client(1, "Dubois", "Paul", "0987654321");

                // Commande en attente
                Commande commandeEnAttente = new Commande(
                    numCommande: 1,
                    dateCommande: new DateTime(2025, 06, 15),
                    dateRetraitPrevue: new DateTime(2025, 06, 20),
                    payee: false,
                    retiree: false,
                    prixTotal: 75.25,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                // Commande payée mais non retirée
                Commande commandePayee = new Commande(
                    numCommande: 2,
                    dateCommande: new DateTime(2025, 06, 10),
                    dateRetraitPrevue: new DateTime(2025, 06, 15),
                    payee: true,
                    retiree: false,
                    prixTotal: 125.00,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                // Commande terminée
                Commande commandeTerminee = new Commande(
                    numCommande: 3,
                    dateCommande: new DateTime(2025, 06, 05),
                    dateRetraitPrevue: new DateTime(2025, 06, 10),
                    payee: true,
                    retiree: true,
                    prixTotal: 299.99,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                // Assert
                Assert.IsFalse(commandeEnAttente.Payee, "commande en attente non payee");
                Assert.IsFalse(commandeEnAttente.Retiree, "commande en attente non retiree");

                Assert.IsTrue(commandePayee.Payee, "commande payee");
                Assert.IsFalse(commandePayee.Retiree, "commande payee non retiree");

                Assert.IsTrue(commandeTerminee.Payee, "commande terminee payee");
                Assert.IsTrue(commandeTerminee.Retiree, "commande terminee retiree");
            }

            [TestMethod]
            public void PrixTotalTest()
            {
                // Test avec différents prix
                Role roleTest = new Role(1, "Caissier");
                Employe employeTest = new Employe(1, "Moreau", "Luc", "pass789", "lmoreau", roleTest);
                Client clientTest = new Client(1, "Bernard", "Sophie", "0123456789");

                Commande commandePetitPrix = new Commande(
                    numCommande: 1,
                    dateCommande: DateTime.Now,
                    dateRetraitPrevue: DateTime.Now.AddDays(3),
                    payee: false,
                    retiree: false,
                    prixTotal: 5.99,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                Commande commandeGrandPrix = new Commande(
                    numCommande: 2,
                    dateCommande: DateTime.Now,
                    dateRetraitPrevue: DateTime.Now.AddDays(7),
                    payee: false,
                    retiree: false,
                    prixTotal: 1599.99,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                Commande commandePrixZero = new Commande(
                    numCommande: 3,
                    dateCommande: DateTime.Now,
                    dateRetraitPrevue: DateTime.Now.AddDays(1),
                    payee: true,
                    retiree: false,
                    prixTotal: 0.00,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                // Assert
                Assert.AreEqual(5.99, commandePetitPrix.PrixTotal, "petit prix correct");
                Assert.AreEqual(1599.99, commandeGrandPrix.PrixTotal, "grand prix correct");
                Assert.AreEqual(0.00, commandePrixZero.PrixTotal, "prix zero correct");
            }

            [TestMethod]
            public void DatesCommandeTest()
            {
                // Test avec différentes combinaisons de dates
                Role roleTest = new Role(1, "Gestionnaire");
                Employe employeTest = new Employe(1, "Petit", "Anne", "pass000", "apetit", roleTest);
                Client clientTest = new Client(1, "Robert", "Michel", "0147258369");

                DateTime dateCommande = new DateTime(2025, 07, 01);
                DateTime dateRetrait1Jour = dateCommande.AddDays(1);
                DateTime dateRetrait7Jours = dateCommande.AddDays(7);
                DateTime dateRetrait30Jours = dateCommande.AddDays(30);

                Commande commandeRapide = new Commande(
                    numCommande: 1,
                    dateCommande: dateCommande,
                    dateRetraitPrevue: dateRetrait1Jour,
                    payee: true,
                    retiree: false,
                    prixTotal: 50.00,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                Commande commandeNormale = new Commande(
                    numCommande: 2,
                    dateCommande: dateCommande,
                    dateRetraitPrevue: dateRetrait7Jours,
                    payee: false,
                    retiree: false,
                    prixTotal: 150.00,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                Commande commandeLongue = new Commande(
                    numCommande: 3,
                    dateCommande: dateCommande,
                    dateRetraitPrevue: dateRetrait30Jours,
                    payee: false,
                    retiree: false,
                    prixTotal: 500.00,
                    unEmploye: employeTest,
                    unClient: clientTest
                );

                // Assert
                Assert.AreEqual(dateCommande, commandeRapide.DateCommande, "date commande rapide");
                Assert.AreEqual(dateRetrait1Jour, commandeRapide.DateRetraitPrevue, "date retrait 1 jour");

                Assert.AreEqual(dateCommande, commandeNormale.DateCommande, "date commande normale");
                Assert.AreEqual(dateRetrait7Jours, commandeNormale.DateRetraitPrevue, "date retrait 7 jours");

                Assert.AreEqual(dateCommande, commandeLongue.DateCommande, "date commande longue");
                Assert.AreEqual(dateRetrait30Jours, commandeLongue.DateRetraitPrevue, "date retrait 30 jours");
            }
        }
    }
}