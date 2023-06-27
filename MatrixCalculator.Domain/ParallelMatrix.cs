namespace MatrixCalculator.Domain;

public class ParallelMatrix : Matrix<int>
{
    public ParallelMatrix(int size) : base(size)
    {
    }

    protected override Matrix<int> Multiply(Matrix<int> b)
    {
        var matrixResult = new ParallelMatrix(base.rowTotal);
        Parallel.For(0, this.rowTotal, row =>
        {
            Parallel.For(0, this.columnTotal, column =>
            {
                MultiplyCompute(row, column, this, (ParallelMatrix)b, matrixResult);
            });
        });

        return matrixResult;
    }

    private void MultiplyCompute(int rowIndex, int columnIndex, ParallelMatrix a, ParallelMatrix b, ParallelMatrix result)
    {
        var tempRowIndex = rowIndex;
        var tempColumnIndex = columnIndex;

        var rowData = a.GetRow(tempRowIndex);
        var colData = b.GetColumn(tempColumnIndex);

        for (var i = 0; i < rowData.Length; i++)
        {
            result[rowIndex, columnIndex] += rowData[i] * colData[i];
        }
    }
}
