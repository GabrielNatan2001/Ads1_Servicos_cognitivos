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
            var start = (linhaEntrada, colunaEntrada);
            var goal = (humanoLinha, humanoColuna);

            var openList = new PriorityQueue<Node, int>();
            var cameFrom = new Dictionary<(int, int), (int, int)>();
            var gScore = new Dictionary<(int, int), int>();
            var fScore = new Dictionary<(int, int), int>();

            foreach (var pos in GetAllPositions())
            {
                gScore[pos] = int.MaxValue;
                fScore[pos] = int.MaxValue;
            }

            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

            openList.Enqueue(new Node(start, 0, Heuristic(start, goal)), fScore[start]);

            while (openList.Count > 0)
            {
                var current = openList.Dequeue().Position;

                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current);
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (Mapa[neighbor.Item1, neighbor.Item2] is Parede) continue;

                    var tentativeGScore = gScore[current] + 1;

                    if (tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                        openList.Enqueue(new Node(neighbor, gScore[neighbor], fScore[neighbor]), fScore[neighbor]);
                    }
                }
            }

            return new List<(int, int)>(); // Não encontrou caminho
        }

        private IEnumerable<(int, int)> GetNeighbors((int, int) pos)
        {
            var directions = new (int, int)[]
            {
            (-1, 0), (1, 0), (0, -1), (0, 1) // Up, Down, Left, Right
            };

            foreach (var direction in directions)
            {
                var newRow = pos.Item1 + direction.Item1;
                var newCol = pos.Item2 + direction.Item2;

                if (IsValidPosition(newRow, newCol) && Mapa[newRow, newCol] is Caminho)
                {
                    yield return (newRow, newCol);
                }
            }
        }
        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < Mapa.GetLength(0) && col >= 0 && col < Mapa.GetLength(1);
        }

        private int Heuristic((int, int) a, (int, int) b)
        {
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2); // Manhattan Distance
        }

        private List<(int, int)> ReconstructPath(Dictionary<(int, int), (int, int)> cameFrom, (int, int) current)
        {
            var totalPath = new List<(int, int)> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Add(current);
            }
            totalPath.Reverse();
            return totalPath;
        }

        private IEnumerable<(int, int)> GetAllPositions()
        {
            for (int row = 0; row < Mapa.GetLength(0); row++)
            {
                for (int col = 0; col < Mapa.GetLength(1); col++)
                {
                    yield return (row, col);
                }
            }
        }

        private class Node
        {
            public (int, int) Position { get; }
            public int GScore { get; }
            public int FScore { get; }

            public Node((int, int) position, int gScore, int fScore)
            {
                Position = position;
                GScore = gScore;
                FScore = fScore;
            }
        }
    }
}
