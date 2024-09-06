using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {

        Console.WriteLine("Digite o caminho do arquivo:");

        string caminhoArquivo = Console.ReadLine();
        List<char[]> labirinto = new List<char[]>();

        try
        {
            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                string linha;
                while ((linha = sr.ReadLine()) != null)
                {
                    labirinto.Add(linha.ToCharArray());
                }
            }

            // Exibe a matriz de labirinto
            Console.WriteLine("Labirinto:");
            foreach (char[] linha in labirinto)
            {
                Console.WriteLine(new string(linha));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ocorreu um erro: " + e.Message);
        }
    }
}
