using ConsoleApp.PecasLabirinto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class HumanoTests
    {
        [Test]
        public void TestConstrutor()
        {
            var humano = new Humano(5, 10);
            Assert.AreEqual(5, humano.Linha);
            Assert.AreEqual(10, humano.Coluna);
            Assert.IsFalse(humano.Coletado);
        }

        [Test]
        public void TestColetadoPeloRobo()
        {
            var humano = new Humano(5, 10);
            humano.ColetadoPeloRobo();
            Assert.IsTrue(humano.Coletado);
        }

        [Test]
        public void TestEjetado()
        {
            var humano = new Humano(5, 10);
            humano.ColetadoPeloRobo(); // Primeiro, marque como coletado
            humano.Ejetado(7, 12);

            Assert.IsFalse(humano.Coletado);
            Assert.AreEqual(7, humano.Linha);
            Assert.AreEqual(12, humano.Coluna);
        }

        [Test]
        public void TestAlterarPosicao()
        {
            var humano = new Humano(5, 10);
            humano.AlterarPosicao(8, 15);

            Assert.AreEqual(8, humano.Linha);
            Assert.AreEqual(15, humano.Coluna);
        }
    }
}
