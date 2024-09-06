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

            Robo robo = new Robo(lab.LinhaEntrada, lab.ColunaEntrada);

            Console.WriteLine($"Robo está olhando para [{robo.VisaoLinha}][{robo.VisaoColuna}]");
            robo.GirarParaDireita();
            Console.WriteLine($"Robo está olhando para [{robo.VisaoLinha}][{robo.VisaoColuna}]");
            Console.WriteLine($"Robo está na posição [{robo.Linha}][{robo.Coluna}]");
            robo.MoverParaFrente();
            Console.WriteLine($"Robo está na posição [{robo.Linha}][{robo.Coluna}]");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
