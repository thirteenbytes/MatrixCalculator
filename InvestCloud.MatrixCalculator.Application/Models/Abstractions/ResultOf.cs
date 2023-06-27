namespace InvestCloud.MatrixCalculator.Application.Models.Abstractions;

public abstract record ResultOf<T>
{
    public T Value { get; set; }
    public string Cause { get; set; }
    public bool Success { get; set; }
}
