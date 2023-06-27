namespace MatrixCalculator.Domain;

public class ParallelMatrixCalculator : Matrix<int>
{
    public ParallelMatrixCalculator(int size) : base(size)
    {
    }

    protected override Matrix<int> Multiply(Matrix<int> b)
    {
        var matrixResult = new ParallelMatrixCalculator(base.rowTotal);
        Parallel.For(0, this.rowTotal, row =>
        {
            Parallel.For(0, this.columnTotal, column =>
            {
                MultiplyCompute(row, column, this, (ParallelMatrixCalculator)b, matrixResult);
            });
        });

        return matrixResult;
    }

    private void MultiplyCompute(int rowIndex, int columnIndex, ParallelMatrixCalculator a, ParallelMatrixCalculator b, ParallelMatrixCalculator result)
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
