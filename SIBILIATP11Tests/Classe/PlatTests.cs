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
            [TestClass]
            public class PlatTests
            {
                [TestMethod]
                public void Constructeur_Complet()
                {
                    var cat = new Categorie(1, "Plats principaux");
                    var sousCat = new SousCategorie(1, "Viandes", cat);
                    var periode = new Periode(1, "Été");
                    var plat = new Plat(1, "Boeuf bourguignon", 25.50, 45, 4, sousCat, periode);

                    Assert.AreEqual(1, plat.NumPlat);
                    Assert.AreEqual("Boeuf bourguignon", plat.NomPlat);
                    Assert.AreEqual(25.50, plat.PrixUnitaire, 0.01);
                    Assert.AreEqual(45, plat.DelaiPreparation);
                    Assert.AreEqual(4, plat.NbPersonnes);
                    Assert.AreEqual(sousCat, plat.UneSousCategorie);
                    Assert.AreEqual(periode, plat.UnePeriode);
                }

                [TestMethod]
                public void Constructeur_Vide()
                {
                    var plat = new Plat();

                    Assert.AreEqual(0, plat.NumPlat);
                    Assert.IsNull(plat.NomPlat);
                    Assert.AreEqual(0.0, plat.PrixUnitaire, 0.01);
                    Assert.AreEqual(0, plat.DelaiPreparation);
                    Assert.AreEqual(0, plat.NbPersonnes);
                    Assert.IsNull(plat.UneSousCategorie);
                    Assert.IsNull(plat.UnePeriode);
                }

                [TestMethod]
                public void Constructeur_NumPlatSeulement()
                {
                    var plat = new Plat(5);

                    Assert.AreEqual(5, plat.NumPlat);
                    Assert.IsNull(plat.NomPlat);
                    Assert.AreEqual(0.0, plat.PrixUnitaire, 0.01);
                }

                [TestMethod]
                public void Modification_Proprietes()
                {
                    var plat = new Plat();
                    bool propertyChanged = false;
                    plat.PropertyChanged += (s, e) => propertyChanged = true;

                    plat.NomPlat = "Coq au vin";
                    Assert.AreEqual("Coq au vin", plat.NomPlat);
                    Assert.IsTrue(propertyChanged);

                    propertyChanged = false;
                    plat.PrixUnitaire = 18.75;
                    Assert.AreEqual(18.75, plat.PrixUnitaire, 0.01);
                    Assert.IsTrue(propertyChanged);
                }
            }
        }
    }
}