using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Entrada : Peca
    {
        public Entrada(int linha, int coluna)
        {
            this.Linha = linha;
            this.Coluna = coluna;
        }
        public int Linha { get; private set; }
        public int Coluna { get; private set; }
    }
}
