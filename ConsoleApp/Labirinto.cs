using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Labirinto
    {
        public List<char[]> Mapa { get; set; } = new();

        public void LerArquivoDeLabirinto(string caminhoArquivo)
        {
            using (StreamReader sr = new StreamReader(caminhoArquivo))
            {
                string linha;
                while ((linha = sr.ReadLine()) != null)
                {
                    Mapa.Add(linha.ToCharArray());
                }
            }
        }
    }
}
