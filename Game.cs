using System.Collections.Generic;
using System.Xml.Linq;
using UniversalGameEngine.Games;

namespace UniversalGameEngine
{
    public abstract class Game
    {
        public const string Space = "      ";

        public abstract int PlayerCount { get; }

        public abstract int CurrentPlayer { get; }

        public abstract double Evaluate(int player);

        public abstract List<object> GetLegalMoves();

        public abstract Game Simulate(object move);

        public abstract bool IsOver();

        public abstract Game Clone();

        public float f;

        public float g;

        public float eval;

        public Game? Parent = null;

        public List<Game> Children = new List<Game>();

        public void PrintAsTree(int spaces = 0)
        {
            for (int i = 0; i < spaces; i++)
                Console.Write(Space);

            Console.WriteLine($"F - {f}. G - {g}, Evaluation - {eval}");

            Console.WriteLine((this as EightPuzzle).ToString(spaces));

            foreach (var pair in Children)
                pair.PrintAsTree(spaces + 1);
        }

        public void PrintWinningPath()
        {
            Game winner = FindWinningNode();

            if (winner == null)
            {
                Console.WriteLine("Победный путь не найден.");
                return;
            }

            var path = new List<Game>();
            for (Game cur = winner; cur != null; cur = cur.Parent)
                path.Add(cur);

            path.Reverse();

            for (int i = 0; i < path.Count; i++)
            {
                var node = path[i];

                Console.WriteLine($"F - {node.f}, G - {node.g}, Evaluation - {node.eval}");
                Console.WriteLine(node);
            }
        }
        private Game FindWinningNode()
        {
            if (IsOver())
                return this;

            foreach (var pair in Children)
            {
                var found = pair.FindWinningNode();
                if (found != null)
                    return found;
            }

            return null;
        }
    }
}