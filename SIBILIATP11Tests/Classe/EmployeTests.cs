using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIBILIATP11.Classe;
using System;

namespace SIBILIATP11.Classe.Tests
{
    [TestClass()]
    public class EmployeTests
    {
        [TestMethod]
        public void ProprieteNomEmploye_PropertyChanged_Test()
        {
            Employe employe = new Employe();
            bool propertyChangedRaised = false;
            employe.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "NomEmploye")
                    propertyChangedRaised = true;
            };
            employe.NomEmploye = "Martin";
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void ProprietePrenomEmploye_PropertyChanged_Test()
        {
            Employe employe = new Employe();
            bool propertyChangedRaised = false;
            employe.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "PrenomEmploye")
                    propertyChangedRaised = true;
            };
            employe.PrenomEmploye = "Marie";
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void ProprietePassword_PropertyChanged_Test()
        {
            Employe employe = new Employe();
            bool propertyChangedRaised = false;
            employe.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Password")
                    propertyChangedRaised = true;
            };
            employe.Password = "motDePasse";
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void ProprieteLogin_PropertyChanged_Test()
        {
            Employe employe = new Employe();
            bool propertyChangedRaised = false;
            employe.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Login")
                    propertyChangedRaised = true;
            };
            employe.Login = "utilisateur";
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void ProprieteUnRole_PropertyChanged_Test()
        {
            Employe employe = new Employe();
            Role role = new Role(1, "Manager");
            bool propertyChangedRaised = false;
            employe.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "UnRole")
                    propertyChangedRaised = true;
            };
            employe.UnRole = role;
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void PropertyChanged_MultipleProperties_Test()
        {
            Employe employe = new Employe();
            Role role = new Role(1, "Employé");
            int propertyChangedCount = 0;
            employe.PropertyChanged += (sender, e) => propertyChangedCount++;

            employe.NomEmploye = "Durand";
            employe.PrenomEmploye = "Pierre";
            employe.Password = "password123";
            employe.Login = "pdurand";
            employe.UnRole = role;

            Assert.AreEqual(5, propertyChangedCount);
        }

        [TestMethod]
        public void NumEmploye_PropertyChanged_NonDeclenche_Test()
        {
            Employe employe = new Employe();
            bool propertyChangedRaised = false;
            employe.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "NumEmploye")
                    propertyChangedRaised = true;
            };
            employe.NumEmploye = 99;
            Assert.IsFalse(propertyChangedRaised);
        }
    }
}
