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
        [TestMethod()]
        public void CategorieTest()
        {
            Categorie c = new Categorie(54, "plat chaud");
            Assert.AreEqual("Plat chaud", c.NomCategorie);
        }

    }
}

        