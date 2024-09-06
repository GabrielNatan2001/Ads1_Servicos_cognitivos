using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Robo : Peca
    {
        public Robo(int linha, int coluna)
        {
            this.Linha = linha;
            this.Coluna = coluna;
            IniciandoVisao();
        }
        public int Linha { get; private set; }
        public int Coluna { get; private set; }
        private bool EncontrouHumano { get; set; } = false;

        public int VisaoLinha { get; private set; }
        public int VisaoColuna { get; private set; }

        private void IniciandoVisao()
        {
            //TODO - Definir regra para visão inicial
            if (Linha == 0)
            {
            }
        }
        public void MoverParaFrente()
        {
            Linha = Linha + 1;
        }
        public void GirarParaDireita()
        {
            VisaoLinha = VisaoLinha - 1;
            VisaoColuna = VisaoColuna + 1;
        }

        public void PegarHumano()
        {
            
        }
        public void EjetarHumano()
        {

        }
        public void Log(string movimento)
        {
        }
    }
}
