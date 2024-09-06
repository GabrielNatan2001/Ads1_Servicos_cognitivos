using System;
using System.IO;
using System.Collections.Generic;
using ConsoleApp;

class Program
{
    static void Main()
    {

        Console.WriteLine("Digite o caminho do arquivo:");

        string caminhoArquivo = Console.ReadLine();
        Labirinto lab = new();

        lab.LerArquivoDeLabirinto(caminhoArquivo);

        foreach (var linha in lab.Mapa)
        {
            Console.WriteLine(new string(linha));
        }
    }
}
