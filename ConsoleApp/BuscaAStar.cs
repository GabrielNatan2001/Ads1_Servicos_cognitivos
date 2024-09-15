using ConsoleApp.PecasLabirinto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BuscaAStar
{
    public Peca[,] Mapa { get; private set; }
    public BuscaAStar(Peca[,] mapa)
    {
        this.Mapa = mapa;
    }

    public List<(int, int)> BuscarCaminhoAStar(int linhaEntrada, int colunaEntrada, int humanoLinha, int humanoColuna)
    {
        var inicio = (linhaEntrada, colunaEntrada);
        var objetivo = (humanoLinha, humanoColuna);

        var listaAberta = new PriorityQueue<Node, int>();
        var veioDe = new Dictionary<(int, int), (int, int)>();
        var gScore = new Dictionary<(int, int), int>();
        var fScore = new Dictionary<(int, int), int>();

        foreach (var pos in ObterTodasPosicoes())
        {
            gScore[pos] = int.MaxValue;
            fScore[pos] = int.MaxValue;
        }

        gScore[inicio] = 0;
        fScore[inicio] = Heuristica(inicio, objetivo);

        listaAberta.Enqueue(new Node(inicio, 0, Heuristica(inicio, objetivo)), fScore[inicio]);

        while (listaAberta.Count > 0)
        {
            var atual = listaAberta.Dequeue().Posicao;

            if (atual == objetivo)
            {
                var caminho = ReconstruirCaminho(veioDe, atual);
                return caminho;
            }

            foreach (var vizinho in ObterVizinhos(atual))
            {
                var gScoreTentativo = gScore[atual] + 1; 

                if (gScoreTentativo < gScore[vizinho])
                {
                    veioDe[vizinho] = atual;
                    gScore[vizinho] = gScoreTentativo;
                    fScore[vizinho] = gScore[vizinho] + Heuristica(vizinho, objetivo);

                    listaAberta.Enqueue(new Node(vizinho, gScore[vizinho], fScore[vizinho]), fScore[vizinho]);
                }
            }
        }

        return new List<(int, int)>();
    }

    private IEnumerable<(int, int)> ObterVizinhos((int, int) pos)
    {
        var direcoes = new (int, int)[]
        {
            (-1, 0), // Cima
            (1, 0),  // Baixo
            (0, -1), // Esquerda
            (0, 1)   // Direita
        };

        foreach (var movimento in direcoes)
        {
            var novaLinha = pos.Item1 + movimento.Item1;
            var novaColuna = pos.Item2 + movimento.Item2;

            if (EhPosicaoValida(novaLinha, novaColuna) && !(Mapa[novaLinha, novaColuna] is Parede))
            {
                yield return (novaLinha, novaColuna);
            }
        }
    }

    private bool EhPosicaoValida(int linha, int coluna)
    {
        return linha >= 0 && linha < Mapa.GetLength(0) && coluna >= 0 && coluna < Mapa.GetLength(1);
    }
    private int Heuristica((int, int) a, (int, int) b)
    {
        return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }

    private List<(int, int)> ReconstruirCaminho(Dictionary<(int, int), (int, int)> veioDe, (int, int) atual)
    {
        var caminhoTotal = new List<(int, int)>();

        while (veioDe.ContainsKey(atual))
        {
            var anterior = veioDe[atual];

            caminhoTotal.Add(atual);

            atual = anterior;
        }
        caminhoTotal.Add(atual);

        caminhoTotal.Reverse();

        return caminhoTotal;
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
        public (int, int) Posicao { get; }
        public int GScore { get; }
        public int FScore { get; }

        public Node((int, int) posicao, int gScore, int fScore)
        {
            Posicao = posicao;
            GScore = gScore;
            FScore = fScore;
        }
    }
}
