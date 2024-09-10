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
            var start = (linhaEntrada, colunaEntrada, 0); // 0 = Orientação para cima (norte)
            var goal = (humanoLinha, humanoColuna);

            var openList = new PriorityQueue<Node, int>();
            var cameFrom = new Dictionary<(int, int, int), (int, int, int)>();
            var gScore = new Dictionary<(int, int, int), int>();
            var fScore = new Dictionary<(int, int, int), int>();

            foreach (var pos in GetAllPositions())
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    gScore[(pos.Item1, pos.Item2, dir)] = int.MaxValue;
                    fScore[(pos.Item1, pos.Item2, dir)] = int.MaxValue;
                }
            }

            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

            openList.Enqueue(new Node(start, 0, Heuristic(start, goal)), fScore[start]);

            while (openList.Count > 0)
            {
                var current = openList.Dequeue().Position;

                if ((current.Item1, current.Item2) == goal)
                {
                    var path = ReconstructPath(cameFrom, current);
                    return path;
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    var tentativeGScore = gScore[current] + (neighbor.Item3 == current.Item3 ? 1 : 2); // 1 para avanço, 2 para giro

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

        private IEnumerable<(int, int, int)> GetNeighbors((int, int, int) pos)
        {
            var directions = new (int, int)[]
            {
            (-1, 0), (1, 0), (0, -1), (0, 1) // Up, Down, Left, Right
            };

            var orientation = pos.Item3;
            var moveDirection = directions[orientation];

            // Avançar
            var forwardRow = pos.Item1 + moveDirection.Item1;
            var forwardCol = pos.Item2 + moveDirection.Item2;

            if (IsValidPosition(forwardRow, forwardCol) && !(Mapa[forwardRow, forwardCol] is Parede))
            {
                yield return (forwardRow, forwardCol, orientation); // Avançar mantém a mesma orientação
            }

            // Girar 90 graus à direita
            var newOrientation = (orientation + 1) % 4;
            yield return (pos.Item1, pos.Item2, newOrientation); // Gira para a direita
        }

        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < Mapa.GetLength(0) && col >= 0 && col < Mapa.GetLength(1);
        }

        private int Heuristic((int, int, int) a, (int, int) b)
        {
            // Considera apenas a posição (despreza a orientação)
            return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2); // Manhattan Distance
        }

        private List<(int, int)> ReconstructPath(Dictionary<(int, int, int), (int, int, int)> cameFrom, (int, int, int) current)
        {
            var totalPath = new List<(int, int)>();
            var commands = new List<char>();

            while (cameFrom.ContainsKey(current))
            {
                var previous = cameFrom[current];
                if (current.Item3 == previous.Item3)
                {
                    // Movimento para frente
                    commands.Add('A');
                }
                else
                {
                    // Giro
                    commands.Add('G');
                }

                totalPath.Add((current.Item1, current.Item2));
                current = previous;
            }
            totalPath.Reverse();
            commands.Reverse();

            // Log dos comandos
            LogCommands(commands);

            return totalPath;
        }

        private void LogCommands(List<char> commands)
        {
            var logFilePath = "comandos.txt";
            using (StreamWriter sw = new StreamWriter(logFilePath))
            {
                sw.WriteLine(string.Join(",", commands));
            }
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
            public (int, int, int) Position { get; }
            public int GScore { get; }
            public int FScore { get; }

            public Node((int, int, int) position, int gScore, int fScore)
            {
                Position = position;
                GScore = gScore;
                FScore = fScore;
            }
        }
    }
}
