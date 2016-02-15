using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertSorting
{
    class Solution
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
            var sortingInput = SortingInput.FromReader(reader);
            insertionSort(sortingInput.UnSortedArray,writer);
            
        }
        static void insertionSort(int[] ar, TextWriter output)
        {


            SortingArray<int> sortedArray = new SortingArray<int>(ar);
            for (int i = 1; i < ar.Length; i++)
            {
                sortedArray.insertionOneSortedElement( i);

                sortedArray.SortedArray.Write(output);
            }

        }
    }

    public class SortingInput {
        public int[] UnSortedArray { get; private set; }
        private SortingInput(int[] values) {
            UnSortedArray = values;
        }
        public static SortingInput FromReader(TextReader input) {
            var allInput = input.ReadToEnd();
            var lines = allInput.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int _ar_size;
            _ar_size = Convert.ToInt32(lines[0]);
            int[] _ar = new int[_ar_size];
            String elements =lines[1];
            String[] split_elements = elements.Split(' ');
            for (int _ar_i = 0; _ar_i < _ar_size; _ar_i++)
            {
                _ar[_ar_i] = Convert.ToInt32(split_elements[_ar_i]);
            }
            return new SortingInput(_ar);
        }
    }

    public static class ArrayExtensions
    {
        public static void Write<T>(this T[] array, TextWriter output)
        {
            output.WriteLine(String.Join(" ", array));
        }
    }
    public class SortingArray<T> where T : IComparable<T>
    {
        private T[] _values;

        public int numberOfShifts { get; private set; }

        public SortingArray(T[] values)
        {
           _values =values;
           numberOfShifts = 0;
           }
        public T[] SortedArray { get { return _values; } }

        public void Sort()
        {
            for (int i = 1; i < _values.Length; i++)
            {
                insertionOneSortedElement(i);
            }
        }

        public void insertionOneSortedElement( int numberOfSortedElements)
        {
            T elementToInsert = _values[numberOfSortedElements];
            for (int i = 0; i < numberOfSortedElements; i++)
            {
                if (_values[i].CompareTo(elementToInsert) > 0)
                {
                    for (int j = numberOfSortedElements-1; j >= i; j--)
                    {
                        _values[j + 1] = _values[j];
                        numberOfShifts += 1;
                    }
                  
                    _values[i] = elementToInsert;
                    break;
                }
                if (i == numberOfSortedElements - 1)
                {
                    _values[i + 1] = elementToInsert;
                }
            }
        }

    }
}

