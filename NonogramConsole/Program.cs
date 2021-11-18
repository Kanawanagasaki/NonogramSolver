using NonogramSolver;

var rawInput = "3,1,6;3,1,5;4,7;4,7;2,1,7;5,3,2;3,1,4,1;3,1,4;3,1,1,1,2;4,1,1,1,1;4,1,1,1,2;6,1,1,1,2;2,3,2,1,2,2;3,4,2,2,2,2;3,2,1,3,1,2,1;3,6,2,3,2;3,1,4,2,4;3,6,3,4;3,6,5,2;3,7,13;4,2,4,4,7;4,2,3,8,2;4,2,3,17;4,1,2,4,3,1;4,1,3,4,2,1;5,1,1,2,4,1,2;5,1,2,3,4,1,1,1;4,3,2,1,3,4,1,1,1;7,1,2,4,4,1,1;5,2,1,7,4,2,1;2,4,1,4,1,5,2,1;2,2,1,4,1,1;1,5,1,2;1,1,5,1,3;1,2,2,2,1,2;1,1,1,3,1,1,2;2,1,3,4,3,2;2,2,2,2,4,8;5,1,7,10;3,10;2,2,4,1;2,1,1,6;2,3,4,1;2,2,3,1;1,1,1,4|7;3,2;2,2;3,1,1;5,3,1;5,3;2,1,6;5,5;5,3,2,3;8,2;9,2;12,2;6,1,2;4,4,1;3,3,1;3,2,1;2,2,2,1;2,3,2,1;2,1,5,3,2;1,2,6,3,1,1;4,5,5,2;6,4,5,1,2;5,12,4;4,11,2,2;4,11,6;12,6;5,8;4,9;3,9,10;6,4,9;3,3,7;1,1,5;6;2,2,3;2,2,6;2,2,4,1,1;2,1,10,1;1,4,2,1,2,4;4,1,4,2,1,5,6;1,6,5,2,1,8;11,3,2,2,8;8,3,5,7,12;5,3,3,11,1,8,2,1;6,2,3,3,2,4,4,1;7,3,2";

int[][] columns = rawInput.Split('|')[0].Split(';').Select(x => x.Split(',').Select(y => int.Parse(y)).ToArray()).ToArray();
int[][] rows = rawInput.Split('|')[1].Split(';').Select(x => x.Split(',').Select(y => int.Parse(y)).ToArray()).ToArray();
int columnsHeight = columns.Max(c => c.Length);
int rowsWidth = rows.Max(r => string.Join(' ', r).Length);

Board board = new(columns, rows);

if(board.Width + rowsWidth + 1 > Console.WindowWidth || board.Height + columnsHeight + 1 > Console.WindowHeight)
{
    Console.WriteLine("Waiting for console to resize...");
    while(board.Width + rowsWidth + 1 > Console.WindowWidth || board.Height + columnsHeight + 1 > Console.WindowHeight)
    {
        Console.SetCursorPosition(0, 1);
        Console.Write(Console.WindowWidth + " " + Console.WindowHeight + "                    ");
        Thread.Sleep(1000);
    }
}

Console.Clear();

for(int iy = columnsHeight - 1; iy >= 0; iy--)
{
    for(int ix = 0; ix < columns.Length; ix++)
    {
        Console.SetCursorPosition(rowsWidth + ix * 2 + 1, columnsHeight - iy - 1);
        if (columns[ix].Length - iy - 1 >= 0)
            Console.Write(columns[ix][columns[ix].Length - iy - 1]);
    }
}

for(int iy = 0; iy < rows.Length; iy++)
{
    string row = string.Join(' ', rows[iy]);
    Console.SetCursorPosition(rowsWidth - row.Length, columnsHeight + iy + 1);
    Console.Write(row);
}

for (int iy = 0; iy < board.Height; iy++)
{
    Console.SetCursorPosition(rowsWidth + 1, columnsHeight + iy + 1);
    for (int ix = 0; ix < board.Width; ix++)
        Console.Write("??");
}

board.OnBoardChanged += (ix, iy, state) =>
{
    Console.SetCursorPosition(rowsWidth + ix * 2 + 1, columnsHeight + iy + 1);
    Console.Write(state == Board.CellState.Filled ? "██" : state == Board.CellState.Cross ? "  " : "??");
    Thread.Sleep(10);
};

Console.ReadLine();

board.Solve();

Console.SetCursorPosition(0, board.Height + columnsHeight);
Console.ReadLine();

if(!board.IsSolved())
{
    Console.Clear();
    Console.WriteLine(board.ToString().Replace(' ', '0').Replace('#', '1').Replace('X', '2').Replace('\n', ';'));
    Console.ReadLine();
}