namespace Calculator.Services.ExpressionHandler;

public interface IExpressionHandler
{
    string HandleCalculatorInput(string input);
    string HandleBackspace();
    void HandleEvalButton();
    string HandleCommaButton(string input);
    void Clear();
    string SetExpressionString();
    event Action? OnChange;
    void SetHasCalculated();
    

    bool HasCalculated { get; set; }
    string? Expression { get; set; }
    string? Result { get; set; }
}