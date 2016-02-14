using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace MatrixRotation
{
    public static class Solution
    {
        internal static void Main()
        {
            Problem.Solve(Console.In, Console.Out);
        }
    }

    public static class Problem
    {
        public static void Solve(TextReader reader, TextWriter writer)
        {
            var rotationInput = MatrixRotationInput.FromReader(reader);
            var matrix = rotationInput.Matrix;
            int rotations = rotationInput.Rotations;
            var rotatedMatrix = matrix.Rotate(rotations);

            rotatedMatrix.Write(writer);
        }

    }
    public class MatrixRotationInput
    {
        public int Rotations { get; private set; }

        public Matrix<string> Matrix { get; private set; }

        private MatrixRotationInput(int rotations, Matrix<string> matrix)
        {
            if (rotations < 0)
            {
                throw new ArgumentOutOfRangeException("rotations", "Cannot have negative rotations");
            }

            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }

            Rotations = rotations;
            Matrix = matrix;
        }

        public static MatrixRotationInput FromReader(TextReader input)
        {
            var allInput = input.ReadToEnd();
            var lines = allInput.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);



            string[] tokens = lines[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var rowCount = Convert.ToInt32(tokens[0]);
            var columnCount = Convert.ToInt32(tokens[1]);
            var rotations = Convert.ToInt32(tokens[2]);
            var values = new Matrix<string>(rowCount, columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                var line = lines[row + 1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int column = 0; column < columnCount; column++)
                {
                    values[row, column] = line[column];
                }
            }

            return new MatrixRotationInput(rotations, values);
        }


    }

    public class Matrix<T>
    {
        private readonly T[,] _values;

        public Matrix(int rowCount, int columnCount)
        {
            if (rowCount < 0)
            {
                throw new ArgumentOutOfRangeException("rowCount", "Cannot have negative row count");
            }

            if (columnCount < 0)
            {
                throw new ArgumentOutOfRangeException("columnCount", "Cannot have negative column count");
            }

            RowCount = rowCount;
            ColumnCount = columnCount;
            _values = new T[RowCount, ColumnCount];
        }

        public int RowCount { get; private set; }

        public int ColumnCount { get; private set; }

        public T this[int rowIndex, int columnIndex]
        {
            get { return _values[rowIndex, columnIndex]; }
            set
            {
                _values[rowIndex, columnIndex] = value;
            }
        }


        public Matrix<T> Rotate(int numberOfRotations)
        {
            if (Math.Min(RowCount, ColumnCount) % 2 != 0)
            {
                throw new InvalidOperationException("Cannot rotate if Min(RowCount, ColumnCount) % 2 != 0");
            }

            if (numberOfRotations < 0)
            {
                throw new ArgumentOutOfRangeException("numberOfRotations", "Cannot have negative rotations");
            }

            int frameworkCount = Math.Min(RowCount, ColumnCount) / 2;
            Framework<T>[] frameworks = new Framework<T>[frameworkCount];
            for (int i = 0; i < frameworkCount; i++)
            {
                frameworks[i] = Framework<T>.FromMatrix(this, i, numberOfRotations);
            }

            return SetMatrixFromFrameworks(frameworks);
        }

        private Matrix<T> SetMatrixFromFrameworks(IReadOnlyList<Framework<T>> frameworks)
        {
            var result = new Matrix<T>(RowCount, ColumnCount);
            PositionOnMatrixFramewokStyleEnumerator positionEnumerator = new PositionOnMatrixFramewokStyleEnumerator(RowCount, ColumnCount);
            foreach (Framework<T> framework in frameworks)
            {
                for (int j = 0; j < framework.ElementCount; j++)
                {
                    positionEnumerator.MoveNext();
                    result[positionEnumerator.Current.Row, positionEnumerator.Current.Column] = framework[j];
                }
            }
            return result;
        }
    }

    public class Framework<T>
    {
        private readonly T[] _values;
        private readonly int _startIndex;
        private Framework(T[] values, int startIndex)
        {

            _values = values;
            if (startIndex >= ElementCount)
            {
                startIndex = startIndex % ElementCount;
            }
            _startIndex = startIndex;
        }
        public static Framework<T> FromMatrix(Matrix<T> matrix, int frameworkIndex, int startIndex)
        {
            if (frameworkIndex >= Math.Min(matrix.RowCount, matrix.ColumnCount) / 2)
            {
                throw new ArgumentOutOfRangeException("frameworkIndex", "frameworkIndex must be lower than Math.Min(matrix.RowCount, matrix.ColumnCount) / 2");
            }

            var enumerator = new PositionOnMatrixFramewokStyleEnumerator(matrix.RowCount, matrix.ColumnCount, frameworkIndex);
            var values = new T[enumerator.TotalElements];

            for (int i = 0; i < values.Length; i++)
            {
                enumerator.MoveNext();
                values[i] = matrix[enumerator.Current.Row, enumerator.Current.Column];

            }
            return new Framework<T>(values, startIndex);
        }
        public int ElementCount { get { return _values.Length; } }

        public T this[int index]
        {
            get
            {
                if (index + _startIndex < ElementCount)
                {
                    return _values[index + _startIndex];
                }
                else
                {
                    return _values[index - (ElementCount - _startIndex)];
                }
            }

        }
    }


    public class PositionOnMatrixFramewokStyleEnumerator : IEnumerator<PositionOnMatrix>
    {
        private PositionOnMatrix _position;
        private int? _frameworkIndex;
        private int _currentIndex;
        private readonly int _columnCount;
        private readonly int _rowCount;

        public PositionOnMatrixFramewokStyleEnumerator(int rowCount, int columnCount, int? frameworkIndex = null)
        {
            if (rowCount <= 0)
            {
                throw new ArgumentOutOfRangeException("rowCount", "rowCount must be greater than 0");
            }

            if (columnCount <= 0)
            {
                throw new ArgumentOutOfRangeException("columnCount", "columnCount must be greater than 0");
            }

            if (frameworkIndex + 1 > Math.Min(rowCount, columnCount) / 2)
            {
                throw new ArgumentOutOfRangeException("frameworkIndex", "frameworwIndex must be lower than Math.Min(rowCount,columncount) / 2");
            }

            _currentIndex = -1;
            _position = null;
            _columnCount = columnCount;
            _rowCount = rowCount;
            _frameworkIndex = frameworkIndex;
        }

        public int TotalElements
        {
            get
            {
                if (_frameworkIndex.HasValue)
                {
                    return (_columnCount - 2 * _frameworkIndex.Value) * 2 + (_rowCount - 2 * _frameworkIndex.Value - 2) * 2;
                }

                return _columnCount * _rowCount;
            }
        }

        object IEnumerator.Current
        {
            get { return _position; }
        }

        public void Reset()
        {
            _currentIndex = -1;
            _position = null;
        }

        public PositionOnMatrix Current
        {
            get { return _position; }
        }

        public void Dispose()
        {

        }
        public bool MoveNext()
        {
            if (_currentIndex + 1 >= TotalElements)
            {
                return false;
            }

            if (_position == null)
            {
                if (!_frameworkIndex.HasValue)
                {
                    _position = new PositionOnMatrix(0, 0, _rowCount, _columnCount);
                    _currentIndex = 0;

                    return true;
                }
                else
                {
                    _position = new PositionOnMatrix(_frameworkIndex.Value, _frameworkIndex.Value, _rowCount, _columnCount);
                    _currentIndex = 0;

                    return true;
                }
            }
            else
            {
                if (MustGoLeft())
                {
                    _position = new PositionOnMatrix(_position.Row, _position.Column + 1, _rowCount, _columnCount);
                    _currentIndex += 1;

                    return true;
                }
                if (MustGoDown())
                {
                    _position = new PositionOnMatrix(_position.Row + 1, _position.Column, _rowCount, _columnCount);
                    _currentIndex += 1;

                    return true;
                }
                if (MustGoRight())
                {
                    _position = new PositionOnMatrix(_position.Row, _position.Column - 1, _rowCount, _columnCount);
                    _currentIndex += 1;

                    return true;
                }
                if (MustGoUp())
                {
                    _position = new PositionOnMatrix(_position.Row - 1, _position.Column, _rowCount, _columnCount);
                    _currentIndex += 1;

                    return true;
                }
                if (MustGoInner())
                {
                    _position = new PositionOnMatrix(_position.Row, _position.Column + 1, _rowCount, _columnCount);
                    _currentIndex += 1;

                    return true;
                }
                throw new InvalidOperationException("Should not get here");
            }

        }

        private bool MustGoInner()
        {
            return _position.Column <= _columnCount / 2 &&
                _position.Row == _position.Column + 1;
        }

        private bool MustGoUp()
        {
            return _position.Column < _columnCount / 2 &&
                _position.Row >= _position.Column + 2 &&
                _position.Row <= _rowCount - 1 - _position.Column;
        }

        private bool MustGoRight()
        {
            return _position.Row >= _rowCount / 2 &&
                _position.Column >= _rowCount - 1 - _position.Row + 1 &&
                _position.Column <= _columnCount - 1 - (_rowCount - 1) + _position.Row;
        }

        private bool MustGoDown()
        {
            return _position.Column >= _columnCount / 2 &&
               _position.Row >= _columnCount - 1 - _position.Column &&
               _position.Row <= _rowCount - 1 - (_columnCount - 1) + _position.Column - 1;
        }

        private bool MustGoLeft()
        {
            return _position.Row < _rowCount / 2 &&
                _position.Column >= _position.Row &&
                _position.Column <= _columnCount - 1 - _position.Row - 1;
        }


    }


    public class PositionOnMatrix
    {
        public PositionOnMatrix(int row, int column, int rowCount, int columnCount)
        {
            if (row >= rowCount)
            {
                throw new ArgumentOutOfRangeException("row", "Row must be lower than RowCount");
            }
            if (column >= columnCount)
            {
                throw new ArgumentOutOfRangeException("row", "Row must be lower than RowCount");
            }

            Row = row;
            Column = column;

        }

        public int Row { get; private set; }

        public int Column { get; private set; }
    }

    public static class MatrixExtensions
    {

        public static void Write<T>(this Matrix<T> matrix, TextWriter output)
        {
            for (int r = 0; r < matrix.RowCount; r++)
            {
                T[] row = new T[matrix.ColumnCount];
                for (int c = 0; c < matrix.ColumnCount; c++)
                {
                    row[c] = matrix[r, c];
                }

                output.WriteLine(string.Join(" ", row));
            }
        }

    }
}