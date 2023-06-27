namespace MatrixCalculator.Domain.Abstractions;

public interface IMatrix<T> where T : struct
{
    void AddRow(int rowNumber, T[] rowData);
    T[] GetRow(int row);
    T[] GetColumn(int column);
}
