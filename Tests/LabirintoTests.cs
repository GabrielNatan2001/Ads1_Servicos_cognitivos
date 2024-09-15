using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class LabirintoTests
    {
        private const string MapaTeste = "mapa_teste.txt";

        [SetUp]
        public void Setup()
        {
            // Criar um arquivo de mapa de teste
            var linhas = new[]
            {
            "* * *",
            "* E *",
            "* H *",
            "* * *"
        };
            File.WriteAllLines(MapaTeste, linhas);
        }

        [TearDown]
        public void Teardown()
        {
            // Limpar o arquivo de mapa de teste após os testes
            if (File.Exists(MapaTeste))
            {
                File.Delete(MapaTeste);
            }
        }

        [Test]
        public void TestCriarMapa()
        {
            var labirinto = new Labirinto();
            labirinto.CriarMapa(MapaTeste);

            Assert.IsNotNull(labirinto.mapa);
            Assert.AreEqual(4, labirinto.mapa.GetLength(0)); // 4 linhas
            Assert.AreEqual(5, labirinto.mapa.GetLength(1)); // 5 colunas
        }

        [Test]
        public void TestPopularMapa()
        {
            var labirinto = new Labirinto();
            labirinto.CriarMapa(MapaTeste);

            Assert.IsNotNull(labirinto.entrada);
            Assert.IsNotNull(labirinto.robo);
            Assert.IsNotNull(labirinto.humano);

            Assert.AreEqual(1, labirinto.entrada.Linha);
            Assert.AreEqual(2, labirinto.entrada.Coluna);
            Assert.AreEqual(1, labirinto.robo.Linha);
            Assert.AreEqual(2, labirinto.robo.Coluna);
            Assert.AreEqual(2, labirinto.humano.Linha);
            Assert.AreEqual(2, labirinto.humano.Coluna);
        }

        [Test]
        public void TestDesenhaMapa()
        {
            var labirinto = new Labirinto();
            labirinto.CriarMapa(MapaTeste);

            // Captura a saída do console
            var sb = new StringBuilder();
            var originalConsoleOut = Console.Out;
            using (var consoleOutput = new StringWriter(sb))
            {
                Console.SetOut(consoleOutput);
                labirinto.DesenhaMapa();
                Console.SetOut(originalConsoleOut);
            }

            var resultadoEsperado = "* * *\n* E *\n* H *\n* * *\n";
            Assert.AreEqual(resultadoEsperado, sb.ToString());
        }
    }
}
