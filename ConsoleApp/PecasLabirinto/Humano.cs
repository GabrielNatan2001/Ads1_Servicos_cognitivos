using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Humano : Peca
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public bool Coletado { get; set; } = false;
    } 
}
