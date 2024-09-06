using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Robo : Peca
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public bool EncontrouHumano { get; set; } = false;
        public int[,] VisaoFrente { get; set; }
             
        public void MoverParaFrente()
        {
            Linha = Linha + 1;
        }
        public void MoverParaDireta()
        {
            Linha = Linha - 1;
            Coluna = Coluna + 1;
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
