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
    public class PeriodeTests
    {
        [TestClass]
        public class PeriodeTest
        {
            [TestMethod]
            public void PeriodeConstructorTests()
            {
                // Test constructeur par défaut
                Periode periodeVide = new Periode();
                Assert.AreEqual(0, periodeVide.NumPeriode, "NumPeriode par défaut doit être 0");
                Assert.IsNull(periodeVide.LibellePeriode, "LibellePeriode par défaut doit être null");

                // Test constructeur avec numPeriode
                Periode periodeAvecNum = new Periode(1);
                Assert.AreEqual(1, periodeAvecNum.NumPeriode, "NumPeriode doit être égal à 1");
                Assert.IsNull(periodeAvecNum.LibellePeriode, "LibellePeriode doit être null");

                // Test constructeur complet
                Periode periodeComplete = new Periode(2, "Période estivale");
                Assert.AreEqual(2, periodeComplete.NumPeriode, "NumPeriode doit être égal à 2");
                Assert.AreEqual("Période estivale", periodeComplete.LibellePeriode, "LibellePeriode doit être égal à 'Période estivale'");
            }

            [TestMethod]
            public void PeriodePropertiesTests()
            {
                Periode periode = new Periode();

                // Test setter NumPeriode
                periode.NumPeriode = 5;
                Assert.AreEqual(5, periode.NumPeriode, "NumPeriode doit être égal à 5");

                // Test setter LibellePeriode
                periode.LibellePeriode = "Période hivernale";
                Assert.AreEqual("Période hivernale", periode.LibellePeriode, "LibellePeriode doit être égal à 'Période hivernale'");
            }

            [TestMethod]
            public void PropertyChangedEventTests()
            {
                Periode periode = new Periode();
                bool propertyChangedFired = false;
                string propertyName = "";

                // S'abonner à l'événement PropertyChanged
                periode.PropertyChanged += (sender, e) =>
                {
                    propertyChangedFired = true;
                    propertyName = e.PropertyName;
                };

                // Test événement pour NumPeriode
                periode.NumPeriode = 10;
                Assert.IsTrue(propertyChangedFired, "PropertyChanged doit être déclenché pour NumPeriode");
                Assert.AreEqual("NumPeriode", propertyName, "PropertyName doit être 'NumPeriode'");

                // Reset pour le test suivant
                propertyChangedFired = false;
                propertyName = "";

                // Test événement pour LibellePeriode
                periode.LibellePeriode = "Période de vacances";
                Assert.IsTrue(propertyChangedFired, "PropertyChanged doit être déclenché pour LibellePeriode");
                Assert.AreEqual("LibellePeriode", propertyName, "PropertyName doit être 'LibellePeriode'");
            }

            [TestMethod]
            public void PeriodeValidationTests()
            {
                // Test avec des valeurs valides
                Periode periodeValide = new Periode(1, "Haute saison");
                Assert.AreEqual(1, periodeValide.NumPeriode, "NumPeriode valide");
                Assert.AreEqual("Haute saison", periodeValide.LibellePeriode, "LibellePeriode valide");

                // Test avec LibellePeriode contenant des espaces
                Periode periodeAvecEspaces = new Periode(2, "   Basse saison   ");
                Assert.AreEqual("   Basse saison   ", periodeAvecEspaces.LibellePeriode, "LibellePeriode avec espaces conservé");

                // Test avec caractères spéciaux et accents
                Periode periodeCaracteresSpeciaux = new Periode(3, "Période été-automne 2025");
                Assert.AreEqual("Période été-automne 2025", periodeCaracteresSpeciaux.LibellePeriode, "LibellePeriode avec caractères spéciaux et accents");
            }

            [TestMethod]
            public void PeriodeEqualityTests()
            {
                Periode periode1 = new Periode(1, "Printemps");
                Periode periode2 = new Periode(1, "Printemps");
                Periode periode3 = new Periode(2, "Printemps");
                Periode periode4 = new Periode(1, "Été");

                // Test égalité des propriétés (pas d'override d'Equals dans votre classe)
                Assert.AreEqual(periode1.NumPeriode, periode2.NumPeriode, "NumPeriode identiques");
                Assert.AreEqual(periode1.LibellePeriode, periode2.LibellePeriode, "LibellePeriode identiques");

                Assert.AreNotEqual(periode1.NumPeriode, periode3.NumPeriode, "NumPeriode différents");
                Assert.AreNotEqual(periode1.LibellePeriode, periode4.LibellePeriode, "LibellePeriode différents");
            }

            [TestMethod]
            public void PeriodeModificationTests()
            {
                Periode periode = new Periode(1, "Initial");

                // Modification du libellé
                periode.LibellePeriode = "Modifié";
                Assert.AreEqual("Modifié", periode.LibellePeriode, "LibellePeriode modifié");

                // Modification du numéro
                periode.NumPeriode = 99;
                Assert.AreEqual(99, periode.NumPeriode, "NumPeriode modifié");

                // Test modification multiple
                periode.LibellePeriode = "Période finale";
                periode.NumPeriode = 100;
                Assert.AreEqual("Période finale", periode.LibellePeriode, "LibellePeriode final");
                Assert.AreEqual(100, periode.NumPeriode, "NumPeriode final");
            }

            [TestMethod]
            public void PeriodeLongLibelleTest()
            {
                // Test avec un libellé très long
                string libelleTresLong = new string('A', 1000);
                Periode periodeLongLibelle = new Periode(1, libelleTresLong);
                Assert.AreEqual(libelleTresLong, periodeLongLibelle.LibellePeriode, "LibellePeriode très long accepté");
            }

            [TestMethod]
            public void PeriodeNegativeNumberTest()
            {
                // Test avec un numéro négatif
                Periode periodeNegatif = new Periode(-1, "Test");
                Assert.AreEqual(-1, periodeNegatif.NumPeriode, "NumPeriode négatif accepté");
            }

            [TestMethod]
            public void PeriodeLibelleSpecialCasesTest()
            {
                // Test avec des cas spéciaux de libellés
                Periode periodeEspaces = new Periode(1, "   ");
                Assert.AreEqual("   ", periodeEspaces.LibellePeriode, "LibellePeriode avec espaces uniquement");

                Periode periodeCaracteresSpeciaux = new Periode(2, "!@#$%^&*()");
                Assert.AreEqual("!@#$%^&*()", periodeCaracteresSpeciaux.LibellePeriode, "LibellePeriode avec caractères spéciaux");

                Periode periodeNombres = new Periode(3, "123456789");
                Assert.AreEqual("123456789", periodeNombres.LibellePeriode, "LibellePeriode avec nombres");

                Periode periodeMixte = new Periode(4, "Période_123!@#");
                Assert.AreEqual("Période_123!@#", periodeMixte.LibellePeriode, "LibellePeriode mixte");
            }

            [TestMethod]
            public void PeriodeTypicalUseCasesTest()
            {
                // Test des cas d'usage typiques pour une période
                Periode periodeEte = new Periode(1, "Été 2025");
                Assert.AreEqual("Été 2025", periodeEte.LibellePeriode, "Période été");

                Periode periodeHiver = new Periode(2, "Hiver 2025-2026");
                Assert.AreEqual("Hiver 2025-2026", periodeHiver.LibellePeriode, "Période hiver");

                Periode periodeVacances = new Periode(3, "Vacances scolaires");
                Assert.AreEqual("Vacances scolaires", periodeVacances.LibellePeriode, "Période vacances");

                Periode periodeHauteSaison = new Periode(4, "Haute saison touristique");
                Assert.AreEqual("Haute saison touristique", periodeHauteSaison.LibellePeriode, "Période haute saison");
            }

            [TestMethod]
            public void PeriodeZeroTest()
            {
                // Test avec NumPeriode à zéro
                Periode periodeZero = new Periode(0, "Test zéro");
                Assert.AreEqual(0, periodeZero.NumPeriode, "NumPeriode zéro accepté");
                Assert.AreEqual("Test zéro", periodeZero.LibellePeriode, "LibellePeriode avec NumPeriode zéro");
            }
        }
    }
}