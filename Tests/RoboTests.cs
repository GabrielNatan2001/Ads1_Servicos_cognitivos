using ConsoleApp.Enums;
using ConsoleApp.PecasLabirinto;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class RoboTests
    {
        [Test]
        public void TestIniciandoVisao_BordaSuperior()
        {
            var robo = new Robo(0, 5, 10, 10);
            Assert.AreEqual((int)EVisao.Sul, robo.Visao);
        }

        [Test]
        public void TestIniciandoVisao_BordaInferior()
        {
            var robo = new Robo(9, 5, 10, 10);
            Assert.AreEqual((int)EVisao.Norte, robo.Visao);
        }

        [Test]
        public void TestIniciandoVisao_BordaEsquerda()
        {
            var robo = new Robo(5, 0, 10, 10);
            Assert.AreEqual((int)EVisao.Leste, robo.Visao);
        }

        [Test]
        public void TestIniciandoVisao_BordaDireita()
        {
            var robo = new Robo(5, 9, 10, 10);
            Assert.AreEqual((int)EVisao.Oeste, robo.Visao);
        }

        [Test]
        public void TestAndar_Valido()
        {
            var robo = new Robo(0, 0, 10, 10);
            var mapa = new Peca[10, 10];
            mapa[1, 1] = new Caminho();

            Assert.DoesNotThrow(() => robo.Andar(1, 1, mapa));
            Assert.AreEqual(1, robo.Linha);
            Assert.AreEqual(1, robo.Coluna);
        }

        [Test]
        public void TestAndar_Invalido()
        {
            var robo = new Robo(0, 0, 10, 10);
            var mapa = new Peca[10, 10];
            mapa[1, 1] = new Parede();

            Assert.Throws<Exception>(() => robo.Andar(1, 1, mapa));
        }

        [Test]
        public void TestGirarParaDireita()
        {
            var robo = new Robo(0, 0, 10, 10);

            Assert.AreEqual((int)EVisao.Sul, robo.Visao);

            robo.GirarParaDireita();
            Assert.AreEqual((int)EVisao.Oeste, robo.Visao);

            robo.GirarParaDireita();
            Assert.AreEqual((int)EVisao.Norte, robo.Visao);

            robo.GirarParaDireita();
            Assert.AreEqual((int)EVisao.Leste, robo.Visao);
        }

        [Test]
        public void TestPegarHumano_Valido()
        {
            var robo = new Robo(2, 1, 10, 10);
            var humano = new Humano(1, 1);

            Assert.DoesNotThrow(() => robo.PegarHumano(humano));
            Assert.IsTrue(robo.EncontrouHumano);
        }

        [Test]
        public void TestPegarHumano_Invalido()
        {
            var robo = new Robo(1, 1, 10, 10);
            var humano = new Humano(2, 2);

            Assert.Throws<Exception>(() => robo.PegarHumano(humano));
        }

        [Test]
        public void TestEjetarHumano_Valido()
        {
            var robo = new Robo(2, 1, 10, 10);
            var humano = new Humano(1, 1);

            robo.PegarHumano(humano);
            Assert.DoesNotThrow(() => robo.EjetarHumano());
            Assert.IsFalse(robo.EncontrouHumano);
        }

        [Test]
        public void TestEjetarHumano_Invalido()
        {
            var robo = new Robo(1, 1, 10, 10);

            Assert.Throws<Exception>(() => robo.EjetarHumano());
        }

        [Test]
        public void TestExportarLog()
        {
            var robo = new Robo(1, 1, 10, 10);
            robo.Log("A");
            robo.Log("G");
            robo.Log("P");

            var arquivo = "test_log.csv";
            if (File.Exists(arquivo))
            {
                File.Delete(arquivo);
            }

            robo.ExportarLog(arquivo);
            Assert.IsTrue(File.Exists(arquivo));

            var conteudo = File.ReadAllText(arquivo);
            Assert.AreEqual("A,G,P", conteudo.Trim());
        }

        [Test]
        public void TestIniciarBusca_Simples()
        {
            var robo = new Robo(1, 1, 4, 3); 
            var entrada = new Entrada(1, 1); 
            var humano = new Humano(2, 1); 
            var mapa = new Peca[4, 3];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapa[i, j] = new Caminho(); 
                }
            }
            mapa[1, 1] = entrada;
            mapa[2, 1] = humano; 

            var buscaAStar = new BuscaAStar(mapa);

            robo.AStart = buscaAStar;

             robo.IniciarBusca(mapa, entrada, humano, "test_busca.csv");

            Assert.IsTrue(robo.MovimentosRealizados.Contains("G")); 
            Assert.IsTrue(robo.MovimentosRealizados.Contains("P")); 
            Assert.IsTrue(robo.MovimentosRealizados.Contains("E")); 

            Assert.AreEqual(entrada.Linha, robo.Linha); 
            Assert.AreEqual(entrada.Coluna, robo.Coluna); 
            Assert.IsFalse(humano.Coletado); 
        }

    }
}
