using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Robo : Peca
    {
        public int[,] Posicao { get; set; }
        public bool EncontrouHumano { get; set; } = false;
        public int[,] VisaoFrente { get; set; }
             
        public void MoverParaFrente()
        {

        }
        public void MoverParaDireta()
        {

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
