using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : IComparer<SaveData>
{
    public int Compare(SaveData x, SaveData y)
    {
        int n;
        n = x.score.CompareTo(y.score);
        return -n;
    }
}