using System.Text.RegularExpressions;

namespace Checkers;

public class RussianDraughts
{
    private Figure[,] _board;
    private bool _whitesMove;
    private bool _gameOn;

    public RussianDraughts()
    {
        _board = new Figure[8, 8];
        _whitesMove = true;
        _gameOn = true;

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

    public bool IsGameOn() => _gameOn;

    public void NextMove()
    {
        try 
        {

            ReadMove(out int[] move);

            var (x, y, z, w) = (--move[0], --move[1], --move[2], --move[3]);

            Move(x, y, z, w);

            _whitesMove = !_whitesMove;

        } 
        catch (Exception e)
        {
            ThrowException();
        }
        
    }

    private void Move(int x, int y, int z, int w)
    {
        if ( IsEmpty(x, y) || IsWhite(x, y) != _whitesMove)
            throw new Exception();

        // not right logic
        _board[x, y] = Figure.None;
        _board[z, w] = _whitesMove ? Figure.WhiteMan : Figure.BlackMan;
    }

    private bool IsEmpty(int x, int y) => _board[x, y] == Figure.None;

    private bool IsWhite(int x, int y) => _board[x, y] is Figure.WhiteMan or Figure.WhiteKing;

    private bool ReadMove(out int[] move)
    {
        var s = Console.ReadLine();

        if ( s is null || !new Regex("[1-8] [1-8] [1-8] [1-8]").IsMatch(s) )
            throw new Exception();

        move = s.Split().Select( s => Convert.ToInt32(s) ).ToArray();

        return true;
    }

    private void ThrowException()
    {
        Console.WriteLine("Введены неверные данные");
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
