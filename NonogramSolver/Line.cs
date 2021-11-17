using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolver
{
    internal record class Line
    {
        internal int Start { get; init; }
        internal int End { get; init; }

        public Line(int start, int end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
            => $"Line: {Start} - {End}";
    }
}
