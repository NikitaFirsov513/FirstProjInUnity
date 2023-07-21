using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpeedConv : MonoBehaviour
{
    //private static float speedConv = 0.015f;
    //private static float speedConv = 0.05f;
    //private static float speedConv = 0.07f;
    //public static float speedConv = 0.1f;
    //public static float speedConv = 0.15f;
    //public static float speedConv = 0.175f;
    private static float speedConv = 0.0166667f;

    public static float getSpeedConv()
    {
        return speedConv;
    }
    public static void IncSpeedConv(float newVar)
    {
        speedConv += newVar;
        //GlobalVar.setSensorUpdateDelay(speedConv);
    }
    public static void DicSpeedConv(float newVar)
    {
        speedConv -= newVar;
        //GlobalVar.setSensorUpdateDelay(speedConv);
    }

    public static void setSpeedConv(float speed)
    {
        speedConv = 0.01f / speed;
    }
}
