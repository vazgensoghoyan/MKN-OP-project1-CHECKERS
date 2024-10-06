using static System.Math;
using System.Text.RegularExpressions;

namespace Checkers;

public class RussianDraughts
{
    private Figure[,] _board;
    private bool _whitesMove;
    private bool _gameOn;

    private static readonly Figure None = new(Color.None, Role.None);
    private static readonly Figure WhiteMan = new(Color.White, Role.Man);
    private static readonly Figure BlackMan = new(Color.Black, Role.Man);
    private static readonly Figure WhiteKing = new(Color.White, Role.King);
    private static readonly Figure BlackKing = new(Color.Black, Role.King);

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
                    _board[row, col] = None;
                    continue;
                }
                _board[row, col] = (row < 3) ? BlackMan : WhiteMan;
            }
        }
    }

    public bool IsGameOn() => _gameOn;

    public void NextMove()
    {
        try 
        {

            ReadMove(out int[] move);

            var (x, y, z, w) = (move[0], move[1], move[2], move[3]);

            Move(x, y, z, w);

            _whitesMove = !_whitesMove;

        } 
        catch (Exception e)
        {
            Console.WriteLine( e.Message );
        }
        
    }

    private void Move(int x, int y, int z, int w)
    {
        if ( IsEmpty(x, y) )
            throw new Exception("Клетка не пуста!");

        if ( IsWhite(x, y) != _whitesMove )
            throw new Exception("Выбрана шашка не того цвета!");

        if ( !IsRightSingleMove(x, y, z, w) )
            throw new Exception("Такой ход невозможен! Читайте правила");
        
        _board[z, w] = _board[x, y];
        _board[x, y] = None;
    }

    private bool IsRightSingleMove(int x, int y, int z, int w)
        => Abs(y - w) == 1 && Abs(x - z) == 1 && ( _whitesMove == (x - z == 1) );

    private bool IsEmpty(int x, int y) => _board[x, y] == None;

    private bool IsWhite(int x, int y) => _board[x, y].Color == Color.White;

    private bool ReadMove(out int[] m)
    {
        var s = Console.ReadLine();

        if ( s is null || !new Regex("[a-h][1-8] [a-h][1-8]").IsMatch(s) )
            throw new IncorrectDataException();

        var split = s.Split().Where(a => a.Length > 0).ToArray();

        m = [ split[0][0] - 'a', split[0][1] - '1', 
              split[1][0] - 'a', split[1][1] - '1' ];
        
        // human move -> indexes in array
        ( m[0], m[1] ) = ( 7 - m[1], m[0] );
        ( m[2], m[3] ) = ( 7 - m[3], m[2] );

        return true;
    }

    public static string GetTheRules()
    {
        return "----------------------------------------------------------------------------------------------------------\n" +
            "Правила хода\r\n\r\n    " +
            "Простая шашка ходит по диагонали вперёд на одну клетку.\r\n    " +
            "Дамка ходит по диагонали на любое свободное поле как вперёд, так и назад, но не может перескакивать свои шашки или дамки.\r\n\r\n" +
            "Правила взятия\r\n\r\n    " +
            "Взятие обязательно. Побитые шашки и дамки снимаются только после завершения хода.\r\n    " +
            "Простая шашка, находящаяся рядом с шашкой соперника, за которой имеется свободное поле, переносится через эту шашку на это свободное поле. Если есть возможность продолжить взятие других шашек соперника, то это взятие продолжается, пока бьющая шашка не достигнет положения, из которого бой невозможен. Взятие простой шашкой производится как вперёд, так и назад.\r\n    " +
            "Дамка бьёт по диагонали, как вперёд, так и назад, и становится на любое свободное поле после побитой шашки. Аналогично, дамка может бить несколько фигур соперника и должна бить до тех пор, пока это возможно.\r\n    " +
            "При бое через дамочное поле простая шашка превращается в дамку и продолжает бой по правилам дамки.\r\n    " +
            "При взятии применяется правило турецкого удара — за один ход шашку противника можно побить только один раз. То есть, если при бое нескольких шашек противника шашка или дамка повторно выходит на уже побитую шашку, то ход останавливается.\r\n    " +
            "При нескольких вариантах взятия, например, одну шашку или две, игрок выбирает вариант взятия по своему усмотрению.\n" +
            "----------------------------------------------------------------------------------------------------------\n";
    }

    public override string ToString()
    {
        var result = string.Empty;

        for (int i = 0; i < 8; i++)
        {
            result += (8 - i).ToString() + ' ';

            for (int j = 0; j < 8; j++)
            {
                result += _board[i, j].ToString() + ' ';
            }

            result += '\n';
        }

        result += "  a b c d e f g h\n";

        return result;
    }
}
