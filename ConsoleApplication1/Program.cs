using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
//Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input05.txt"));
namespace MatrixRotation
{

    class Program
    {


        static void Main(String[] args)
        {
            Console.SetIn(new System.IO.StreamReader("D:\\Documentos\\Downloads\\input05.txt"));
            string[] tokens = Console.ReadLine().Split();
            int row_count = Convert.ToInt32(tokens[0]);
            int column_count = Convert.ToInt32(tokens[1]);
            int rotations = Convert.ToInt32(tokens[2]);
            string[,] matrix = new string[row_count, column_count];
            for (int r = 0; r < row_count; r++)
            {
                var line = Console.ReadLine().Split();
                for (int c = 0; c < column_count; c++)
                {
                    matrix[r, c] = line[c];
                }
            }
            int framework_count = Math.Min(row_count, column_count) / 2;
            Tuple<int, int>[][] rotateArray = new Tuple<int, int>[framework_count][];
            for (int k = 0; k < framework_count; k++)
            {
                rotateArray[k] = new Tuple<int, int>[(column_count - 2 * k) * 2 + (row_count - 2 * k - 2) * 2];
                int i = 0;
                int row = k;
                for (int col = k; col < column_count - k; col++)
                {
                    rotateArray[k][i] = new Tuple<int, int>(row, col);
                    i += 1;
                }
                int column = column_count - 1 - k;
                for (int r = k + 1; r < row_count - k; r++)
                {
                    rotateArray[k][i] = new Tuple<int, int>(r, column);
                    i += 1;
                }
                row = row_count - 1 - k;
                for (int col = column_count - k - 2; col > k - 1; col--)
                {
                    rotateArray[k][i] = new Tuple<int, int>(row, col);
                    i += 1;
                }
                column = k;
                for (int r = row_count - k - 2; r > k; r--)
                {
                    rotateArray[k][i] = new Tuple<int, int>(r, column);
                    i += 1;
                }
            }
            string[][] rotatedMatrix = new string[row_count][];
            for (int r = 0; r < row_count; r++)
            {
                rotatedMatrix[r] = new string[column_count];
            }
            for (int k = 0; k < framework_count; k++)
            {

                int i = rotations % rotateArray[k].Length;
                int row = k;
                for (int col = k; col < column_count - k; col++)
                {
                    rotatedMatrix[row][col] = matrix[rotateArray[k][i].Item1, rotateArray[k][i].Item2];
                    i += 1;
                    if (i >= rotateArray[k].Length) i = 0;
                }
                int column = column_count - 1 - k;
                for (int r = k + 1; r < row_count - k; r++)
                {
                    rotatedMatrix[r][column] = matrix[rotateArray[k][i].Item1, rotateArray[k][i].Item2];
                    i += 1;
                    if (i >= rotateArray[k].Length) i = 0;
                }
                row = row_count - 1 - k;
                for (int col = column_count - k - 2; col > k - 1; col--)
                {
                    rotatedMatrix[row][col] = matrix[rotateArray[k][i].Item1, rotateArray[k][i].Item2];
                    i += 1;
                    if (i >= rotateArray[k].Length) i = 0;
                }
                column = k;
                for (int r = row_count - k - 2; r > k; r--)
                {
                    rotatedMatrix[r][column] = matrix[rotateArray[k][i].Item1, rotateArray[k][i].Item2];
                    i += 1;
                    if (i >= rotateArray[k].Length) i = 0;
                }
            }
            for (int r = 0; r < row_count; r++)
            {
                Console.WriteLine(String.Join(" ", rotatedMatrix[r]));
            }
            Console.ReadLine();
        }
        static Tuple<int, int> RotateMatrixIndex(int row, int column, int row_count, int column_count, int rotations)
        {

            int framework_count = Math.Min(row_count, column_count) / 2;
            for (int rot = 1; rot <= rotations; rot++)
            {
                for (int k = 0; k < framework_count; k++)
                {
                    if (column == column_count - k - 1 && row >= k && row < row_count - k - 1)
                    {
                        row = row + 1;
                        break;
                    }
                    if (column == k && row >= k + 1 && row < row_count - k)
                    {
                        row = row - 1;
                        break;
                    }
                    if (row == k && column >= k && column < column_count - k - 1)
                    {
                        column = column + 1;
                        break;
                    }
                    if (row == row_count - k - 1 && column >= k + 1 && column < column_count - k)
                    {
                        column = column - 1;
                        break;
                    }
                }
            }
            return new Tuple<int, int>(row, column);

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

            //   rotatedMatrix.Write(writer);
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
        public MatrixFrameworkStyleEnumerator<T> GetFrameworkStyleEnumerator(int? framework = null)
        {
            return new MatrixFrameworkStyleEnumerator<T>(this, framework);
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
            int framework_Count = Math.Min(RowCount, ColumnCount) / 2;
            Framework<T>[] frameworks = new Framework<T>[framework_Count];
            for (int i = 0; i < framework_Count; i++)
            {
                frameworks[i] = Framework<T>.FromMatrix(this, i, numberOfRotations);
            }

            return SetMatrixFromFrameworks(frameworks);
        }

        public Matrix<T> SetMatrixFromFrameworks(Framework<T>[] frameworks)
        {
            var result = new Matrix<T>(RowCount, ColumnCount);
            PositionOnMatrixFramewokStyleEnumerator positionEnumerator = new PositionOnMatrixFramewokStyleEnumerator(RowCount, ColumnCount);
            for (int i = 0; i < frameworks.Length; i++)
            {
                for (int j = 0; j < frameworks[i].ElementCount; j++)
                {
                    positionEnumerator.MoveNext();
                    this[positionEnumerator.Current.Row, positionEnumerator.Current.Column] = frameworks[i][j];
                }

            }
            return result;
        }


        public Matrix<T> Transform(Func<int, int, T> func)
        {
            var result = new Matrix<T>(RowCount, ColumnCount);

            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColumnCount; column++)
                {
                    result[row, column] = func(row, column);
                }
            }

