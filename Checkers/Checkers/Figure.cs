﻿namespace Checkers;

public enum Color
{
    None,
    White,
    Black,
}

public enum Role
{
    None,
    Man,
    King,
}

readonly struct Figure
{
    public readonly Color Color;
    public readonly Role Role;

    public Figure(Color color, Role role)
    {
        Color = color;
        Role = role;
    }

    public static bool operator ==(Figure a, Figure b) => a.Color == b.Color && a.Role == b.Role;

    public static bool operator !=(Figure a, Figure b) => !(a == b);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Figure) 
            return false;

        return this == (Figure)obj;
    }

    public override int GetHashCode() => HashCode.Combine(Color, Role);

    public override string ToString()
    {
        if (Role == Role.None) return "-";
        if (Color == Color.White) 
            return Role == Role.Man ? "☻" : "W";
        return Role == Role.Man ? "☺" : "B";
    }
}