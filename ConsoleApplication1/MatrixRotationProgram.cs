using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;

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

            int frameCount = Math.Min(RowCount, ColumnCount) / 2;
            var frames = Enumerable.Range(0, frameCount)
                .Select(i => Frame<T>.FromMatrix(this, i, numberOfRotations))
                .ToList();

            return BuildMatrixFromFrames(frames);
        }

        private Matrix<T> BuildMatrixFromFrames(IReadOnlyList<Frame<T>> frames)
        {
            var result = new Matrix<T>(RowCount, ColumnCount);
            foreach (Frame<T> frame in frames)
            {
                frame.SetFrameInMatrix(ref result);
            }

            return result;
        }
    }

    public class Frame<T>
    {
        private readonly T[] _values;
        private readonly int _startIndex;
        private readonly int _frameIndex;
        private readonly int _rowCount;
        private readonly int _columnCount;
        private readonly int _frameRowCount;
        private readonly int _frameColumnCount;

        private Frame(T[] values, int startIndex,int frameIndex,int rowCount,int columnCount)
        {

            _values = values;
            if (startIndex >= ElementCount)
            {
                startIndex = startIndex % ElementCount;
            }

            _startIndex = startIndex;
            _frameIndex = frameIndex;
            _rowCount = rowCount;
            _columnCount = columnCount;
            _frameRowCount = CalculateFrameRowCount();
            _frameColumnCount = CalculateFrameColumnCount();
        }

        private int CalculateFrameRowCount() {
            return _rowCount - (2 * _frameIndex);
        }

        private int CalculateFrameColumnCount()
        {
            return _columnCount - (2 * _frameIndex);
        }

        public static Frame<T> FromMatrix(Matrix<T> matrix, int frameIndex, int startIndex)
        {
            if (frameIndex >= Math.Min(matrix.RowCount, matrix.ColumnCount) / 2)
            {
                throw new ArgumentOutOfRangeException("frameIndex", "frameIndex must be lower than Math.Min(matrix.RowCount, matrix.ColumnCount) / 2");
            }

            var enumerable = new PositionOnMatrixFrameStyleEnumerable(matrix.RowCount, matrix.ColumnCount, frameIndex);
            var values = enumerable.Select(x => matrix[x.Row, x.Column]).ToArray();
            return new Frame<T>(values, startIndex,frameIndex,matrix.RowCount,matrix.ColumnCount);
        }

        public void SetFrameInMatrix(ref Matrix<T> matrix)
        {
            var enumerable = new PositionOnMatrixFrameStyleEnumerable(_rowCount, _columnCount, _frameIndex);
            
            foreach (PositionOnMatrix p in enumerable)
            {
                matrix[p.Row, p.Column] = this[GetIndexFromPositionOnMatrix(p)];
            }
        }

        public int GetIndexFromPositionOnMatrix(PositionOnMatrix pos) {
            if (IsOnFrameTop(pos)) {
                return pos.Column - _frameIndex;
            }
            if (IsOnFrameLeft(pos)) {
                return _frameColumnCount - 1 + (pos.Row - _frameIndex);
            }
            if (IsOnFrameBottom(pos)) { 
                return (_frameColumnCount - 1) + (_frameRowCount - 1) + (_frameColumnCount - 1 + _frameIndex - pos.Column);
            }
              if (IsOnFrameRight(pos)) {
                  return 2 * (_frameColumnCount - 1) + (_frameRowCount - 1) + (_frameRowCount - 1 + _frameIndex - pos.Row);
            }
              throw new ArgumentOutOfRangeException("Position","La posición no está en el frame");
        }
       
        private bool IsOnFrameTop(PositionOnMatrix pos)
        {
            return pos.Row == _frameIndex;
        }
        
        private bool IsOnFrameLeft(PositionOnMatrix pos)
        {
            return pos.Column == _columnCount - 1 - _frameIndex;
        }

        private bool IsOnFrameBottom(PositionOnMatrix pos)
        {
            return pos.Row == _rowCount - 1 - _frameIndex;
        }

        private bool IsOnFrameRight(PositionOnMatrix pos)
        {
            return pos.Column == _frameIndex;
        }
          
        public int ElementCount { get { return _values.Length; } }

        public T this[int index]
        {
            get
            {
                return index + _startIndex < ElementCount
                    ? _values[index + _startIndex]
                    : _values[index - (ElementCount - _startIndex)];
            }
        }
    }

    public class PositionOnMatrixFrameStyleEnumerable : IEnumerable<PositionOnMatrix>
    {
        private readonly IEnumerator<PositionOnMatrix> _enumerator;

        public PositionOnMatrixFrameStyleEnumerable(int rowCount, int columnCount, int? frameIndex = null)
        {
            _enumerator = new PositionOnMatrixFramewokStyleEnumerator(rowCount, columnCount, frameIndex);
        }

        public IEnumerator<PositionOnMatrix> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PositionOnMatrixFramewokStyleEnumerator : IEnumerator<PositionOnMatrix>
    {
        private PositionOnMatrix _position;
        private int? _frameIndex;
        private int _currentIndex;
        private readonly int _columnCount;
        private readonly int _rowCount;
        private readonly int _totalElements;

        public PositionOnMatrixFramewokStyleEnumerator(int rowCount, int columnCount, int? frameIndex = null)
        {
            if (rowCount <= 0)
            {
                throw new ArgumentOutOfRangeException("rowCount", "rowCount must be greater than 0");
            }

            if (columnCount <= 0)
            {
                throw new ArgumentOutOfRangeException("columnCount", "columnCount must be greater than 0");
            }

            if (frameIndex + 1 > Math.Min(rowCount, columnCount) / 2)
            {
                throw new ArgumentOutOfRangeException("frameIndex", "frameIndex must be lower than Math.Min(rowCount,columncount) / 2");
            }

            _currentIndex = -1;
            _position = null;
            _columnCount = columnCount;
            _rowCount = rowCount;
            _frameIndex = frameIndex;
            _totalElements = CalculateTotalElements();
        }

        private int CalculateTotalElements()
        {
            if (_frameIndex.HasValue)
            {
                return (_columnCount - 2 * _frameIndex.Value) * 2 + (_rowCount - 2 * _frameIndex.Value - 2) * 2;
            }

            return _columnCount * _rowCount;
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

        void IDisposable.Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_currentIndex + 1 >= _totalElements)
            {
                return false;
            }

            if (_position == null)
            {
                if (!_frameIndex.HasValue)
                {
                    _position = new PositionOnMatrix(0, 0, _rowCount, _columnCount);
                    _currentIndex = 0;

                    return true;
                }

                _position = new PositionOnMatrix(_frameIndex.Value, _frameIndex.Value, _rowCount, _columnCount);
                _currentIndex = 0;

                return true;
            }

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