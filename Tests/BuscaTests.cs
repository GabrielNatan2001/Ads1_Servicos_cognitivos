using ConsoleApp.PecasLabirinto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class BuscaAStarTests
    {
        [Test]
        public void TestBuscarCaminhoAStar_SemObstaculos()
        {
            // Arrange
            var mapa = new Peca[,]
            {
                { new Caminho(), new Caminho(), new Caminho() },
                { new Caminho(), new Caminho(), new Caminho() },
                { new Caminho(), new Caminho(), new Caminho() }
            };
            var busca = new BuscaAStar(mapa);

            // Act
            var caminho = busca.BuscarCaminhoAStar(0, 0, 2, 2);

            // Assert
            var caminhoEsperado = new List<(int, int)>
            {
                (0, 0),
                (1, 0),
                (1, 1),
                (1, 2),
                (2, 2)
            };

            Assert.AreEqual(caminhoEsperado, caminho);
        }

        [Test]
        public void TestBuscarCaminhoAStar_ComObstaculos()
        {
            // Arrange
            var mapa = new Peca[,]
            {
                { new Caminho(), new Parede(), new Caminho() },
                { new Caminho(), new Parede(), new Caminho() },
                { new Caminho(), new Caminho(), new Caminho() }
            };
            var busca = new BuscaAStar(mapa);

            // Act
            var caminho = busca.BuscarCaminhoAStar(0, 0, 2, 2);

            // Assert
            var caminhoEsperado = new List<(int, int)>
            {
                (0, 0),
                (1, 0),
                (2, 0),
                (2, 1),
                (2, 2)
            };

            Assert.AreEqual(caminhoEsperado, caminho);
        }

        [Test]
        public void TestBuscarCaminhoAStar_SemCaminho()
        {
            // Arrange
            var mapa = new Peca[,]
            {
                { new Parede(), new Parede(), new Parede() },
                { new Parede(), new Parede(), new Parede() },
                { new Parede(), new Parede(), new Parede() }
            };
            var busca = new BuscaAStar(mapa);

            // Act
            var caminho = busca.BuscarCaminhoAStar(0, 0, 2, 2);

            // Assert
            Assert.IsEmpty(caminho);
        }
    }
}
