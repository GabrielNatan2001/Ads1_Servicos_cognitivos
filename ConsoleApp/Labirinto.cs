using ConsoleApp.PecasLabirinto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Labirinto
    {
        public Peca[,] Mapa { get; set; }
        public int[,] Entrada { get; set; }


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

                    for (int colunaAtual = 0; colunaAtual < linhaItens.Length; colunaAtual++)
                    {
                        switch (linhaItens[colunaAtual])
                        {
                            case '*':
                                this.Mapa[linhaAtual, colunaAtual] = new Parede();
                                break;
                            case ' ':
                                this.Mapa[linhaAtual, colunaAtual] = new Caminho();
                                break;
                            case 'E':
                                this.Mapa[linhaAtual, colunaAtual] = new Robo()
                                {
                                    Posicao = new int[linhaAtual, colunaAtual]
                                };
                                this.Entrada = new int[linhaAtual, colunaAtual];
                                break;
                            case 'H':
                                this.Mapa[linhaAtual, colunaAtual] = new Pessoa();
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
                    switch (peca)
                    {
                        case Parede:
                            Console.Write("*");
                            break;;
                        case Caminho:
                            Console.Write(" ");
                            break;
                        case Robo:
                            Console.Write("E");
                            break;
                        case Pessoa:
                            Console.Write("H");
                            break;

                    }
                }

                Console.Write("\n");
            }
        }
    }
}
