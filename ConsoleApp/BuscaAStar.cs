using ConsoleApp.PecasLabirinto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        // Define o ponto de partida e o ponto de chegada (objetivo)
        var inicio = (linhaEntrada, colunaEntrada);
        var objetivo = (humanoLinha, humanoColuna);

        // Fila de prioridade que armazena os nós a serem explorados, ordenados pelo valor de fScore
        var listaAberta = new PriorityQueue<Node, int>();
        // Dicionário que rastreia o nó de onde cada nó veio (para reconstruir o caminho)
        var veioDe = new Dictionary<(int, int), (int, int)>();
        // Dicionário que armazena o custo do caminho desde o início até um nó específico
        var gScore = new Dictionary<(int, int), int>();
        // Dicionário que armazena o custo total estimado do caminho passando por um nó específico
        var fScore = new Dictionary<(int, int), int>();

        // Inicializa os custos g e f para todas as posições possíveis com valores infinitos
        foreach (var pos in ObterTodasPosicoes())
        {
            gScore[pos] = int.MaxValue;
            fScore[pos] = int.MaxValue;
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
            if (atual == objetivo)
            {
                // Reconstrói o caminho encontrado a partir do nó objetivo
                var caminho = ReconstruirCaminho(veioDe, atual);
                return caminho; // Retorna o caminho encontrado
            }

            // Explora todos os vizinhos do nó atual
            foreach (var vizinho in ObterVizinhos(atual))
            {
                // Calcula o custo tentativo para o vizinho
                var gScoreTentativo = gScore[atual] + 1; // Custo fixo para todos os movimentos

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

    // Método que obtém os vizinhos de uma posição atual considerando movimentos em quatro direções
    private IEnumerable<(int, int)> ObterVizinhos((int, int) pos)
    {
        // Define as direções possíveis de movimento (cima, baixo, esquerda, direita)
        var direcoes = new (int, int)[]
        {
            (-1, 0), // Cima
            (1, 0),  // Baixo
            (0, -1), // Esquerda
            (0, 1)   // Direita
        };

        foreach (var movimento in direcoes)
        {
            // Calcula a nova posição ao mover para frente
            var novaLinha = pos.Item1 + movimento.Item1;
            var novaColuna = pos.Item2 + movimento.Item2;

            // Verifica se a nova posição é válida e não é uma parede
            if (EhPosicaoValida(novaLinha, novaColuna) && !(Mapa[novaLinha, novaColuna] is Parede))
            {
                yield return (novaLinha, novaColuna);
            }
        }
    }

    // Método que verifica se uma posição é válida dentro dos limites do mapa
    private bool EhPosicaoValida(int linha, int coluna)
    {
        return linha >= 0 && linha < Mapa.GetLength(0) && coluna >= 0 && coluna < Mapa.GetLength(1);
    }

    // Método que calcula a heurística (distância de Manhattan) entre duas posições
    private int Heuristica((int, int) a, (int, int) b)
    {
        // Calcula a distância de Manhattan
        return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }

    // Método que reconstrói o caminho a partir do nó objetivo até o início
    private List<(int, int)> ReconstruirCaminho(Dictionary<(int, int), (int, int)> veioDe, (int, int) atual)
    {
        var caminhoTotal = new List<(int, int)>();

        // Reconstroi o caminho seguindo o dicionário veioDe
        while (veioDe.ContainsKey(atual))
        {
            var anterior = veioDe[atual];
            // Adiciona a posição atual ao caminho total
            caminhoTotal.Add(atual);
            // Atualiza a posição atual para a posição anterior
            atual = anterior;
        }
        // Adiciona o ponto de partida
        caminhoTotal.Add(atual);
        // Reverte a lista para obter o caminho do início ao fim
        caminhoTotal.Reverse();

        return caminhoTotal;
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
        public (int, int) Posicao { get; }
        public int GScore { get; }
        public int FScore { get; }

        // Construtor da classe Node que inicializa a posição, o custo gScore e o custo fScore
        public Node((int, int) posicao, int gScore, int fScore)
        {
            Posicao = posicao;
            GScore = gScore;
            FScore = fScore;
        }
    }
}
