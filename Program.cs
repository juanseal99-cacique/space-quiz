using System;
using System.Collections.Generic;
using System.Threading;

internal static class Program
{
    private sealed record Question(string Prompt, string[] Answers, int CorrectIndex, string Fact);

    private static readonly Question[] Questions =
    [
        new("Which planet is known as the Red Planet?",
            ["Venus", "Mars", "Jupiter", "Mercury"], 1,
            "Iron oxide (rust) gives Mars its reddish color."),
        new("Who was the first person to walk on the Moon?",
            ["Buzz Aldrin", "Yuri Gagarin", "Neil Armstrong", "Sally Ride"], 2,
            "Neil Armstrong stepped onto the Moon during Apollo 11 in 1969."),
        new("What is the largest planet in our solar system?",
            ["Saturn", "Neptune", "Earth", "Jupiter"], 3,
            "More than 1,000 Earths could fit inside Jupiter."),
        new("What is the name of our galaxy?",
            ["Andromeda", "The Milky Way", "The Whirlpool Galaxy", "The Sombrero Galaxy"], 1,
            "Our solar system lives in a spiral arm of the Milky Way."),
        new("Which spacecraft first carried humans to the Moon's surface?",
            ["Apollo 11", "Voyager 1", "Sputnik 1", "Space Shuttle Columbia"], 0,
            "Apollo 11's lunar module, Eagle, landed on the Moon in July 1969."),
        new("What is a light-year a measure of?",
            ["Time", "Brightness", "Distance", "Speed"], 2,
            "A light-year is the distance light travels in one year."),
        new("Which planet has the most prominent ring system?",
            ["Mars", "Saturn", "Venus", "Mercury"], 1,
            "Saturn's spectacular rings are made mostly of ice and rock."),
        new("What is the name of the first artificial satellite?",
            ["Hubble", "Explorer 1", "Sputnik 1", "Voyager 2"], 2,
            "The Soviet Union launched Sputnik 1 into orbit in 1957."),
        new("What keeps planets in orbit around the Sun?",
            ["Magnetism", "Solar wind", "Gravity", "Moonlight"], 2,
            "The Sun's gravity pulls the planets toward it as they move through space."),
        new("Which telescope launched in 2021 observes mainly infrared light?",
            ["James Webb Space Telescope", "Hubble Space Telescope", "Kepler Space Telescope", "Chandra X-ray Observatory"], 0,
            "Webb uses infrared light to peer through cosmic dust and study the early universe.")
    ];

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var playAgain = true;

        while (playAgain)
        {
            RunQuiz();
            Console.Write("\nPlay again? (y/n): ");
            playAgain = string.Equals(Console.ReadLine()?.Trim(), "y", StringComparison.OrdinalIgnoreCase);
        }
    }

    private static void RunQuiz()
    {
        var score = 0;
        if (!Console.IsOutputRedirected)
            Console.Clear();
        WriteColor("✦ ORBIT", ConsoleColor.Magenta);
        Console.WriteLine("  A LITTLE COSMIC CHALLENGE\n");

        for (var index = 0; index < Questions.Length; index++)
        {
            var item = Questions[index];
            Console.WriteLine($"QUESTION {index + 1:00} / {Questions.Length}                         SCORE  {score}");
            DrawProgress(index);
            Console.WriteLine($"\n{item.Prompt}\n");

            for (var answerIndex = 0; answerIndex < item.Answers.Length; answerIndex++)
                Console.WriteLine($"  {answerIndex + 1}. {item.Answers[answerIndex]}");

            var selected = ReadAnswer();
            if (selected is null)
                return;

            var isCorrect = selected.Value == item.CorrectIndex;
            if (isCorrect)
            {
                score++;
                WriteColor("\n  ✓ Correct! ✨", ConsoleColor.Green);
            }
            else
            {
                AnimateWrongAnswer();
                Console.Write($"  The answer is ");
                WriteColor(item.Answers[item.CorrectIndex], ConsoleColor.Green);
                Console.WriteLine(".");
            }

            Console.WriteLine($"  {item.Fact}");
            Console.WriteLine();
            if (index < Questions.Length - 1)
            {
                Console.Write("  Press Enter for the next question...");
                Console.ReadLine();
            }
        }

        ShowResults(score);
    }

    private static int? ReadAnswer()
    {
        while (true)
        {
            Console.Write("\n  Your answer (1–4): ");
            var line = Console.ReadLine();
            if (line is null)
                return null;

            var input = line.Trim();
            if (int.TryParse(input, out var answer) && answer is >= 1 and <= 4)
                return answer - 1;

            WriteColor("  Enter a number from 1 to 4.", ConsoleColor.Yellow);
            Console.WriteLine();
        }
    }

    private static void DrawProgress(int answered)
    {
        const int width = 20;
        var filled = answered * width / Questions.Length;
        Console.Write("  [");
        WriteColor(new string('■', filled), ConsoleColor.Magenta);
        Console.Write(new string('·', width - filled));
        Console.WriteLine($"] {answered}/{Questions.Length} answered");
    }

    private static void AnimateWrongAnswer()
    {
        var originalColor = Console.ForegroundColor;
        var positions = new[] { "    ", "  ", "      ", "   ", "    " };
        foreach (var spaces in positions)
        {
            Console.Write($"\r{spaces}× Not quite!");
            Console.ForegroundColor = ConsoleColor.Red;
            Thread.Sleep(65);
        }
        Console.ForegroundColor = originalColor;
        Console.WriteLine();
    }

    private static void ShowResults(int score)
    {
        var (emoji, title, message) = score switch
        {
            >= 8 => ("🚀", "You're a space star!", "An out-of-this-world performance. The cosmos is lucky to have you."),
            >= 5 => ("🪐", "Nice exploring!", "You've got some stellar knowledge. Keep looking up!"),
            _ => ("🌱", "A curious beginning!", "Every great explorer starts somewhere. Your cosmic journey is just beginning.")
        };

        Console.WriteLine("\n  MISSION COMPLETE");
        Console.WriteLine($"  {emoji}  {title}");
        Console.WriteLine($"  {message}");
        Console.Write("  SCORE  ");
        WriteColor($"{score} / {Questions.Length}", ConsoleColor.Magenta);
        Console.WriteLine("\n");
    }

    private static void WriteColor(string text, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }
}
