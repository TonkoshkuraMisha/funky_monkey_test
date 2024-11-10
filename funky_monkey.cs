using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Данные
        int[] reel1 = { 9, 8, 8, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 5, 4, 4, 4, 4, 4,
                        3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                        1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0 };
        
        int[] reel2 = { 9, 8, 8, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 5, 4, 4, 4, 4, 4,
                        3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                        1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0 };

        int[] reel3 = { 9, 8, 8, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 5, 4, 4, 4, 4, 0,
                        3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                        1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0 };

        // Параметры
        long spins = (long)1e6;
        int betPerSpin = 5;

        // Таблица выплат
        Dictionary<(int, int, int), int> paytable = new Dictionary<(int, int, int), int>
        {
            { (1, 1, 1), 60 }, { (1, 1, 2), 30 }, { (1, 1, 3), 30 }, { (1, 2, 1), 30 },
            { (1, 2, 2), 30 }, { (1, 2, 3), 30 }, { (1, 3, 1), 30 }, { (1, 3, 2), 30 },
            { (1, 3, 3), 30 }, { (2, 1, 1), 30 }, { (2, 1, 2), 30 }, { (2, 1, 3), 30 },
            { (2, 2, 1), 30 }, { (2, 2, 2), 120 }, { (2, 2, 3), 30 }, { (2, 3, 1), 30 },
            { (2, 3, 2), 30 }, { (2, 3, 3), 30 }, { (3, 1, 1), 30 }, { (3, 1, 2), 30 },
            { (3, 1, 3), 30 }, { (3, 2, 1), 30 }, { (3, 2, 2), 30 }, { (3, 2, 3), 30 },
            { (3, 3, 1), 30 }, { (3, 3, 2), 30 }, { (3, 3, 3), 180 }, { (4, 4, 4), 240 },
            { (5, 5, 5), 300 }, { (6, 6, 6), 600 }, { (7, 7, 7), 1200 }, { (8, 8, 8), 2500 },
            { (9, 9, 9), 20000 }
        };

        // Статистика выигрышей
        long totalWinnings = 0;
        Dictionary<(int, int, int), int> winCounts = new Dictionary<(int, int, int), int>();
        List<int> winAmounts = new List<int>();

        Random rnd = new Random();
        long progressStep = spins / 10;

        // Симуляция вращений
        for (long spin = 0; spin < spins; spin++)
        {
            int symbol1 = reel1[rnd.Next(reel1.Length)];
            int symbol2 = reel2[rnd.Next(reel2.Length)];
            int symbol3 = reel3[rnd.Next(reel3.Length)];
            var combination = (symbol1, symbol2, symbol3);

            if (paytable.ContainsKey(combination))
            {
                int win = paytable[combination] * betPerSpin;
                totalWinnings += win;
                if (!winCounts.ContainsKey(combination))
                    winCounts[combination] = 0;
                winCounts[combination]++;
                winAmounts.Add(win);
            }
            else
            {
                winAmounts.Add(0);
            }

            // Прогресс каждые 10%
            if ((spin + 1) % progressStep == 0)
            {
                int progress = (int)((spin + 1) * 100 / spins);
                Console.WriteLine($"{DateTime.Now:HH:mm:ss}: {new string('*', progress)}{new string('-', 100 - progress)} {progress}% Done.");
            }
        }

        // Расчёт RTP
        double totalBet = spins * betPerSpin;
        double overallRTP = (totalWinnings / totalBet) * 100;
        var combinationRTP = winCounts.ToDictionary(
            combo => combo.Key,
            combo => (combo.Value * paytable[combo.Key] * betPerSpin / totalBet) * 100
        );

        // Распределение выигрышей по интервалам
        int[] intervals = { 0, 30, 60, 120, 180, 240, 300, 600, 1200, 2500, 20000 };
        Dictionary<string, int> winDistribution = new Dictionary<string, int>();
        foreach (var win in winAmounts)
        {
            for (int i = 0; i < intervals.Length - 1; i++)
            {
                if (intervals[i] < win && win <= intervals[i + 1])
                {
                    string intervalKey = $"{intervals[i] + 1}-{intervals[i + 1]}";
                    if (!winDistribution.ContainsKey(intervalKey))
                        winDistribution[intervalKey] = 0;
                    winDistribution[intervalKey]++;
                    break;
                }
            }
        }

        // Волатильность и стандартное отклонение
        double winStd = Math.Sqrt(winAmounts.Select(w => Math.Pow(w - winAmounts.Average(), 2)).Sum() / winAmounts.Count);
        double volatility = winStd / betPerSpin;

        // Вывод результатов
        Console.WriteLine($"Общий выигрыш: {totalWinnings}");
        Console.WriteLine($"Общая ставка: {totalBet}");
        Console.WriteLine($"Общее RTP: {overallRTP:F2}%\n");

        Console.WriteLine("RTP для каждой комбинации:");
        foreach (var combo in combinationRTP)
            Console.WriteLine($"{combo.Key}: {combo.Value:F2}%");

        Console.WriteLine("Частота выпадения комбинаций:");
        foreach (var combo in winCounts.OrderBy(x => x.Key))
            Console.WriteLine($"{combo.Key}: {combo.Value} per {spins} spins");

        Console.WriteLine("\nРаспределение выигрышей по интервалам:");
        foreach (var interval in winDistribution.OrderBy(x => int.Parse(x.Key.Split('-')[0])))
            Console.WriteLine($"{interval.Key}: {interval.Value} раз");

        Console.WriteLine($"\nСтандартное отклонение выигрышей: {winStd:F2}");
        Console.WriteLine($"Волатильность: {volatility:F2}\n");
    }
}
