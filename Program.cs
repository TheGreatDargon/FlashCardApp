using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;


List<FlashCard> cards = await CsvHelper("flashcards.csv");
var numCorrect = 0;
var numIncorrect = 0;

Console.WriteLine("Welcome to the FlashCard App!");

foreach(var card in cards)
{
    Console.WriteLine($"Question: {card.Question}");

    foreach (var option in card.Options)
    {
        Console.WriteLine($"- {option}");
    }

    Console.WriteLine("Please enter your answer:");
    string userAnswer = Console.ReadLine();
    if (userAnswer.Equals(card.Answer, StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Correct!");
        numCorrect++;
    }
    else
    {
        Console.WriteLine($"Incorrect. The correct answer is: {card.Answer}\n");
        numIncorrect++;
    }

}
var total = numCorrect+numIncorrect;
double percentage = total > 0 ? numCorrect / (double)total * 100 : 0;
Console.WriteLine($"You got {numCorrect} correct, and {numIncorrect} incorrect. {numCorrect}/{total} or {percentage}%");


static async Task<List<FlashCard>> CsvHelper(string path, bool header = true)
{
    List<FlashCard> flashCards = [];
    using (StreamReader reader = new StreamReader(path))
    {
        string line;
        char[] whiteSpace= {' ', '\t', '\r', '\n'};

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            if (header)
            {
                header = false;
                continue;
            }

            var parts = SplitCsvLine(line);

            if (parts.Count < 2) continue;

            string question = parts[0].Trim(whiteSpace);
            string answer = parts[1].Trim(whiteSpace);
            List<string> options = parts[2..].Select(opt => opt.Trim(whiteSpace)).ToList();
            flashCards.Add(new FlashCard(question, answer, options));
        }
    }

    return flashCards;

}

static List<string> SplitCsvLine(string line)
{
    List<string> columns = [];
    StringBuilder currentField = new StringBuilder("");
    bool inQuotes = false;

    for (int i = 0; i<line.Length; i++)
    {
        var currentChar = line[i];

        if (currentChar == '"')
        {
            // Escaped double quotes
            if (i + 1 < line.Length && line[i + 1] == '"')
            {
                currentField.Append('"');
                i++;
            }
            else
            {
                inQuotes = !inQuotes;
            }

        }
        else if (currentChar == ',' && !inQuotes)
        {
            if (inQuotes)
            {
                currentField.Append(currentChar);
            }
            else
            {
                columns.Add(currentField.ToString());
                currentField.Clear();
            } 
        }
        else
        {
            currentField.Append(currentChar);
        }

    }

    columns.Add(currentField.ToString());
    return columns;
}

public record FlashCard(string Question, string Answer, List<string> Options);