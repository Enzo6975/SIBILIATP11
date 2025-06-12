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
    public class RoleTests
    {
        [TestClass]
        public class RoleTest
        {
            [TestMethod]
            public void RoleConstructorTests()
            {
                // Test constructeur par défaut
                Role roleVide = new Role();
                Assert.AreEqual(0, roleVide.NumRole, "NumRole par défaut doit être 0");
                Assert.IsNull(roleVide.NomRole, "NomRole par défaut doit être null");

                // Test constructeur avec numRole
                Role roleAvecNum = new Role(1);
                Assert.AreEqual(1, roleAvecNum.NumRole, "NumRole doit être égal à 1");
                Assert.IsNull(roleAvecNum.NomRole, "NomRole doit être null");

                // Test constructeur complet
                Role roleComplet = new Role(2, "Administrateur");
                Assert.AreEqual(2, roleComplet.NumRole, "NumRole doit être égal à 2");
                Assert.AreEqual("Administrateur", roleComplet.NomRole, "NomRole doit être égal à 'Administrateur'");
            }

            [TestMethod]
            public void RolePropertiesTests()
            {
                Role role = new Role();

                // Test setter NumRole
                role.NumRole = 5;
                Assert.AreEqual(5, role.NumRole, "NumRole doit être égal à 5");

                // Test setter NomRole
                role.NomRole = "Utilisateur";
                Assert.AreEqual("Utilisateur", role.NomRole, "NomRole doit être égal à 'Utilisateur'");
            }

            [TestMethod]
            public void PropertyChangedEventTests()
            {
                Role role = new Role();
                bool propertyChangedFired = false;
                string propertyName = "";

                // S'abonner à l'événement PropertyChanged
                role.PropertyChanged += (sender, e) =>
                {
                    propertyChangedFired = true;
                    propertyName = e.PropertyName;
                };

                // Test événement pour NumRole
                role.NumRole = 10;
                Assert.IsTrue(propertyChangedFired, "PropertyChanged doit être déclenché pour NumRole");
                Assert.AreEqual("NumRole", propertyName, "PropertyName doit être 'NumRole'");

                // Reset pour le test suivant
                propertyChangedFired = false;
                propertyName = "";

                // Test événement pour NomRole
                role.NomRole = "Manager";
                Assert.IsTrue(propertyChangedFired, "PropertyChanged doit être déclenché pour NomRole");
                Assert.AreEqual("NomRole", propertyName, "PropertyName doit être 'NomRole'");
            }


            [TestMethod]
            public void RoleValidationTests()
            {
                // Test avec des valeurs valides
                Role roleValide = new Role(1, "Superviseur");
                Assert.AreEqual(1, roleValide.NumRole, "NumRole valide");
                Assert.AreEqual("Superviseur", roleValide.NomRole, "NomRole valide");

                // Test avec NomRole contenant des espaces
                Role roleAvecEspaces = new Role(2, "   Chef de projet   ");
                Assert.AreEqual("   Chef de projet   ", roleAvecEspaces.NomRole, "NomRole avec espaces conservé");

                // Test avec caractères spéciaux
                Role roleCaracteresSpeciaux = new Role(3, "Rôle-test_123");
                Assert.AreEqual("Rôle-test_123", roleCaracteresSpeciaux.NomRole, "NomRole avec caractères spéciaux");
            }

            [TestMethod]
            public void RoleEqualityTests()
            {
                Role role1 = new Role(1, "Admin");
                Role role2 = new Role(1, "Admin");
                Role role3 = new Role(2, "Admin");
                Role role4 = new Role(1, "User");

                // Test égalité des propriétés (pas d'override d'Equals dans votre classe)
                Assert.AreEqual(role1.NumRole, role2.NumRole, "NumRole identiques");
                Assert.AreEqual(role1.NomRole, role2.NomRole, "NomRole identiques");

                Assert.AreNotEqual(role1.NumRole, role3.NumRole, "NumRole différents");
                Assert.AreNotEqual(role1.NomRole, role4.NomRole, "NomRole différents");
            }

            [TestMethod]
            public void RoleModificationTests()
            {
                Role role = new Role(1, "Initial");

                // Modification du nom
                role.NomRole = "Modifié";
                Assert.AreEqual("Modifié", role.NomRole, "NomRole modifié");

                // Modification du numéro
                role.NumRole = 99;
                Assert.AreEqual(99, role.NumRole, "NumRole modifié");

                // Test modification multiple
                role.NomRole = "Final";
                role.NumRole = 100;
                Assert.AreEqual("Final", role.NomRole, "NomRole final");
                Assert.AreEqual(100, role.NumRole, "NumRole final");
            }

            [TestMethod]
            public void RoleLongNameTest()
            {
                // Test avec un nom très long
                string nomTresLong = new string('A', 1000);
                Role roleLongNom = new Role(1, nomTresLong);
                Assert.AreEqual(nomTresLong, roleLongNom.NomRole, "NomRole très long accepté");
            }

            [TestMethod]
            public void RoleNegativeNumberTest()
            {
                // Test avec un numéro négatif
                Role roleNegatif = new Role(-1, "Test");
                Assert.AreEqual(-1, roleNegatif.NumRole, "NumRole négatif accepté");
            }
        }
    }
}