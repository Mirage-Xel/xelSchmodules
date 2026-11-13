using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : IComparable<Key>
{
    public int Sym1;
    public int Sym2;
    public Key(int sym1, int sym2)
    {
        Sym1 = sym1;
        Sym2 = sym2;
    }
    public int CompareTo(Key other)
    {

        if (other == null) return 1;
        if ((Sym1 / 9 + Sym2 / 9) / 2.0 != (other.Sym1 / 9 + other.Sym2 / 9) / 2.0 ? (Sym1 / 9 + Sym2 / 9) / 2.0 > (other.Sym1 / 9 + other.Sym2 / 9) / 2.0 : (Sym1 % 3 + Sym2 % 3) / 2.0 != (other.Sym1 % 3 + other.Sym2 % 3) / 2.0 ? (Sym1 % 3 + Sym2 % 3) / 2.0 > (other.Sym1 % 3 + other.Sym2 % 3) / 2.0 : (Sym1 / 3 % 3 + Sym2 / 3 % 3) / 2.0 != (other.Sym1 / 3 % 3 + other.Sym2 / 3 % 3) / 2.0 ? (Sym1 / 3 % 3 + Sym2 / 3 % 3) / 2.0 > (other.Sym1 / 3 % 3 + other.Sym2 / 3 % 3) / 2.0 : Mathf.Min(Sym1, Sym2) > Mathf.Min(other.Sym1, other.Sym2)) return 1;
        if (Equals(other)) return 0;
        return -1;
    }
    public override bool Equals(object other)
    {
        return other is Key && Equals((Key)other);
    }
    public bool Equals(Key other)
    {
        return Sym1 == other.Sym1 && Sym2 == other.Sym2;
    }
}