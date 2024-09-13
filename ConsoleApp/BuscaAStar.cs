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
        // Propriedade que representa o mapa de busca, onde cada célula pode ser um tipo de objeto, como uma parede ou espaço vazio
        public Peca[,] Mapa { get; private set; }

        // Construtor da classe que inicializa o mapa com o valor fornecido
        public BuscaAStar(Peca[,] mapa)
        {
            this.Mapa = mapa;
        }

        // Método que realiza a busca do caminho utilizando o algoritmo A*
        public List<(int, int)> BuscarCaminhoAStar(int linhaEntrada, int colunaEntrada, int humanoLinha, int humanoColuna)
        {
            // Determina a orientação inicial do robô com base na posição de entrada
            int orientacaoInicial = DeterminarOrientacaoInicial(linhaEntrada, colunaEntrada);
            // Define o ponto de partida com a posição e orientação inicial
            var inicio = (linhaEntrada, colunaEntrada, orientacaoInicial);
            // Define o ponto de chegada (objetivo) onde está o humano
            var objetivo = (humanoLinha, humanoColuna);

            // Fila de prioridade que armazena os nós a serem explorados, ordenados pelo valor de fScore
            var listaAberta = new PriorityQueue<Node, int>();
            // Dicionário que rastreia o nó de onde cada nó veio (para reconstruir o caminho)
            var veioDe = new Dictionary<(int, int, int), (int, int, int)>();
            // Dicionário que armazena o custo do caminho desde o início até um nó específico
            var gScore = new Dictionary<(int, int, int), int>();
            // Dicionário que armazena o custo total estimado do caminho passando por um nó específico
            var fScore = new Dictionary<(int, int, int), int>();

            // Inicializa os custos g e f para todas as posições e direções possíveis com valores infinitos
            foreach (var pos in ObterTodasPosicoes())
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    gScore[(pos.Item1, pos.Item2, dir)] = int.MaxValue;
                    fScore[(pos.Item1, pos.Item2, dir)] = int.MaxValue;
                }
            }

            // Define o custo inicial gScore do ponto de partida como 0
            gScore[inicio] = 0;
            // Calcula o valor de fScore para o ponto de partida, que é a soma do custo gScore com a heurística
            fScore[inicio] = Heuristica(inicio, objetivo);

            // Adiciona o ponto de partida na lista aberta com seu custo fScore
            listaAberta.Enqueue(new Node(inicio, 0, Heuristica(inicio, objetivo)), fScore[inicio]);

            // Loop principal do algoritmo A*
            while (listaAberta.Count > 0)
            {
                // Remove o nó com o menor fScore da lista aberta
                var atual = listaAberta.Dequeue().Posicao;

                // Verifica se o nó atual é o objetivo
                if ((atual.Item1, atual.Item2) == objetivo)
                {
                    // Reconstrói o caminho encontrado a partir do nó objetivo
                    var caminho = ReconstruirCaminho(veioDe, atual);
                    return caminho; // Retorna o caminho encontrado
                }

                // Explora todos os vizinhos do nó atual
                foreach (var vizinho in ObterVizinhos(atual))
                {
                    // Calcula o custo tentativo para o vizinho considerando o custo atual e se a orientação muda
                    var gScoreTentativo = gScore[atual] + (vizinho.Item3 == atual.Item3 ? 1 : 2); // 1 para avanço, 2 para giro

                    // Se o custo tentativo é menor que o custo conhecido, atualiza os valores
                    if (gScoreTentativo < gScore[vizinho])
                    {
                        // Atualiza o nó de onde o vizinho veio
                        veioDe[vizinho] = atual;
                        // Atualiza o custo gScore para o vizinho
                        gScore[vizinho] = gScoreTentativo;
                        // Calcula e atualiza o custo fScore do vizinho
                        fScore[vizinho] = gScore[vizinho] + Heuristica(vizinho, objetivo);

                        // Adiciona o vizinho na lista aberta com seu custo fScore
                        listaAberta.Enqueue(new Node(vizinho, gScore[vizinho], fScore[vizinho]), fScore[vizinho]);
                    }
                }
            }

            // Se não encontrou um caminho, retorna uma lista vazia
            return new List<(int, int)>();
        }

        // Método que determina a orientação inicial do robô com base na posição de entrada
        private int DeterminarOrientacaoInicial(int linhaEntrada, int colunaEntrada)
        {
            // Se a posição estiver na borda superior, define a orientação inicial para baixo (1)
            if (linhaEntrada == 0) return 1;
            // Se a posição estiver na borda inferior, define a orientação inicial para cima (0)
            if (linhaEntrada == Mapa.GetLength(0) - 1) return 0;
            // Se a posição estiver na borda esquerda, define a orientação inicial para a direita (3)
            if (colunaEntrada == 0) return 3;
            // Se a posição estiver na borda direita, define a orientação inicial para a esquerda (2)
            if (colunaEntrada == Mapa.GetLength(1) - 1) return 2;

            // Se não estiver nas bordas, define a orientação inicial como para cima (0) por padrão
            return 0;
        }

        // Método que obtém os vizinhos de uma posição atual considerando movimentos e giros
        private IEnumerable<(int, int, int)> ObterVizinhos((int, int, int) pos)
        {
            // Define as direções possíveis de movimento (cima, baixo, esquerda, direita)
            var direcoes = new (int, int)[]
            {
            (-1, 0), // Cima
            (1, 0),  // Baixo
            (0, -1), // Esquerda
            (0, 1)   // Direita
            };

            var orientacaoAtual = pos.Item3; // Orientação atual do robô
            var movimentoAtual = direcoes[orientacaoAtual]; // Movimento correspondente à orientação atual

            // Calcula a nova posição ao mover para frente
            var linhaFrente = pos.Item1 + movimentoAtual.Item1;
            var colunaFrente = pos.Item2 + movimentoAtual.Item2;

            // Verifica se a nova posição é válida e não é uma parede
            if (EhPosicaoValida(linhaFrente, colunaFrente) && !(Mapa[linhaFrente, colunaFrente] is Parede))
            {
                // Retorna a nova posição com a mesma orientação
                yield return (linhaFrente, colunaFrente, orientacaoAtual);
            }

            // Calcula a nova orientação ao girar 90 graus à direita
            var novaOrientacao = (orientacaoAtual + 1) % 4;
            // Retorna a posição atual com a nova orientação
            yield return (pos.Item1, pos.Item2, novaOrientacao);
        }

        // Método que verifica se uma posição é válida dentro dos limites do mapa
        private bool EhPosicaoValida(int linha, int coluna)
        {
            return linha >= 0 && linha < Mapa.GetLength(0) && coluna >= 0 && coluna < Mapa.GetLength(1);
        }

        // Método que calcula a heurística (distância de Manhattan) entre duas posições
        private int Heuristica((int, int, int) a, (int, int) b)
        {
            // Calcula a distância de Manhattan desconsiderando a orientação
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
        }

        // Método que reconstrói o caminho a partir do nó objetivo até o início
        private List<(int, int)> ReconstruirCaminho(Dictionary<(int, int, int), (int, int, int)> veioDe, (int, int, int) atual)
        {
            var caminhoTotal = new List<(int, int)>();
            var comandos = new List<char>();

            // Reconstroi o caminho seguindo o dicionário veioDe
            while (veioDe.ContainsKey(atual))
            {
                var anterior = veioDe[atual];
                // Adiciona 'A' para movimento para frente e 'G' para giro
                if (atual.Item3 == anterior.Item3)
                {
                    comandos.Add('A');
                }
                else
                {
                    comandos.Add('G');
                }

                // Adiciona a posição atual ao caminho total
                caminhoTotal.Add((atual.Item1, atual.Item2));
                // Atualiza a posição atual para a posição anterior
                atual = anterior;
            }
            // Reverte a lista para obter o caminho do início ao fim
            caminhoTotal.Reverse();
            comandos.Reverse();

            // Registra os comandos gerados em um arquivo
            RegistrarComandos(comandos);

            return caminhoTotal;
        }

        // Método que registra os comandos gerados em um arquivo de texto
        private void RegistrarComandos(List<char> comandos)
        {
            var caminhoArquivoLog = "comandos.txt";
            // Usa um StreamWriter para criar e escrever no arquivo de log
            using (StreamWriter sw = new StreamWriter(caminhoArquivoLog))
            {
                // Escreve os comandos separados por vírgulas no arquivo
                sw.WriteLine(string.Join(",", comandos));
            }
        }

        // Método que gera todas as posições possíveis no mapa
        private IEnumerable<(int, int)> ObterTodasPosicoes()
        {
            // Itera sobre todas as linhas e colunas do mapa
            for (int linha = 0; linha < Mapa.GetLength(0); linha++)
            {
                for (int coluna = 0; coluna < Mapa.GetLength(1); coluna++)
                {
                    // Retorna a posição atual
                    yield return (linha, coluna);
                }
            }
        }

        // Classe interna que representa um nó na busca A*
        private class Node
        {
            public (int, int, int) Posicao { get; }
            public int GScore { get; }
            public int FScore { get; }

            // Construtor da classe Node que inicializa a posição, o custo gScore e o custo fScore
            public Node((int, int, int) posicao, int gScore, int fScore)
            {
                Posicao = posicao;
                GScore = gScore;
                FScore = fScore;
            }
        }
    }


}
