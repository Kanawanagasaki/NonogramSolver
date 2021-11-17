﻿using System.Diagnostics;

namespace NonogramSolver
{
    internal record class PotentialLine
    {
        private Board _board;
        private Numbers _numbers;
        private bool _isColumn;
        private int _size;
        private int _index;
        private List<List<Line>> _potentials;

        internal PotentialLine(Board board, Numbers nums, int index, bool isColumn)
        {
            _board = board;
            _numbers = nums;
            _isColumn = isColumn;
            _size = _isColumn ? board.Height : board.Width;
            _index = index;

            _potentials = AddPosiblePositions();
        }

        private List<List<Line>> AddPosiblePositions()
        {
            List<List<Line>> ret = new();

            int leftSpace = 0;
            for (int i = 0; i < _numbers.Length; i++)
            {
                ret.Add(GetPossibleLines(i, leftSpace));
                leftSpace += _numbers[i] + 1;
            }

            return ret;
        }

        private List<Line> GetPossibleLines(int index, int leftSpace)
        {
            List<Line> ret = new();

            int rightSpace = _numbers.RightSpace(index);
            for (int i = leftSpace; i <= _size - rightSpace - _numbers[index]; i++)
            {
                var line = new Line(i, i + _numbers[index] - 1);
                ret.Add(line);
            }

            return ret;
        }

        internal bool Process()
        {
            bool ret = false;

            var getCell = (int i) => _isColumn ? _board[_index, i] : _board[i, _index];
            var setCell = (int i, Board.CellState state) =>
            {
                if (_isColumn)
                {
                    if (_board[_index, i] != state)
                    {
                        _board[_index, i] = state;
                        ret = true;
                    }
                }
                else
                {
                    if(_board[i, _index] != state)
                    {
                        _board[i, _index] = state;
                        ret = true;
                    }
                }
            };

            int filledCells = 0;
            for (int i = 0; i < _size; i++)
            {
                var cell = getCell(i);
                if (cell == Board.CellState.Filled)
                {
                    List<Line>[] pretendents = new List<Line>[_potentials.Count];
                    int pretendentsCount = 0;

                    for (int j = 0; j < _potentials.Count; j++)
                    {
                        for(int k = 0; k < _potentials[j].Count; k++)
                        {
                            if(_potentials[j][k].Start - 1 == i || _potentials[j][k].End + 1 == i)
                            {
                                _potentials[j].RemoveAt(k);
                                k--;
                            }
                        }

                        var lines = _potentials[j].Where(l => l.Start <= i && i <= l.End).ToArray();
                        if (lines.Length > 0)
                        {
                            if (pretendents[j] is null) pretendents[j] = new();
                            pretendents[j].AddRange(lines);
                            pretendentsCount++;
                        }
                    }

                    if(pretendentsCount > 0 && pretendents.All(lines => lines is null || lines.Count == 1))
                    {
                        Line line = null;
                        bool isAllSame = true;
                        foreach(var pretendent in pretendents)
                        {
                            if (pretendent is null) continue;
                            if (line is null) line = pretendent.First();
                            if (line != pretendent.First())
                            {
                                isAllSame = false;
                                break;
                            }
                        }

                        if(isAllSame && line is not null)
                        {
                            if (line.Start - 1 >= 0)
                                setCell(line.Start - 1, Board.CellState.Cross);
                            if (line.End + 1 < _size)
                                setCell(line.End + 1, Board.CellState.Cross);
                        }
                    }

                    if (pretendentsCount == 1)
                    {
                        for(int j = 0; j < pretendents.Length; j++)
                        {
                            if(pretendents[j] is not null && _potentials[j].Count != pretendents[j].Count)
                            {
                                _potentials[j] = pretendents[j];
                                ret = true;
                                break;
                            }
                        }
                    }

                    filledCells++;
                }
                else if(cell == Board.CellState.Cross)
                {
                    for(int j = 0; j < _potentials.Count; j++)
                    {
                        for(int k = 0; k < _potentials[j].Count; k++)
                        {
                            if(_potentials[j][k].Start <= i && i <= _potentials[j][k].End)
                            {
                                _potentials[j].RemoveAt(k);
                                k--;
                                ret = true;
                            }
                        }
                    }
                }
            }

            if(filledCells == _numbers.Sum)
            {
                for(int i = 0; i < _size; i++)
                {
                    if (getCell(i) == Board.CellState.Empty)
                        setCell(i, Board.CellState.Cross);
                }
            }

            for(int i = 0; i < _size; i++)
            {
                if (getCell(i) != Board.CellState.Empty) continue;

                var pretendents = _potentials.Where(lines => lines.Any(l => l.Start <= i && i <= l.End));
                if (pretendents.Count() == 0)
                    setCell(i, Board.CellState.Cross);
                else if (pretendents.Count() == 1 && pretendents.First().All(l => l.Start <= i && i <= l.End))
                    setCell(i, Board.CellState.Filled);
            }

            for (int i = 0; i < _potentials.Count; i++)
            {
                if (_potentials[i].Count == 1)
                {
                    var line = _potentials[i][0];
                    for (int j = line.Start; j <= line.End; j++)
                        setCell(j, Board.CellState.Filled);
                    if (line.Start - 1 >= 0)
                        setCell(line.Start - 1, Board.CellState.Cross);
                    if(line.End + 1 < _size)
                        setCell(line.End + 1, Board.CellState.Cross);

                    for (int j = 0; j < i; j++)
                    {
                        for (int k = 0; k < _potentials[j].Count; k++)
                        {
                            if (_potentials[j][k].End >= line.Start - 1)
                            {
                                _potentials[j].RemoveAt(k);
                                k--;
                                ret = true;
                            }
                        }
                    }
                    for (int j = i + 1; j < _potentials.Count; j++)
                    {
                        for (int k = 0; k < _potentials[j].Count; k++)
                        {
                            if (_potentials[j][k].Start <= line.End + 1)
                            {
                                _potentials[j].RemoveAt(k);
                                k--;
                                ret = true;
                            }
                        }
                    }
                }
                if(i < _potentials.Count - 1 && _potentials[i].Count > 0 && _potentials[i + 1].Count > 0)
                {
                    for(int j = 0; j < _potentials[i + 1].Count; j++)
                    {
                        if(_potentials[i + 1][j].Start <= _potentials[i].First().End + 1)
                        {
                            _potentials[i + 1].RemoveAt(j);
                            j--;
                            ret = true;
                        }
                    }
                    for(int j = 0; j < _potentials[i].Count; j++)
                    {
                        if(_potentials[i + 1].Last().Start - 1 <= _potentials[i][j].End)
                        {
                            _potentials[i].RemoveAt(j);
                            j--;
                            ret = true;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
