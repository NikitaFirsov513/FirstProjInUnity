using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalcEgg : MonoBehaviour
{
    
    
    
    private static int sum=0;
    public static int getSum()
    {
        return sum;
    }

    public static void setSum(int newSum)
    {
        sum=newSum;
    }

    public static void addSum()
    {
        sum += 1;
    }
}
