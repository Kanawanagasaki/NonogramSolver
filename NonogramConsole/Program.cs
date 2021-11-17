using NonogramSolver;

var rawInput = "1,2,18,1,20,8;2,3,17,2,14,3,8;3,4,16,3,14,1,2,8;3,4,10,4,14,1,2,8;3,4,5,7,5,8,1,1,2,4;3,2,7,5,7,3,9,2,3;2,1,8,3,8,2,7,2,2,2;11,3,10,4,2,2,5;1,2,3,3,10,6,2,2,4;1,6,3,10,5,3,3;1,2,5,1,10,4,3,2;3,2,5,11,4,2,3;1,3,4,11,4,2,6;4,3,11,4,2,7;2,4,10,1,3,3,2,6;3,5,10,4,12,3,3;4,2,9,4,5,4,1,4,2;5,8,4,4,4,1,5,3;5,3,3,9,5,4,4;3,5,6,4,2,4,5;2,10,6,3,4,2,4,2;1,21,5,3,2,4,1;20,7,1,2,7,1;5,10,10,2,5,2;4,9,4,3;4,12,6;3,2,4,1,4;3,5,6,4;3,3,7,6;4,5,4,2,3;4,2,4,4,1,1;7,3,5,3,2,1,1;8,2,2,6,3,3,2,1;3,9,10,6,3,4,3,1;20,10,6,3,4,3,1;3,2,2,6,3,3,3,1;3,5,3,2,2;2,4,2,3,1,1,3;5,2,12;1,9,1,1;3,6,3,3,2;4,8,5,2,2,3;6,3,1,8,3,3,2;9,2,4,4,3;10,4,3,2,1;10,2,3,2,2,2;10,2,4,1,2;9,3,4,3,5;15,6,2,7,4;12,7,2,8;13,4,2,7,3;13,1,5,4,5;23,3,3,7;19,3,3,5;14,1,3,3,2;9,3,2,7,3;15,4,2,7,3;13,6,2,11;11,11,2,2;10,11,2,5,2;10,5,2,1,6,2;10,7,5,3;10,8,5,3;9,7,5,3;15,5,3;14,5,3;13,5,3;12,6,3;11,6,3;4,2,6,3;4,6,3;4,6,3;11,3;10,3;9,4;7,4;5,5;4,6|3,5,8,5;5,1,7,8;7,2,5,10;3,4,3,5,4;6,3,2,4,4;3,1,1,5,4,4;3,2,2,4,3,4;4,2,3,4,3,2,2;4,3,4,3,3,5;3,5,4,1,4,4;5,4,5,3;3,5,3,7,2;3,4,9,2;3,3,2,11,2;3,1,2,6,4,2,1,1;4,3,8,3,2,2,3;9,8,3,2,2,3;8,9,3,1,6;7,9,3,1,7;6,9,4,1,9;6,10,4,1,10;5,9,5,1,1,10;5,9,5,1,14;4,8,6,1,15;4,8,6,1,5,11;3,7,6,2,1,11;3,7,1,2,1,4,3,12;2,6,2,1,2,4,12;1,6,3,3,2,13;6,5,4,2,14;5,5,4,2,15;4,4,3,2,1,13;2,3,3,2,1,5,8;7,2,1,2,1,4,8;6,1,1,2,2,3,9;5,2,2,1,4,6,10;5,2,2,2,2,2,2,2,12;5,2,2,3,1,1,2,6,8;5,2,1,3,2,2,2,2,2,2,7;5,1,1,3,2,2,2,2,4,7;5,1,2,4,1,1,1,2,6,6;4,1,1,3,10,1,2,7,5;4,2,1,1,4,10,1,2,14;4,1,2,2,4,1,10,1,2,2,2,14;4,1,1,2,3,2,8,2,2,3,1,3,9;4,1,1,3,3,2,6,3,3,2,2,9;4,1,2,2,3,2,4,6,3,2,8;1,1,1,7,9,2,9,7,4;1,1,1,2,3,20,9,13;1,3,6,1,11,1,10,3,8;2,2,3,4,12,3,1,6,7;4,3,7,4,3,14,6;5,2,17,3,4,5,12,16;19,6,3,4,11,18;12,10,2,6,2,3,5,3,19;11,1,8,1,1,6,1,5,2,3,17;4,1,3,1,2,1,2,6,1,2,3,16,1;4,2,5,2,1,1,1,6,6,1,3,3,3,6,2;5,3,5,3,1,4,5,2,4,2,3,1,2,4;6,3,5,3,6,5,2,2,2,4,3,17;6,4,3,4,7,2,3,1,2,3,24;6,4,3,4,15,2,2,2,2,3,24";

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