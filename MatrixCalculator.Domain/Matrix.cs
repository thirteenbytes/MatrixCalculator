namespace MatrixCalculator.Domain;

public abstract class Matrix<T> : IMatrix<T> where T : struct
{
    private T[,] data { get; set; }

    protected readonly int rowTotal;
    protected readonly int columnTotal;
    protected T this[int row, int column]
    {
        get { return data[row, column]; }
        set { data[row, column] = value; }
    }

    protected abstract Matrix<T> Multiply(Matrix<T> b);

    public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b) =>
        a.Multiply(b);

    public Matrix(int size)
    {
        rowTotal = size;
        columnTotal = size;
        data = new T[rowTotal, columnTotal];
    }
    public void AddRow(int rowNumber, T[] rowData)
    {
        for (var columnNumber = 0; columnNumber < columnTotal; columnNumber++)
        {
            data[rowNumber, columnNumber] = rowData[columnNumber];
        }
    }

    public T[] GetColumn(int column)
    {
        var result = new T[columnTotal];
        for (var row = 0; row < rowTotal; row++)
        {
            result[row] = data[row, column];
        }
        return result;
    }

    public T[] GetRow(int row)
    {
        var result = new T[columnTotal];
        for (var column = 0; column < columnTotal; column++)
        {
            result[column] = data[row, column];
        }
        return result;
    }

    public override string ToString() =>
         string.Concat(Enumerable
            .Range(0, data.GetUpperBound(0) + 1)
            .Select(x => Enumerable.Range(0, data.GetUpperBound(1) + 1)
            .Select(y => data[x, y]))
            .Select(z => string.Concat(z)));
}
