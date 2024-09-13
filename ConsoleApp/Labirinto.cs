using ConsoleApp.PecasLabirinto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp
{
    public class Labirinto
    {
        public Peca[,] mapa { get; private set; }
        public Robo robo { get; private set; }
        public Humano humano { get; private set; }
        public Entrada entrada { get; private set; }

        public void CriarMapa(string caminhoArquivo)
        {
            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                string[] linhas = File.ReadAllLines(caminhoArquivo);

                int qtdeColunas = sr.ReadLine().ToArray().Length;
                int qtdeLinhas = linhas.Length;

                this.mapa = new Peca[qtdeLinhas, qtdeColunas];
            }

            PopularMapa(caminhoArquivo);
        }
        public void PopularMapa(string caminhoArquivo)
        {
            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                string linha;
                int linhaAtual = 0;

                while ((linha = sr.ReadLine()) != null)
                {
                    var linhaItens = linha.ToCharArray();

                    for (int colunaAtual = 0; colunaAtual < this.mapa.GetLength(1); colunaAtual++)
                    {
                        var i = (colunaAtual > linhaItens.Length -1) ? '\0': linhaItens[colunaAtual];
                        switch (i)
                        {
                            case '*':
                                this.mapa[linhaAtual, colunaAtual] = new Parede();
                                break;
                            case ' ':
                                this.mapa[linhaAtual, colunaAtual] = new Caminho();
                                break;
                            case 'E':
                                this.mapa[linhaAtual, colunaAtual] = new Entrada(linhaAtual, colunaAtual);
                                this.entrada = new Entrada(linhaAtual, colunaAtual);
                                this.robo = new Robo(linhaAtual, colunaAtual);
                                break;
                            case 'H':
                                this.mapa[linhaAtual, colunaAtual] = new Caminho();
                                this.humano = new Humano(linhaAtual, colunaAtual);
                                break;
                            default:
                                this.mapa[linhaAtual, colunaAtual] = new Caminho();
                                break;

                        }
                    }
                    linhaAtual++;
                }
            }
        }

        public void DesenhaMapa()
        {
            for (int linhaAtual = 0; linhaAtual < this.mapa.GetLength(0); linhaAtual++)
            {
                for (int colunaAtual = 0; colunaAtual < this.mapa.GetLength(1); colunaAtual++)
                {
                    var peca = this.mapa[linhaAtual, colunaAtual];
                    if (humano.Linha == linhaAtual && humano.Coluna == colunaAtual)
                    {
                        peca = new Humano(linhaAtual, colunaAtual);
                    }

                    switch (peca)
                    {
                        case Parede:
                            Console.Write("*");
                            break;
                        case Caminho:
                            Console.Write(" ");
                            break;
                        case Entrada:
                            Console.Write("E");
                            break;
                        case Humano:
                            Console.Write("H");
                            break;

                    }
                }
                Console.Write("\n");
            }
        }

    }
}
