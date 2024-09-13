using ConsoleApp.PecasLabirinto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class BuscaAStar
    {
        public Peca[,] Mapa { get; private set; }
        public BuscaAStar(Peca[,] mapa)
        {
            this.Mapa = mapa;
        }

        public List<(int, int)> BuscarCaminhoAStar(int linhaEntrada, int colunaEntrada, int humanoLinha, int humanoColuna)
        {
            int orientacaoInicial = DeterminarOrientacaoInicial(linhaEntrada, colunaEntrada);
            var inicio = (linhaEntrada, colunaEntrada, orientacaoInicial);
            var objetivo = (humanoLinha, humanoColuna);

            var listaAberta = new PriorityQueue<Node, int>();
            var veioDe = new Dictionary<(int, int, int), (int, int, int)>();
            var gScore = new Dictionary<(int, int, int), int>();
            var fScore = new Dictionary<(int, int, int), int>();

            foreach (var pos in ObterTodasPosicoes())
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    gScore[(pos.Item1, pos.Item2, dir)] = int.MaxValue;
                    fScore[(pos.Item1, pos.Item2, dir)] = int.MaxValue;
                }
            }

            gScore[inicio] = 0;
            fScore[inicio] = Heuristica(inicio, objetivo);

            listaAberta.Enqueue(new Node(inicio, 0, Heuristica(inicio, objetivo)), fScore[inicio]);

            while (listaAberta.Count > 0)
            {
                var atual = listaAberta.Dequeue().Posicao;

                if ((atual.Item1, atual.Item2) == objetivo)
                {
                    var caminho = ReconstruirCaminho(veioDe, atual);
                    return caminho;
                }

                foreach (var vizinho in ObterVizinhos(atual))
                {
                    var gScoreTentativo = gScore[atual] + (vizinho.Item3 == atual.Item3 ? 1 : 2); // 1 para avanço, 2 para giro

                    if (gScoreTentativo < gScore[vizinho])
                    {
                        veioDe[vizinho] = atual;
                        gScore[vizinho] = gScoreTentativo;
                        fScore[vizinho] = gScore[vizinho] + Heuristica(vizinho, objetivo);

                        listaAberta.Enqueue(new Node(vizinho, gScore[vizinho], fScore[vizinho]), fScore[vizinho]);
                    }
                }
            }

            return new List<(int, int)>(); // Não encontrou caminho
        }
        private int DeterminarOrientacaoInicial(int linhaEntrada, int colunaEntrada)
        {
            if (linhaEntrada == 0) return 1; // Se estiver na borda superior, inicializa para baixo
            if (linhaEntrada == Mapa.GetLength(0) - 1) return 0; // Se estiver na borda inferior, inicializa para cima
            if (colunaEntrada == 0) return 3; // Se estiver na borda esquerda, inicializa para a direita
            if (colunaEntrada == Mapa.GetLength(1) - 1) return 2; // Se estiver na borda direita, inicializa para a esquerda

            return 0;
        }
        private IEnumerable<(int, int, int)> ObterVizinhos((int, int, int) pos)
        {
            var direcoes = new (int, int)[]
            {
                (-1, 0), // Cima
                (1, 0),  // Baixo
                (0, -1), // Esquerda
                (0, 1)   // Direita
                    };

            var orientacaoAtual = pos.Item3;
            var movimentoAtual = direcoes[orientacaoAtual];

            // Movimento para frente
            var linhaFrente = pos.Item1 + movimentoAtual.Item1;
            var colunaFrente = pos.Item2 + movimentoAtual.Item2;

            if (EhPosicaoValida(linhaFrente, colunaFrente) && !(Mapa[linhaFrente, colunaFrente] is Parede))
            {
                yield return (linhaFrente, colunaFrente, orientacaoAtual); // Avançar mantém a mesma orientação
            }

            // Giro 90 graus à direita
            var novaOrientacao = (orientacaoAtual + 1) % 4;
            yield return (pos.Item1, pos.Item2, novaOrientacao); // Gira para a direita
        }

        private bool EhPosicaoValida(int linha, int coluna)
        {
            return linha >= 0 && linha < Mapa.GetLength(0) && coluna >= 0 && coluna < Mapa.GetLength(1);
        }

        private int Heuristica((int, int, int) a, (int, int) b)
        {
            // Considera apenas a posição (despreza a orientação)
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2); // Distância de Manhattan
        }

        private List<(int, int)> ReconstruirCaminho(Dictionary<(int, int, int), (int, int, int)> veioDe, (int, int, int) atual)
        {
            var caminhoTotal = new List<(int, int)>();
            var comandos = new List<char>();

            while (veioDe.ContainsKey(atual))
            {
                var anterior = veioDe[atual];
                if (atual.Item3 == anterior.Item3)
                {
                    // Movimento para frente
                    comandos.Add('A');
                }
                else
                {
                    // Giro
                    comandos.Add('G');
                }

                caminhoTotal.Add((atual.Item1, atual.Item2));
                atual = anterior;
            }
            caminhoTotal.Reverse();
            comandos.Reverse();

            // Log dos comandos
            RegistrarComandos(comandos);

            return caminhoTotal;
        }

        private void RegistrarComandos(List<char> comandos)
        {
            var caminhoArquivoLog = "comandos.txt";
            using (StreamWriter sw = new StreamWriter(caminhoArquivoLog))
            {
                sw.WriteLine(string.Join(",", comandos));
            }
        }

        private IEnumerable<(int, int)> ObterTodasPosicoes()
        {
            for (int linha = 0; linha < Mapa.GetLength(0); linha++)
            {
                for (int coluna = 0; coluna < Mapa.GetLength(1); coluna++)
                {
                    yield return (linha, coluna);
                }
            }
        }

        private class Node
        {
            public (int, int, int) Posicao { get; }
            public int GScore { get; }
            public int FScore { get; }

            public Node((int, int, int) posicao, int gScore, int fScore)
            {
                Posicao = posicao;
                GScore = gScore;
                FScore = fScore;
            }
        }
    }

}
