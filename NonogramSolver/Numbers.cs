using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolver
{
    public record class Numbers
    {
        private int[] _numbers;

        public int this[int index] => _numbers[index];
        public int Length => _numbers.Length;

        public int Sum { get; private set; }

        public Numbers(params int[] numbers)
        {
            _numbers = numbers;
            Sum = _numbers.Sum();
        }

        public int RightSpace(int index) => _numbers.Select((v, i) => i <= index ? 0 : v + 1).Sum();
    }
}
