using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdMath {

    public static bool IsIntMatch(int a, int b, int decrease = 0, int increase = 0) {
        if(decrease != 0 || increase != 0) {
            return (a-decrease) < b && b < (a+increase); 
        }
        return a == b;
    }

    public static bool IsArrayNullOrEmpty<T>(T[] array) {
        return array == null || array.Length == 0;
    }
}
