using System;
using System.IO;
using System.Collections.Generic;
using ConsoleApp;
using ConsoleApp.PecasLabirinto;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Digite o caminho do arquivo:");

            string caminhoArquivo = Console.ReadLine();
            Labirinto lab = new();

            lab.CriarMapa(caminhoArquivo);
            lab.PopularMapa(caminhoArquivo);
            lab.DesenhaMapa();

            lab.Robo.IniciarBusca(lab.Mapa, lab.EntradaL, lab.HumanoL);

            Console.WriteLine("\n\n\nLabirinto com coordenadas");
            for (int linhaAtual = 0; linhaAtual < lab.Mapa.GetLength(0); linhaAtual++)
            {
                for (int colunaAtual = 0; colunaAtual < lab.Mapa.GetLength(1); colunaAtual++)
                {
                    var peca = lab.Mapa[linhaAtual, colunaAtual];
                    if (lab.HumanoL.Linha == linhaAtual && lab.HumanoL.Coluna == colunaAtual)
                    {
                        peca = new Humano(linhaAtual, colunaAtual);
                    }

                    switch (peca)
                    {
                        case Parede:
                            Console.Write($"({linhaAtual},{colunaAtual})*");
                            break;
                        case Caminho:
                            Console.Write($"({linhaAtual},{colunaAtual}) ");
                            break;
                        case Entrada:
                            Console.Write($"({linhaAtual},{colunaAtual})E");
                            break;
                        case Humano:
                            Console.Write($"({linhaAtual},{colunaAtual})H");
                            break;

                    }
                }
                Console.Write("\n");
            }

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
