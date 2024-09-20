using ConsoleApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.PecasLabirinto
{
    public class Robo : Peca
    {
        public Robo(int linha, int coluna, int mapaX, int mapaY)
        {
            this.Linha = linha;
            this.Coluna = coluna;
            IniciandoVisao(mapaX, mapaY);
        }
        public int Linha { get; private set; }
        public int Coluna { get; private set; }
        public bool EncontrouHumano { get; set; } = false;

        public int Visao { get; private set; }
        public BuscaAStar AStart { get; set; }
        public List<string> MovimentosRealizados { get; private set; } = new() { "Ligar" };
        public List<string> SensorFrente { get; private set; } = new() { "" };
        public List<string> SensorEsquerdo { get; private set; } = new() { "" };
        public List<string> SensorDireito { get; private set; } = new() { "" };
        public List<string> SituacaoCarga { get; private set; } = new() { "" };

        private void IniciandoVisao(int mapaX, int mapaY)
        {
            // Se a posição estiver na borda superior, define a orientação inicial para baixo (1)
            if (this.Linha == 0)
            {
                Visao = (int)EVisao.Sul;
            }
            else if (this.Linha == mapaX - 1)  // Se a posição estiver na borda inferior, define a orientação inicial para cima (0)
            {
                Visao = (int)EVisao.Norte;
            }
            else if (this.Coluna == 0)// Se a posição estiver na borda esquerda, define a orientação inicial para a direita (3)
            {
                Visao = (int)EVisao.Leste;
            }
            else if (this.Coluna == mapaY - 1)// Se a posição estiver na borda direita, define a orientação inicial para a esquerda (2)
            {
                Visao = (int)EVisao.Oeste;
            }
            else
            {
                Visao = (int)EVisao.Norte;
            }
        }
        public void Andar(int linha, int coluna, Peca[,] mapa)
        {
            // Checar o que está à frente e nas laterais
            Peca frente = null, esquerda = null, direita = null;
            int maxLinhas = mapa.GetLength(0); // número de linhas do mapa
            int maxColunas = mapa.GetLength(1); // número de colunas do mapa
            switch (this.Visao)
            {
                case (int)EVisao.Norte:
                    frente = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    esquerda = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    direita = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    break;
                case (int)EVisao.Leste:
                    frente = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    esquerda = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    direita = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    break;
                case (int)EVisao.Sul:
                    frente = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    esquerda = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    direita = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    break;
                case (int)EVisao.Oeste:
                    frente = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    esquerda = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    direita = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    break;
            }

            if (!(mapa[linha, coluna] is Caminho))
            {
                throw new Exception("Robo não é permitido andar para uma posição que não seja caminho!");
            }

            this.Linha = linha;
            this.Coluna = coluna;
            SensorEsquerdo.Add(esquerda is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorFrente.Add(frente is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorDireito.Add(direita is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            MovimentosRealizados.Add("A");
            SituacaoCarga.Add(EncontrouHumano ? "Com humano" : "Sem Humano");
        }

        public void GirarParaDireita(int linha, int coluna, Peca[,] mapa)
        {
            switch (this.Visao)
            {
                case (int)EVisao.Norte:
                    Visao = (int)EVisao.Leste;
                    break;
                case (int)EVisao.Leste:
                    Visao = (int)EVisao.Sul;
                    break;
                case (int)EVisao.Sul:
                    Visao = (int)EVisao.Oeste;
                    break;
                case (int)EVisao.Oeste:
                    Visao = (int)EVisao.Norte;
                    break;
            }

            Peca frente = null, esquerda = null, direita = null;
            int maxLinhas = mapa.GetLength(0); // número de linhas do mapa
            int maxColunas = mapa.GetLength(1); // número de colunas do mapa
            switch (this.Visao)
            {
                case (int)EVisao.Norte:
                    frente = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    esquerda = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    direita = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    break;
                case (int)EVisao.Leste:
                    frente = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    esquerda = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    direita = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    break;
                case (int)EVisao.Sul:
                    frente = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    esquerda = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    direita = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    break;
                case (int)EVisao.Oeste:
                    frente = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    esquerda = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    direita = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    break;
            }
            SensorEsquerdo.Add(esquerda is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorFrente.Add(frente is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorDireito.Add(direita is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            MovimentosRealizados.Add("G");
            SituacaoCarga.Add(EncontrouHumano ? "Com humano" : "Sem Humano");
        }

        public void PegarHumano(Humano humano, int linha, int coluna, Peca[,] mapa)
        {
            if (!EstaHumanoNaFrente(humano))
            {
                throw new Exception("Não há humano à frente do robô para ser coletado!");
            }

            // Validação: Verificar se o robô já encontrou um humano
            if (EncontrouHumano)
            {
                throw new Exception("Humano já foi coletado pelo robô!");
            }

            Peca frente = null, esquerda = null, direita = null;
            int maxLinhas = mapa.GetLength(0); // número de linhas do mapa
            int maxColunas = mapa.GetLength(1); // número de colunas do mapa
            switch (this.Visao)
            {
                case (int)EVisao.Norte:
                    frente = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    esquerda = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    direita = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    break;
                case (int)EVisao.Leste:
                    frente = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    esquerda = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    direita = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    break;
                case (int)EVisao.Sul:
                    frente = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    esquerda = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    direita = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    break;
                case (int)EVisao.Oeste:
                    frente = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    esquerda = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    direita = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    break;
            }
            SensorEsquerdo.Add(esquerda is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorFrente.Add(frente is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorDireito.Add(direita is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SituacaoCarga.Add(EncontrouHumano ? "Com humano" : "Sem Humano");
            MovimentosRealizados.Add("P");

            EncontrouHumano = true;
        }
        private bool EstaHumanoNaFrente(Humano humano)
        {
            switch (Visao)
            {
                case (int)EVisao.Norte:
                    return humano.Linha < Linha && humano.Coluna == Coluna;
                case (int)EVisao.Sul:
                    return humano.Linha > Linha && humano.Coluna == Coluna;
                case (int)EVisao.Oeste:
                    return humano.Coluna < Coluna && humano.Linha == Linha;
                case (int)EVisao.Leste:
                    return humano.Coluna > Coluna && humano.Linha == Linha;
                default:
                    return false;
            }
        }
        public void EjetarHumano(int linha, int coluna, Peca[,] mapa)
        {
            // Validação: Verificar se há um humano para ser ejetado
            if (!EncontrouHumano)
            {
                throw new Exception("Não há humano para ser ejetado!");
            }

            EncontrouHumano = false;

            Peca frente = null, esquerda = null, direita = null;
            int maxLinhas = mapa.GetLength(0); // número de linhas do mapa
            int maxColunas = mapa.GetLength(1); // número de colunas do mapa
            switch (this.Visao)
            {
                case (int)EVisao.Norte:
                    frente = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    esquerda = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    direita = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    break;
                case (int)EVisao.Leste:
                    frente = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    esquerda = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    direita = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    break;
                case (int)EVisao.Sul:
                    frente = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    esquerda = (coluna + 1 < maxColunas) ? mapa[linha, coluna + 1] : null;
                    direita = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    break;
                case (int)EVisao.Oeste:
                    frente = (coluna - 1 >= 0) ? mapa[linha, coluna - 1] : null;
                    esquerda = (linha + 1 < maxLinhas) ? mapa[linha + 1, coluna] : null;
                    direita = (linha - 1 >= 0) ? mapa[linha - 1, coluna] : null;
                    break;
            }
            SensorEsquerdo.Add(esquerda is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorFrente.Add(frente is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            SensorDireito.Add(direita is Parede ? "Parede" : esquerda is Humano ? "Humano" : "Vazio");
            MovimentosRealizados.Add("E");
            SituacaoCarga.Add(EncontrouHumano ? "Com humano" : "Sem Humano");
        }
        public void ExportarLog(string caminhoArquivo)
        {
            string nomeArquivo = "";
            if (caminhoArquivo.Contains("\\"))
            {
                nomeArquivo = caminhoArquivo.Split("\\").Last().Split(".").First();
            }
            else
            {
                nomeArquivo = caminhoArquivo.Split(".").First();
            }

            try
            {
                using (StreamWriter sw = new StreamWriter($"{nomeArquivo}.csv", true))
                {
                    sw.WriteLine("Comando enviados," + string.Join(",", MovimentosRealizados));
                    sw.WriteLine("Sensor frente," + string.Join(",", SensorFrente));
                    sw.WriteLine("Sensor esquerdo," + string.Join(",", SensorEsquerdo));
                    sw.WriteLine("Sensor direito," + string.Join(",", SensorDireito));
                    sw.WriteLine("Sitacao carga," + string.Join(",", SituacaoCarga));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exportar CSV de movimentos do robo");
            }
        }
        private void GirarParaDirecao(int direcao, int linha, int coluna, Peca[,] mapa)
        {
            while (Visao != direcao)
            {
                GirarParaDireita(linha, coluna, mapa);
            }
        }
        private int CalcularDirecaoParaPosicao(int linha, int coluna)
        {
            if (linha < Linha) return (int)EVisao.Norte;
            if (linha > Linha) return (int)EVisao.Sul;
            if (coluna < Coluna) return (int)EVisao.Oeste;
            return (int)EVisao.Leste;
        }
        public void IniciarBusca(Peca[,] mapa, Entrada entrada, Humano humano, string arquivo)
        {
            var astar = new BuscaAStar(mapa);

            var caminho = astar.BuscarCaminhoAStar(entrada.Linha, entrada.Coluna, humano.Linha, humano.Coluna);
            if (caminho.Count > 0)
            {
                Console.WriteLine("\nCaminho Encontrado!");
                for (int i = 0; i < caminho.Count; i++)
                {
                    if (mapa[caminho[i].Item1, caminho[i].Item2] is Entrada) //Pula a entrada
                    {
                        continue;
                    }
                    //Chegou uma posição antes do humano, pegar humano
                    if (caminho[i].Item1 == humano.Linha && caminho[i].Item2 == humano.Coluna)
                    {
                        var proximaPosicao = caminho[i];

                        // Calcular a direção para a próxima posição
                        int direcaoDesejada = CalcularDirecaoParaPosicao(proximaPosicao.Item1, proximaPosicao.Item2);

                        // Girar para a direção correta
                        GirarParaDirecao(direcaoDesejada, proximaPosicao.Item1, proximaPosicao.Item2, mapa);

                        PegarHumano(humano, proximaPosicao.Item1, proximaPosicao.Item2, mapa);
                        humano.ColetadoPeloRobo();
                    }
                    else
                    {
                        var proximaPosicao = caminho[i];

                        // Calcular a direção para a próxima posição
                        int direcaoDesejada = CalcularDirecaoParaPosicao(proximaPosicao.Item1, proximaPosicao.Item2);

                        // Girar para a direção correta
                        GirarParaDirecao(direcaoDesejada, proximaPosicao.Item1, proximaPosicao.Item2, mapa);
                        Andar(proximaPosicao.Item1, proximaPosicao.Item2, mapa);
                    }
                }

                //-2 pois já está na posição correta(o -1 seria o humano)
                for (int i = caminho.Count - 2; i >= 0; i--)
                {
                    //Chegou uma posição antes da entrada, ejetar humano
                    if (i == 0)
                    {
                        var proximaPosicao = caminho[i]; // Calcular a direção para a próxima posição
                        int direcaoDesejada = CalcularDirecaoParaPosicao(proximaPosicao.Item1, proximaPosicao.Item2);

                        // Girar para a direção correta
                        GirarParaDirecao(direcaoDesejada, proximaPosicao.Item1, proximaPosicao.Item2, mapa);

                        this.EjetarHumano(proximaPosicao.Item1, proximaPosicao.Item2, mapa);
                        humano.Ejetado(entrada.Linha, entrada.Coluna);
                    }
                    else if (caminho[i].Item1 == Linha && caminho[i].Item2 == Coluna)
                    {
                        continue;
                    }
                    else
                    {
                        var proximaPosicao = caminho[i]; // Calcular a direção para a próxima posição
                        int direcaoDesejada = CalcularDirecaoParaPosicao(proximaPosicao.Item1, proximaPosicao.Item2);

                        // Girar para a direção correta
                        GirarParaDirecao(direcaoDesejada, proximaPosicao.Item1, proximaPosicao.Item2, mapa);

                        Andar(caminho[i].Item1, caminho[i].Item2, mapa);
                        humano.AlterarPosicao(caminho[i].Item1, caminho[i].Item2);
                    }
                }

                ExportarLog(arquivo);
            }
            else
            {
                Console.WriteLine("Nenhum caminho encontrado.");
            }
        }
    }
}
