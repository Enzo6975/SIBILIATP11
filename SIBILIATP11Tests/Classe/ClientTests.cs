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
    public class ClientTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "telephone invalide")]
        public void ClientTests_numeroDeTelPasValide()
        {

            Client c = new Client(56, "Claude", "Claude", "07070707", "7 rue de la rue", "07070", "Ville");
        }

    }
}