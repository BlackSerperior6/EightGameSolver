using UniversalGameEngine;
using UniversalGameEngine.Games;

Console.WriteLine("=== Пятнашки (одиночная игра) ===");
PlaySingle(new EightPuzzle());

static void PlaySingle(Game game)
{
    Game original = game;

    var engine = new Engine(game);
    int step = 0;

    while (!game.IsOver() && step < 200)
    {
        var move = engine.FindBestMove();

        game = game.Simulate(move);

        Console.WriteLine($"Ход: {step}");
        Console.WriteLine(game);

        engine.UpdateEngine(game);
        step++;
    }

    Console.WriteLine($"Финал (ход {step}):");
    Console.WriteLine(game);
    Console.WriteLine(game.IsOver() ? "Решено!" : "Не решено за отведённые ходы");

    Console.WriteLine("Дерево решений!");
    original.PrintAsTree();
}
