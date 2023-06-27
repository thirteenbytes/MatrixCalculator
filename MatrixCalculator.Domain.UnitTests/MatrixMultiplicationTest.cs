namespace MatrixCalculator.Domain.UnitTests
{
    public class MatrixMultiplicationTest
    {
        [Fact]
        public void Multiple_2by2_Matrices()
        {
            // Arrange
            var matrixA = new ParallelMatrixCalculator(2);
            var matrixB = new ParallelMatrixCalculator(2);

            var row0 = new List<int> { 2, 3 };
            var row1 = new List<int> { 1, 4 };

            matrixA.AddRow(0, row0.ToArray());
            matrixA.AddRow(1, row1.ToArray());

            row0 = new List<int> { 3, 2 };
            row1 = new List<int> { 1, -6 };

            matrixB.AddRow(0, row0.ToArray());
            matrixB.AddRow(1, row1.ToArray());

            // Act
            var resultMatrix = matrixA * matrixB;

            var actualResult = resultMatrix.ToString();
            var expectedResult = "9-147-22";

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Multiple_3by3_Matrices()
        {
            // Arrange
            var matrixA = new ParallelMatrixCalculator(3);
            var matrixB = new ParallelMatrixCalculator(3);

            var row0 = new List<int> { 22, 8, -4 };
            var row1 = new List<int> { -1, 1, 34 };
            var row2 = new List<int> { 19, 0, 1 };

            matrixA.AddRow(0, row0.ToArray());
            matrixA.AddRow(1, row1.ToArray());
            matrixA.AddRow(2, row2.ToArray());

            row0 = new List<int> { 14, -1, 0 };
            row1 = new List<int> { 33, -4, 2 };
            row2 = new List<int> { 9, -1, 1 };

            matrixB.AddRow(0, row0.ToArray());
            matrixB.AddRow(1, row1.ToArray());
            matrixB.AddRow(2, row2.ToArray());

            // Act
            var resultMatrix = matrixA * matrixB;

            var actualResult = resultMatrix.ToString();
            var expectedResult = "536-5012325-3736275-201";

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Multiple_2by2_EmptyMatrixA()
        {
            var matrixA = new ParallelMatrixCalculator(2);
            var matrixB = new ParallelMatrixCalculator(2);

            var row0 = new List<int> { 1, 1 };
            var row1 = new List<int> { 1, 1 };

            matrixB.AddRow(0, row0.ToArray());
            matrixB.AddRow(1, row1.ToArray());

            // Act
            var resultMatrix = matrixA * matrixB;

            var actualResult = resultMatrix.ToString();
            var expectedResult = "0000";

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}