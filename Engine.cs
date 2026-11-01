namespace UniversalGameEngine
{
    public class Engine
    {
        private Game _game;

        private const int FOUND = -1;
        
        public Engine(Game game) => UpdateEngine(game);

        public void UpdateEngine(Game game) => _game = game ?? throw new ArgumentNullException(nameof(game));

        public object? FindBestMove()
        {
            if (_game.IsOver()) 
                return null;

            int threshold = (int) _game.Evaluate(_game.CurrentPlayer);

            while (true)
            {
                
                var path = new List<object>();
                var visited = new HashSet<Game>();

                int t = Search(_game.Parent, _game, 0, threshold, path, visited);

                if (t == FOUND) 
                    return path[0];

                if (t == int.MaxValue)
                    return null;

                threshold = t;
            }
        }

        int Search(Game parent, Game state, int g, int threshold, List<object> path, HashSet<Game> visited)
        {
            var evaluation = (int)state.Evaluate(state.CurrentPlayer);

            int f = g + evaluation;
            
            state.Parent = parent;

            state.f = f;

            state.g = g;

            state.eval = evaluation;

            state.Parent?.Children.Add(state);

            if (f > threshold)
            {
                return f;
            }   

            if (state.IsOver())
            {
                return FOUND;
            }
            
            int min = int.MaxValue;

            foreach (var move in state.GetLegalMoves())
            {
                Game next = state.Simulate(move);

                if (visited.Contains(next))
                    continue;

                visited.Add(next);
                path.Add(move);
                int t = Search(state, next, g + 1, threshold, path, visited);

                if (t == FOUND) 
                    return FOUND;

                if (t < min) 
                    min = t;
                
                path.RemoveAt(path.Count - 1);
                visited.Remove(next);
            }

            return min;
        }
    }
}