namespace UniversalGameEngine.Games
{
    public class EightPuzzle : Game
    {
        private readonly int[] _board;
        private static readonly int[] Goal = { 1, 2, 3, 8, 0, 4, 7, 6, 5 };

        public override int PlayerCount => 1;
        public override int CurrentPlayer => 0;

        public EightPuzzle() : this(new[] { 2, 1, 6, 4, 0, 8, 7, 5, 3 }) { }
        private EightPuzzle(int[] b) { _board = (int[])b.Clone(); }

        public override double Evaluate(int player)
        {
            int manhattan = 0;
            int linearConflicts = 0;

            int[] goalIndex = new int[9];
            for (int i = 0; i < 9; i++)
                goalIndex[Goal[i]] = i;

            for (int i = 0; i < 9; i++)
            {
                int v = _board[i];
                if (v == 0) continue;

                int goal = goalIndex[v];
                manhattan += Math.Abs(i / 3 - goal / 3) + Math.Abs(i % 3 - goal % 3);
            }

            for (int r = 0; r < 3; r++)
            {
                for (int c1 = 0; c1 < 3; c1++)
                {
                    int v1 = _board[r * 3 + c1];
                    if (v1 == 0) continue;

                    int goal1 = goalIndex[v1];
                    if (goal1 / 3 != r) continue;

                    for (int c2 = c1 + 1; c2 < 3; c2++)
                    {
                        int v2 = _board[r * 3 + c2];
                        if (v2 == 0) continue;

                        int goal2 = goalIndex[v2];
                        if (goal2 / 3 != r) continue;

                        if (goal1 > goal2)
                            linearConflicts++;
                    }
                }
            }

            for (int c = 0; c < 3; c++)
            {
                for (int r1 = 0; r1 < 3; r1++)
                {
                    int v1 = _board[r1 * 3 + c];
                    if (v1 == 0) continue;

                    int goal1 = goalIndex[v1];
                    if (goal1 % 3 != c) continue;

                    for (int r2 = r1 + 1; r2 < 3; r2++)
                    {
                        int v2 = _board[r2 * 3 + c];
                        if (v2 == 0) continue;

                        int goal2 = goalIndex[v2];
                        if (goal2 % 3 != c) continue;

                        if (goal1 > goal2)
                            linearConflicts++;
                    }
                }
            }

            return manhattan + 2 * linearConflicts;
        }

        public override List<object> GetLegalMoves()
        {
            var moves = new List<object>();
            int zero = Array.IndexOf(_board, 0);
            int r = zero / 3, c = zero % 3;
            foreach (var (dr, dc) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                int nr = r + dr, nc = c + dc;
                if (nr is >= 0 and < 3 && nc is >= 0 and < 3)
                    moves.Add(nr * 3 + nc);
            }
            return moves;
        }

        public override Game Simulate(object move)
        {
            int target = (int)move;
            var copy = (int[])_board.Clone();
            int zero = Array.IndexOf(copy, 0);
            copy[zero] = copy[target];
            copy[target] = 0;

            var newPuzzle = new EightPuzzle(copy);
            return newPuzzle;
        }

        public override bool IsOver()
        {
            for (int i = 0; i < 9; i++) if (_board[i] != Goal[i]) return false;
            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not EightPuzzle eightPuzzle || obj == null)
                return false;

            var result = _board.SequenceEqual(eightPuzzle._board);
            return result;
        }

        public override Game Clone() => new EightPuzzle(_board);

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                    sb.Append(_board[r * 3 + c] == 0 ? " . " : $" {_board[r * 3 + c]} ");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string ToString(int spaces)
        {
            var sb = new System.Text.StringBuilder();

            for (int r = 0; r < 3; r++)
            {
                for (int j = 0; j < spaces; j++)
                    sb.Append(Space);

                for (int c = 0; c < 3; c++)
                    sb.Append(_board[r * 3 + c] == 0 ? " . " : $" {_board[r * 3 + c]} ");

                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}