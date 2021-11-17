using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolver
{
    public record class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Numbers[] Columns { get; set; }
        public Numbers[] Rows { get; set; }

        public event Action<int, int, CellState>? OnBoardChanged;
        public event Action<int>? OnPotentialLineProgress;

        private CellState[,] _board;
        public CellState this[int column, int row]
        {
            get => _board[column, row];
            set
            {
                if(_board[column, row] != value)
                {
                    _board[column, row] = value;
                    OnBoardChanged?.Invoke(column, row, value);
                }
            }
        }

        public Board(int[][] columns, int[][] rows)
        {
            Columns = columns.Select(c => new Numbers(c)).ToArray();
            Rows = rows.Select(r => new Numbers(r)).ToArray();

            Width = Columns.Length;
            Height = Rows.Length;

            _board = new CellState[Width, Height];
        }

        public void Solve()
        {
            PotentialLine[] potentialColumns = Columns.Select((c, i) =>
                {
                    OnPotentialLineProgress?.Invoke((int)(((double)i + 1) / Columns.Length * 50.0));
                    return new PotentialLine(this, c, i, true);
                }).ToArray();
            PotentialLine[] potentialRows = Rows.Select((r, i) =>
                {
                    OnPotentialLineProgress?.Invoke(50 + (int)(((double)i + 1) / Rows.Length * 50.0));
                    return new PotentialLine(this, r, i, false);
                }).ToArray();

            OnPotentialLineProgress?.Invoke(100);

            bool hasChanges = false;
            do
            {
                hasChanges = false;
                foreach (var c in potentialColumns)
                    if (c.Process()) hasChanges = true;
                foreach (var r in potentialRows)
                    if (r.Process()) hasChanges = true;
            }
            while(hasChanges);

        }

        public enum CellState : byte
        {
            Empty, Filled, Cross
        }
    }
}