            return result;
        }
    }

    public class Framework<T>
    {
        private T[] _values;
        private int _startIndex;
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
                throw new ArgumentOutOfRangeException("frameworkIndex must be lower than Math.Min(matrix.RowCount, matrix.ColumnCount) / 2");
            }

            var enumerator = new PositionOnMatrixFramewokStyleEnumerator(matrix.RowCount, matrix.ColumnCount, frameworkIndex);
            var values = new T[enumerator.TotalElements];

            for (int i = 0; i < values.Length; i++)
            {
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
        private int _columnCount;
        private int _rowCount;

        public PositionOnMatrixFramewokStyleEnumerator(int rowCount, int columnCount, int? frameworkIndex = null)
        {
            if (rowCount <= 0)
            {
                throw new ArgumentOutOfRangeException("rowCount must be greater than 0");
            }
            if (columnCount <= 0)
            {
                throw new ArgumentOutOfRangeException("columCount must be greater than 0");

            }
            if (frameworkIndex + 1 > Math.Min(rowCount, columnCount) / 2)
            {
                throw new ArgumentOutOfRangeException("frameworwIndex must be lower than Math.Min(rowCount,columncount) / 2");
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
                else
                {
                    return _columnCount * _rowCount;
                }
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
            return _position.Column <= _columnCount / 2 &&
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
            return _position.Row <= _rowCount / 2 &&
                _position.Column >= _position.Row &&
                _position.Column <= _columnCount - 1 - _position.Row - 1;
        }


    }
    public class MatrixFrameworkStyleEnumerator<T> : IEnumerator<T>, IEnumerator
    {

        private Matrix<T> _matrix;
        private PositionOnMatrix _position;
        private T _currentElement;
        private int? _frameworkIndex;
        private int _currentIndex;

        public MatrixFrameworkStyleEnumerator(Matrix<T> matrix, int? frameworkIndex = null)
        {
            if (frameworkIndex + 1 > Math.Min(matrix.RowCount, matrix.ColumnCount) / 2)
            {
                throw new ArgumentOutOfRangeException("frameworwIndex must be lower than Math.Min(matrix.RowCount, matrix.ColumnCount) / 2");
            }
            _matrix = matrix;
            _position = null;
            _currentElement = default(T);
            _frameworkIndex = frameworkIndex;
            _currentIndex = -1;
        }
        public int TotalElements
        {
            get
            {
                if (_frameworkIndex.HasValue)
                {
                    return (_matrix.ColumnCount - 2 * _frameworkIndex.Value) * 2 + (_matrix.RowCount - 2 * _frameworkIndex.Value - 2) * 2;
                }
                else
                {
                    return _matrix.ColumnCount * _matrix.RowCount;
                }
            }
        }
        public void Reset() { _currentIndex = -1; _position = null; _currentElement = default(T); }

        void IDisposable.Dispose() { }

        T IEnumerator<T>.Current
        {
            get { return _currentElement; }
        }

        object IEnumerator.Current
        {
            get { return _currentElement; }
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
                    _position = new PositionOnMatrix(0, 0, _matrix.RowCount, _matrix.ColumnCount);
                    _currentIndex = 0;
                    _currentElement = _matrix[_position.Row, _position.Column];
                    return true;
                }
                else
                {
                    _position = new PositionOnMatrix(_frameworkIndex.Value, _frameworkIndex.Value, _matrix.RowCount, _matrix.ColumnCount);
                    _currentIndex = 0;
                    _currentElement = _matrix[_position.Row, _position.Column];
                    return true;
                }
            }
            else
            {
                if (MustGoLeft())
                {
                    _position = new PositionOnMatrix(_position.Row, _position.Column + 1, _matrix.RowCount, _matrix.ColumnCount);
                    _currentIndex += 1;
                    _currentElement = _matrix[_position.Row, _position.Column];
                    return true;
                }
                if (MustGoDown())
                {
                    _position = new PositionOnMatrix(_position.Row + 1, _position.Column, _matrix.RowCount, _matrix.ColumnCount);
                    _currentIndex += 1;
                    _currentElement = _matrix[_position.Row, _position.Column];
                    return true;
                }
                if (MustGoRight())
                {
                    _position = new PositionOnMatrix(_position.Row, _position.Column - 1, _matrix.RowCount, _matrix.ColumnCount);
                    _currentIndex += 1;
                    _currentElement = _matrix[_position.Row, _position.Column];
                    return true;
                }
                if (MustGoUp())
                {
                    _position = new PositionOnMatrix(_position.Row - 1, _position.Column, _matrix.RowCount, _matrix.ColumnCount);
                    _currentIndex += 1;
                    _currentElement = _matrix[_position.Row, _position.Column];
                    return true;
                }
                if (MustGoInner())
                {
                    _position = new PositionOnMatrix(_position.Row, _position.Column + 1, _matrix.RowCount, _matrix.ColumnCount);
                    _currentIndex += 1;
                    _currentElement = _matrix[_position.Row, _position.Column];
                    return true;
                }
                throw new InvalidOperationException("Should not get here");
            }

        }

        private bool MustGoInner()
        {
            return _position.Column <= _matrix.ColumnCount / 2 &&
                _position.Row == _position.Column + 1;
        }

        private bool MustGoUp()
        {
            return _position.Column <= _matrix.ColumnCount / 2 &&
                _position.Row >= _position.Column + 2 &&
                _position.Row <= _matrix.RowCount - 1 - _position.Column;
        }

        private bool MustGoRight()
        {
            return _position.Row >= _matrix.RowCount / 2 &&
                _position.Column >= _matrix.RowCount - 1 - _position.Row + 1 &&
                _position.Column <= _matrix.ColumnCount - 1 - (_matrix.RowCount - 1) + _position.Row;
        }

        private bool MustGoDown()
        {
            return _position.Column >= _matrix.ColumnCount / 2 &&
               _position.Row >= _matrix.ColumnCount - 1 - _position.Column &&
               _position.Row <= _matrix.RowCount - 1 - (_matrix.ColumnCount - 1) + _position.Column - 1;
        }

        private bool MustGoLeft()
        {
            return _position.Row <= _matrix.RowCount / 2 &&
                _position.Column >= _position.Row &&
                _position.Column <= _matrix.ColumnCount - 1 - _position.Row - 1;
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
            RowCount = rowCount;
            ColumnCount = columnCount;

            Row = row;
            Column = column;

        }
        private int RowCount { get; set; }

        private int ColumnCount { get; set; }

        public int Row { get; private set; }

        public int Column { get; private set; }
    }
}