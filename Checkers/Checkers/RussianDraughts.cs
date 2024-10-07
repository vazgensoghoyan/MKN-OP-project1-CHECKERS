using static System.Math;
using System.Text.RegularExpressions;
using System.Runtime.ExceptionServices;

namespace Checkers;

public class RussianDraughts
{
    private Figure[,] _board;

    private bool _gameOn;
    private bool _whitesMove;
    private bool _shouldWhiteEat;
    private bool _shouldBlackEat;

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
        _shouldWhiteEat = false;
        _shouldBlackEat = false;

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
        //foreach (var fig in FiguresOnDiagonal(1, 0, 0, 1))
        //    Console.Write("{0} ", fig);
        //Console.WriteLine();
        //foreach (var fig in FiguresOnDiagonal(0, 1, 6, 7))
        //    Console.Write("{0} ", fig);
        //Console.WriteLine();
        //foreach (var fig in FiguresOnDiagonal(3, 0, 0, 3))
        //    Console.Write("{0} ", fig);
        //Console.WriteLine();
        //foreach (var fig in FiguresOnDiagonal(0, 3, 4, 7))
        //    Console.Write("{0} ", fig);
        //Console.WriteLine();
        //foreach (var fig in FiguresOnDiagonal(5, 0, 0, 5))
        //    Console.Write("{0} ", fig);
        //Console.WriteLine();
        //foreach (var fig in FiguresOnDiagonal(0, 5, 2, 7))
        //    Console.Write("{0} ", fig);

        try 
        {
            ReadMove(out int[] move);

            var (x, y, z, w) = (move[0], move[1], move[2], move[3]);

            IsRightMove(x, y, z, w);

            MoveThePiece(x, y, z, w);

            Console.WriteLine("{0}    {1}\n", _shouldWhiteEat, _shouldBlackEat);
        } 
        catch (Exception e)
        {
            Console.WriteLine( e.Message );
        }
        
    }

    private bool IsRightMove(int x, int y, int z, int w)
    {
        if ( IsEmpty(x, y) || !IsEmpty(z, w) || Abs(x - z) != Abs(y - w) )
            throw new Exception("Неправильный ход!");

        if ( IsWhite(x, y) != _whitesMove )
            throw new Exception("Выбрана шашка не того цвета!");

        if ( EatsHimself(x, y, z, w) )
            throw new Exception("Вы съели себя!");

        var shouldEat = IsWhite(x, y) ? _shouldWhiteEat : _shouldBlackEat;

        if ( shouldEat != EatsOpponent(x, y, z, w) )
            throw new Exception("Надо съесть шашку противника!");

        if ( IsMan(x, y) )
        {
            if ( !shouldEat && Abs(x - z) == 1 && (x > z) == _whitesMove )
                return true;

            if ( shouldEat && Abs(x - z) == 2 )
                return true;
        }

        throw new Exception("Неправильный ход!");
    }

    private bool ShouldEat(int x, int y)
    {

        return false;
    }

    private bool ShouldEat(Color color)
    {
        for (int i = 1; i < 6; i += 2)
        {
            if ( ShouldEatOnDiagonal( i, 0, 0, i, color ) || ShouldEatOnDiagonal( 0, i, 7-i, 7, color ) ||
                 ShouldEatOnDiagonal( 7-i, 7, 7, 7-i, color ) || ShouldEatOnDiagonal( 7, 7-i, i, 0, color) )
            {
                return true;
            }
        }

        return ShouldEatOnDiagonal( 7, 0, 0, 7, color );
    }

    private bool ShouldEatOnDiagonal(int x, int y, int z, int w, Color color)
    {
        var diag = FiguresOnDiagonal(x, y, z, w);

        for (int i = 1; i < diag.Length - 1; i++)
        {
            if ( diag[i].Color == color || diag[i].Color == Color.None ) continue;

            bool a = ( diag[i-1].Color == color && diag[i+1].Color == Color.None );
            bool b = ( diag[i+1].Color == color && diag[i-1].Color == Color.None ); 

            if ( a || b ) return true;
        }

        return false;
    }

    private bool EatsOpponent(int x, int y, int z, int w)
    {
        var diag = FiguresOnDiagonal(x, y, z, w);

        var ourColor = _board[x, y].Color;
        var oppColor = IsWhite(x, y) ? Color.Black : Color.White;

        var seen = false;

        for (int i = 1; i < diag.Length - 1; i++)
        {
            if ( diag[i].Color == ourColor )
                return false;

            if ( diag[i].Color == oppColor )
            {
                if ( diag[i - 1].Color == oppColor )
                    return false;
                seen = true;
            }
        }

        return seen;
    }

    private bool EatsHimself(int x, int y, int z, int w)
    {
        var diag = FiguresOnDiagonal(x, y, z, w);
        var ourColor = _board[x, y].Color;

        for (int i = 1; i < diag.Length - 1; i++)
            if ( diag[i].Color == ourColor )
                return true;

        return false;
    }

    private Figure[] FiguresOnDiagonal(int x, int y, int z, int w)
    {
        var result = new Figure[ Abs(x - z) + 1 ];

        int d1 = (x < z) ? 1 : -1,
            d2 = (y < w) ? 1 : -1;

        int index = 0;
        int i = x, j = y;

        while (d1 * i <= d1 * z)
        {
            result[index++] = _board[i, j];
            i += d1;
            j += d2;
        }

        return result;
    }

    private bool IsEmpty(int x, int y) => _board[x, y] == None;

    private bool IsMan(int x, int y) => _board[x, y].Role == Role.Man;

    private bool IsThatColor(int x, int y, Color color) => _board[x, y].Color == color;

    private bool IsWhite(int x, int y) => IsThatColor(x, y, Color.White);

    private bool IsBlack(int x, int y) => IsThatColor(x, y, Color.Black);

    private void MoveThePiece(int x, int y, int z, int w)
    {
        int d1 = (x < z) ? 1 : -1,
            d2 = (y < w) ? 1 : -1;

        int i = x + d1,
            j = y + d2;

        bool justEaten = false;

        while (d1 * i < d1 * z)
        {
            if (_board[i, j] != None)
            {
                justEaten = true;
                _board[i, j] = None;
            }
            i += d1;
            j += d2;
        }

        _board[z, w] = _board[x, y];
        _board[x, y] = None;

        var isWhite = IsWhite(x, y);

        if (z % 7 == 0 && ((z == 0) == isWhite) )
        {
            _board[z, w] = isWhite ? WhiteKing : BlackKing;
        }

        _shouldWhiteEat = ShouldEat( Color.White );
        _shouldBlackEat = ShouldEat( Color.Black );

        if ( !( justEaten && ShouldEat(z, w) ) )
            _whitesMove = !_whitesMove;
    }

    private void ReadMove(out int[] m)
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
