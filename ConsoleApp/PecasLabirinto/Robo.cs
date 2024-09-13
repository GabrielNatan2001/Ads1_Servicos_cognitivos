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
            this.EncontrouHumano = true;
        }
        public void EjetarHumano()
        {
            this.EncontrouHumano = false;
        }
        public void Log(string movimento)
        {

        }

        public void AlterarPosicao(int linha, int coluna)
        {
            this.Linha= linha;
            this.Coluna= coluna;
        }
        public void IniciarBusca(Peca[,] mapa, Entrada entrada, Humano humano)
        {
            var astar = new BuscaAStar(mapa);

            var caminho = astar.BuscarCaminhoAStar(entrada.Linha, entrada.Coluna, humano.Linha, humano.Coluna);
            Console.WriteLine("\nCaminho Encontrado");
            if (caminho.Count > 0)
            {
                for(int i = 0; i < caminho.Count - 1; i++)
                {
                    //Chegou uma posição antes do humano, pegar humano
                    if (caminho[i].Item1 == humano.Linha && caminho[i].Item2 == humano.Coluna)
                    {
                        PegarHumano();
                        humano.ColetadoPeloRobo();
                    }
                    else
                    {
                        AlterarPosicao(caminho[i].Item1, caminho[i].Item2);
                        humano.AlterarPosicao(caminho[i].Item1, caminho[i].Item2);
                    }
                }

                for (int i = caminho.Count -1; i >= 0; i--)
                {
                    //Chegou uma posição antes da entrada, ejetar humano
                    if (i == 0)
                    {
                        AlterarPosicao(caminho[i].Item1, caminho[i].Item2);
                        humano.AlterarPosicao(caminho[i].Item1, caminho[i].Item2);
                        this.EjetarHumano();
                        humano.Ejetado(entrada.Linha, entrada.Coluna);
                    }
                    else
                    {
                        AlterarPosicao(caminho[i].Item1, caminho[i].Item2);
                        humano.AlterarPosicao(caminho[i].Item1, caminho[i].Item2);
                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhum caminho encontrado.");
            }
        }
    }
}
