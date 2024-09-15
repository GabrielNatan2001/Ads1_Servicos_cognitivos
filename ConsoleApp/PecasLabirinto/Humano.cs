using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Humano : Peca
    {
        public Humano(int linha, int coluna)
        {
            this.Linha = linha;
            this.Coluna = coluna;
        }
        public int Linha { get; private set; }
        public int Coluna { get; private set; }
        public bool Coletado { get; private set; } = false;

        public void ColetadoPeloRobo()
        {
            Coletado = true;
        }

        internal void Ejetado(int linha, int coluna)
        {
            this.Linha = linha;
            this.Coluna = coluna;
            Coletado = false;
        }

        public void AlterarPosicao(int linha, int coluna)
        {
            this.Linha = linha;
            this.Coluna = coluna;
        }
    } 
}
