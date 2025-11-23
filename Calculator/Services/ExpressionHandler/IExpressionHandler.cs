namespace Calculator.Services.ExpressionHandler;

public interface IExpressionHandler
{
    string HandleCalculatorInput(string input);
    string HandleBackspace();
    string HandleCommaButton(string input);
    string Clear();
    string SetExpressionString();
    event Action? OnChange;

    string? Expression { get; set; }
}