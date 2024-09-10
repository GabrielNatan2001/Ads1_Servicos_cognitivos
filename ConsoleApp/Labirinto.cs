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
        public Peca[,] Mapa { get; private set; }
        public int LinhaEntrada  { get; private set; }
        public int ColunaEntrada  { get; private set; }

        public Robo Robo { get; private set; }
        public Humano HumanoL { get; private set; }
        public Entrada EntradaL { get; private set; }

        public void CriarMapa(string caminhoArquivo)
        {
            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                string[] linhas = File.ReadAllLines(caminhoArquivo);

                int qtdeColunas = sr.ReadLine().ToArray().Length;
                int qtdeLinhas = linhas.Length;

                this.Mapa = new Peca[qtdeLinhas, qtdeColunas];
            }
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

                    for (int colunaAtual = 0; colunaAtual < this.Mapa.GetLength(1); colunaAtual++)
                    {
                        var i = (colunaAtual > linhaItens.Length -1) ? '\0': linhaItens[colunaAtual];
                        switch (i)
                        {
                            case '*':
                                this.Mapa[linhaAtual, colunaAtual] = new Parede();
                                break;
                            case ' ':
                                this.Mapa[linhaAtual, colunaAtual] = new Caminho();
                                break;
                            case 'E':
                                this.Mapa[linhaAtual, colunaAtual] = new Caminho();
                                this.EntradaL = new Entrada(linhaAtual, colunaAtual);
                                this.Robo = new Robo(linhaAtual, colunaAtual);
                                this.ColunaEntrada = colunaAtual;
                                this.LinhaEntrada = linhaAtual;
                                break;
                            case 'H':
                                this.Mapa[linhaAtual, colunaAtual] = new Caminho();
                                this.HumanoL = new Humano(linhaAtual, colunaAtual);
                                break;
                            default:
                                this.Mapa[linhaAtual, colunaAtual] = new Caminho();
                                break;

                        }
                    }
                    linhaAtual++;
                }
            }
        }

        public void DesenhaMapa()
        {
            for (int linhaAtual = 0; linhaAtual < this.Mapa.GetLength(0); linhaAtual++)
            {
                for (int colunaAtual = 0; colunaAtual < this.Mapa.GetLength(1); colunaAtual++)
                {
                    var peca = this.Mapa[linhaAtual, colunaAtual];
                    if (HumanoL.Linha == linhaAtual && HumanoL.Coluna == colunaAtual)
                    {
                        peca = new Humano(linhaAtual, colunaAtual);
                    }else if (EntradaL.Linha == linhaAtual && EntradaL.Coluna == colunaAtual)
                    {
                        peca = new Entrada(linhaAtual, colunaAtual);
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

        public void Iniciar()
        {
            BuscaAStar astar = new BuscaAStar(this.Mapa);

            var caminho = astar.BuscarCaminhoAStar(LinhaEntrada, ColunaEntrada, HumanoL.Linha, HumanoL.Coluna);
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
