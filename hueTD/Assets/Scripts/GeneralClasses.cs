using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일반 자료구조
/// </summary>
public class Pair<T, U> {
    public Pair() {
    }

    public Pair(T first, U second) {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};
