namespace Calculator.Services.NCalcCalculator;

public class NCalcCalculator : INCalcCalculator
{
    public object Evaluate(string expression)
    {
        var expr = new NCalc.Expression(expression);
        return expr.Evaluate();
    }
}