namespace Checkers;

public class RussianDraughts
{
    private Figure[,] _board;

    public RussianDraughts()
    {
        _board = new Figure[8, 8];

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (row is 3 or 4 || (row + col) % 2 == 0)
                {
                    _board[row, col] = Figure.None;
                    continue;
                }
                _board[row, col] = (row < 3) ? Figure.BlackMan : Figure.WhiteMan;
            }
        }
    }

    public override string ToString()
    {
        var result = string.Empty;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                result += ((int)_board[i, j]).ToString() + ' ';
            }
            result += '\n';
        }

        return result;
    }
}
