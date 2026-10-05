FlashCard flashCard = new FlashCard(
    "What is the capital of France?",
    "Paris",
    new List<string> { "Berlin", "Madrid", "Paris", "Rome" }
);

Console.WriteLine("Welcome to the FlashCard App!");
Console.WriteLine($"Question: {flashCard.Question}");
foreach (string option in flashCard.Options)
{
    Console.WriteLine($"- {option}");
}

Console.WriteLine("Please enter your answer:");
string userAnswer = Console.ReadLine();
if (userAnswer.Equals(flashCard.Answer, StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Correct!");
}
else
{
    Console.WriteLine($"Incorrect. The correct answer is: {flashCard.Answer}");
}

public record FlashCard(string Question, string Answer, List<string> Options);