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
            lab.DesenhaMapa();

            lab.robo.IniciarBusca(lab.mapa, lab.entrada, lab.humano, caminhoArquivo);

            if (lab.humano.Linha == lab.entrada.Linha && lab.humano.Coluna == lab.entrada.Coluna)
            {
                Console.WriteLine("Humano saiu do labirinto");
            }
            
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
