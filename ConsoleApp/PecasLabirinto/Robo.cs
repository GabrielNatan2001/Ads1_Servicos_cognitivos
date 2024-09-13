using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Robo : Peca
    {
        public Robo(int linha, int coluna)
        {
            this.Linha = linha;
            this.Coluna = coluna;
            IniciandoVisao();
        }
        public int Linha { get; private set; }
        public int Coluna { get; private set; }
        private bool EncontrouHumano { get; set; } = false;

        public int VisaoLinha { get; private set; }
        public int VisaoColuna { get; private set; }
        public BuscaAStar AStart {  get; set; }

        private void IniciandoVisao()
        {
           
        }
        public void MoverParaFrente()
        {
            Linha = Linha + 1;
        }
        public void GirarParaDireita()
        {
            VisaoLinha = VisaoLinha - 1;
            VisaoColuna = VisaoColuna + 1;
        }

        public void PegarHumano()
        {
            
        }
        public void EjetarHumano()
        {

        }
        public void Log(string movimento)
        {

        }
        public void IniciarBusca(Peca[,] mapa, Entrada entrada, Humano humano)
        {
            var astar = new BuscaAStar(mapa);

            var caminho = astar.BuscarCaminhoAStar(entrada.Linha, entrada.Coluna, humano.Linha, humano.Coluna);
            Console.WriteLine("\nCaminho Encontrado:");
            if (caminho.Count > 0)
            {
                foreach (var pos in caminho)
                {
                    Console.WriteLine($"({pos.Item1}, {pos.Item2})");
                }
            }
            else
            {
                Console.WriteLine("Nenhum caminho encontrado.");
            }
        }
    }
}
